using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CloudAppSharp.Auth
{
    internal static class Helpers
    {
        private static readonly char[] HexLowerChars = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();
        private static readonly object LockForRng = new object();

        public static string HexEncode(byte[] rawbytes)
        {
            int length = rawbytes.Length;
            char[] chArray = new char[2 * length];
            int index = 0;
            int num3 = 0;

            while (index < length)
            {
                chArray[num3++] = HexLowerChars[rawbytes[index] >> 4];
                chArray[num3++] = HexLowerChars[rawbytes[index] & 15];
                index++;
            }
            return new string(chArray);
        }

        public static void GetBytes(byte[] array)
        {
            lock (LockForRng)
                RandomNumberGenerator.GetBytes(array);
        }
    }

    class CloudAppDigestAuth : IAuthenticationModule
    {
        static readonly Dictionary<string, DigestChallenge> Challengecache = new Dictionary<string, DigestChallenge>();

        #region IAuthenticationModule Members

        public Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials)
        {
            return DoAuth(challenge, request, credentials, false);
        }

        public Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
        {
            return DoAuth(null, request, credentials, false);
        }

        public bool CanPreAuthenticate
        {
            get { return true; }
        }

        public string AuthenticationType
        {
            get { return "Digest"; }
        }

        #endregion

        #region Challenge Lookup

        private static DigestChallenge Lookup(string ha1)
        {
            List<KeyValuePair<string, DigestChallenge>> results;
            lock (Challengecache)
            {
                results = Challengecache.Where(challenge => challenge.Key == ha1).ToList();
            }
            if (results.Count == 0)
                return null;
            return results.First().Value;
        }

        private static void AddChallenge(DigestChallenge challenge, string ha1)
        {
            lock (Challengecache)
            {
                Challengecache.Add(ha1, challenge);
            }
        }

        #endregion

        private static Authorization DoAuth(string challenge, WebRequest request, ICredentials credentials, bool preauth)
        {
            if (!(credentials is DigestCredentials))
                return null;

            DigestCredentials digestCredentials = (DigestCredentials)credentials;

            DigestChallenge challenge2 = preauth ? Lookup(digestCredentials.Ha1) : new DigestChallenge(challenge);

            if (challenge2 == null)
                return null;
            if (!challenge2.Qop)
                return null;

            if (challenge.Contains("stale=TRUE"))
                challenge2.UpdateNonce(challenge);

            if (preauth)
            {
                challenge2.IncreaseNonce();
            }
            else
            {
                if (string.IsNullOrEmpty(digestCredentials.Ha1))
                    digestCredentials.CalculateHa1(challenge2.Realm, challenge2.HashAlgo);
            }

            challenge2.CNonce = GetNonce(32);

            string uri = ((HttpWebRequest)request).Address.AbsolutePath;

            string ha2 =
                Helpers.HexEncode(challenge2.HashAlgo.ComputeHash(Encoding.UTF8.GetBytes(string.Format("GET:{0}", uri))));

            string response = string.Format("{0}:{1}:{2}:{3}:auth:{4}", digestCredentials.Ha1, challenge2.Nonce,
                                            challenge2.NonceCount.ToString("x8"), challenge2.CNonce, ha2);
            response = Helpers.HexEncode(challenge2.HashAlgo.ComputeHash(Encoding.UTF8.GetBytes(response)));

            if (!preauth)
                AddChallenge(challenge2, digestCredentials.Ha1);
            string auth =
                string.Format(
                    "Digest username=\"{0}\",realm=\"{1}\",nonce=\"{2}\",uri=\"{3}\",algorithm=\"{4}\",cnonce=\"{5}\",nc={6},qop=\"auth\",response=\"{7}\",opaque=\"{8}\"",
                    digestCredentials.Username, challenge2.Realm, challenge2.Nonce, uri,
                    challenge2.HashType, challenge2.CNonce, challenge2.NonceCount.ToString("x8"), response,
                    challenge2.Opaque);

            return new Authorization(auth);
        }

        private static string GetNonce(int len)
        {
            byte[] buffer = len % 2 == 0 ? new byte[len / 2] : new byte[(len / 2) + 1];
            Helpers.GetBytes(buffer);
            return Helpers.HexEncode(buffer).Substring(0, len);
        }
    }

    internal class DigestChallenge
    {
        public readonly string HashType;

        public readonly string Opaque;

        public readonly bool Qop;
        public readonly string Realm;
        public string CNonce;

        public DigestChallenge(string challenge)
        {
            if (!challenge.StartsWith("Digest"))
                throw new ArgumentException("Not a digest challenge.");
            // This is basically just barebones, and what's needed to cloudapp.
            Realm = GetInQuotes(challenge, challenge.IndexOf("realm=") + 7);
            Opaque = GetInQuotes(challenge, challenge.IndexOf("opaque=") + 8);
            Nonce = GetInQuotes(challenge, challenge.IndexOf("nonce=") + 7);
            string qop = GetInQuotes(challenge, challenge.IndexOf("qop=") + 5);
            Qop = (qop.Split(',').Contains("auth"));
            HashType = GetToComma(challenge, challenge.IndexOf("algorithm=") + 10);
            NonceCount = 1;
        }

        public HashAlgorithm HashAlgo
        {
            get
            {
                HashAlgorithm algo = (HashAlgorithm)CryptoConfig.CreateFromName(HashType);
                algo.Initialize();
                return algo;
            }
        }

        public string Nonce { get; private set; }

        public int NonceCount { get; private set; }

        private static string GetInQuotes(string original, int start)
        {
            string output = "";

            for (int i = start; i < original.Length; i++)
            {
                if (original[i] == '\"')
                    break;
                output += original[i];
            }
            return output;
        }

        private static string GetToComma(string original, int start)
        {
            string output = "";

            for (int i = start; i < original.Length; i++)
            {
                if (original[i] == ',')
                    break;
                output += original[i];
            }
            return output;
        }

        public void UpdateNonce(string challenge)
        {
            Nonce = GetInQuotes(challenge, challenge.IndexOf("nonce=") + 7);
        }

        public void IncreaseNonce()
        {

        }
    }

    public class DigestCredentials : ICredentials
    {
        private string _password;

        public DigestCredentials(string userName, string password, bool passisha1)
        {
            if (passisha1)
            {
                Username = userName;
                Ha1 = password;
                _password = null;
            }
            else
            {
                Ha1 = null;
                Username = userName;
                _password = password;
            }
        }
        public string Username { get; private set; }
        public string Ha1 { get; private set; }

        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            return (authType == "Digest" && Ha1 != null) ? new NetworkCredential(Username, Ha1) : null;
        }

        internal void CalculateHa1(string realm, HashAlgorithm hashAlgorithm)
        {
            Ha1 =
                Helpers.HexEncode(
                    hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}:{1}:{2}", Username, realm, _password))));
            _password = null;
        }
    }
}
