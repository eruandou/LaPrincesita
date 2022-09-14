using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Globals
{
    public static class GlobalSerializer
    {
        private const string JSON_EXTENSION = ".arch";
        private const string BINARY_EXTENSION = ".bin";

        public static void SerializeJSON<T>(T data, string path, string fileName)
        {
            string applicationBasePath;
#if UNITY_EDITOR
            applicationBasePath = Application.dataPath;
#else
                applicationBasePath = Application.persistentDataPath;
#endif

            var fullPath = Path.Combine(applicationBasePath, path, fileName + JSON_EXTENSION);
            Directory.CreateDirectory(fullPath);
            var jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(fullPath, jsonData);
        }

        public static T DeserializeJSON<T>(string path, string fileName)
        {
            string applicationBasePath;
#if UNITY_EDITOR
            applicationBasePath = Application.dataPath;
#else
                applicationBasePath = Application.persistentDataPath;
#endif

            var fullPath = Path.Combine(applicationBasePath, path, fileName + JSON_EXTENSION);

            if (!File.Exists(fullPath))
            {
                return default;
            }

            var jsonData = File.ReadAllText(fullPath);
            var dataToLoad = JsonUtility.FromJson<T>(jsonData);
            return dataToLoad;
        }

        public static void SerializeBinary<T>(T data, string path, string fileName)
        {
            string applicationBasePath;
#if UNITY_EDITOR
            applicationBasePath = Application.dataPath;
#else
                applicationBasePath = Application.persistentDataPath;
#endif

            var fullPath = Path.Combine(applicationBasePath, path, fileName + BINARY_EXTENSION);
            Directory.CreateDirectory(fullPath);
            var file = File.Create(fullPath);
            var formatter = new BinaryFormatter();
            formatter.Serialize(file, data);
            file.Close();
        }

        public static T DeserializeBinary<T>(string path, string fileName)
        {
            string applicationBasePath;
#if UNITY_EDITOR
            applicationBasePath = Application.dataPath;
#else
                applicationBasePath = Application.persistentDataPath;
#endif

            var fullPath = Path.Combine(applicationBasePath, path, fileName + BINARY_EXTENSION);

            if (!File.Exists(fullPath)) return default;
            var file = File.OpenRead(fullPath);
            var formatter = new BinaryFormatter();
            var data = (T)formatter.Deserialize(file);
            file.Close();
            return data;
        }
    }
}