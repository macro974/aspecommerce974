using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MC3Shopper.Models
{
    public class Security
    {
        // classe serialization binaire 
        public static string Serialize<T>(T obj)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            byte[] buff = ms.ToArray();
            ms.Close();

            return Convert.ToBase64String(buff);
        }

        public static T DeSerialize<T>(string val)
        {
            byte[] buff = Convert.FromBase64String(val);
            var bf = new BinaryFormatter();
            var ms = new MemoryStream(buff);
            var result = (T) bf.Deserialize(ms);
            ms.Close();

            return result;
        }

        public static string chiffrer(string val)
        {
            return null;
        }
    }
}