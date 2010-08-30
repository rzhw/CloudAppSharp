using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CloudAppSharp
{
    internal class JsonHelper
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
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
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
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }
    }
}
