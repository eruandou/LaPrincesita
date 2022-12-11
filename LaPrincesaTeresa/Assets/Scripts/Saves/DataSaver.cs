using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Saves
{
    public class DataSaver
    {
        public void Initialize()
        {
            LoadSaveData();
        }

        private SaveData _currentSaveData;
        private const string SaveDataFolder = "SaveData/";
        private const string SaveDataName = "data01.json";
        private bool _saveDataFound;

        public bool GetSaveDataFound()
        {
            return _saveDataFound;
        }

        public SaveData GetCurrentSaveData() => _currentSaveData;

        public void SaveData()
        {
            //Check folder
            var saveFolder = GetDataFolderPath();
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            var path = GetFullSaveDataPath();
            var jsonFile = JsonUtility.ToJson(_currentSaveData, true);
            File.WriteAllText(path, jsonFile);
        }

        private static string GetFullSaveDataPath()
        {
            return string.Concat(GetDataFolderPath(), SaveDataName);
        }

        private static string GetDataFolderPath()
        {
            return string.Concat(Application.dataPath, "/", SaveDataFolder);
        }


        public void LoadSaveData()
        {
            var filePath = GetFullSaveDataPath();
            var saveExists = File.Exists(filePath);
            if (!saveExists)
            {
                _currentSaveData = new SaveData();
                _saveDataFound = false;
                return;
            }

            _saveDataFound = true;
            var saveDataText = File.ReadAllText(filePath);
            var loadedSaveData = JsonUtility.FromJson<SaveData>(saveDataText);
            _currentSaveData = loadedSaveData;
        }
    }
}