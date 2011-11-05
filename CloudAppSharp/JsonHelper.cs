using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CloudAppSharp
{
    public class JsonHelper
    {
        /// <summary>
        /// Serializes a specified object to JavaScript Object Notation (JSON) data and writes the resulting JSON to a string.
        /// Written by Chris Pietschmann.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized object in JSON form.</returns>
        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.Default.GetString(ms.ToArray());
            ms.Dispose();
            return retVal;
        }

        /// <summary>
        /// Reads a string in the JavaScript Object Notation (JSON) format and returns the deserialized object with the type designated by the specified generic type parameter.
        /// Written by Chris Pietschmann.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized JSON string as an object of type T.</returns>
        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }

        /// <summary>
        /// Reads a string from a WebResponse in the JavaScript Object Notation (JSON) format and returns the deserialized object with the type designated by the specified generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The WebReponse with the JSON string to deserialize.</param>
        /// <returns>The deserialized JSON string as an object of type T.</returns>
        public static T Deserialize<T>(System.Net.WebResponse response)
        {
            using (Stream dataStream = response.GetResponseStream())
            {
                // Only be specific with exceptions if it's a JSON structure that's part of CloudAppSharp.
                try
                {
                    return Deserialize<T>(new StreamReader(dataStream).ReadToEnd());
                }
                catch (SerializationException e)
                {
                    if (typeof(T).BaseType == typeof(CloudAppJsonBase))
                        throw new CloudAppInvalidResponseException(
                            "Response received from CloudApp is either not valid JSON or is missing expected parameters."
                                + " The service and/or its API may be down, or its API may be incompatible with this release of CloudAppSharp.",
                            e, System.Net.WebExceptionStatus.ReceiveFailure, response);
                    else
                        throw e;
                }
                catch (System.Xml.XmlException e)
                {
                    if (typeof(T).BaseType == typeof(CloudAppJsonBase) && e.StackTrace.Contains("System.Runtime.Serialization.Json"))
                        throw new CloudAppInvalidResponseException(
                            "Response received from CloudApp is malformed JSON.",
                            e, System.Net.WebExceptionStatus.ReceiveFailure, response);
                    else
                        throw e;
                }
            }
        }
    }
}
