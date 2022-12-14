using System;
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
        private bool _saveLoaded;

        public bool GetSaveDataFound()
        {
            return _saveDataFound;
        }

        public void SetUnlockedElement(PowerupType type, string levelID = "")
        {
            switch (type)
            {
                case PowerupType.DoubleJump:
                    _currentSaveData.UnlockDoubleJump();
                    break;
                case PowerupType.Dash:
                    _currentSaveData.UnlockDash();
                    break;
                case PowerupType.Glide:
                    _currentSaveData.UnlockGlide();
                    break;
                case PowerupType.Level:
                    _currentSaveData.UnlockLevel(levelID);
                    break;
            }

            SaveCurrentData();
        }

        public SaveData GetCurrentSaveData()
        {
            if (!_saveLoaded)
            {
                LoadSaveData();
            }

            return _currentSaveData;
        }

        public void SaveCurrentData()
        {
            SaveData(_currentSaveData);
        }

        public static void SaveData(SaveData saveData)
        {
            //Check folder
            var saveFolder = GetDataFolderPath();
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            var path = GetFullSaveDataPath();
            var jsonFile = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, jsonFile);
        }

        public void ResetSaveData()
        {
            _currentSaveData = new SaveData();
            _currentSaveData.UnlockLevel("Totorial Inicio");
            SaveData(_currentSaveData);
        }

        public static string GetFullSaveDataPath()
        {
            return string.Concat(GetDataFolderPath(), SaveDataName);
        }

        private static string GetDataFolderPath()
        {
            return string.Concat(Application.dataPath, "/", SaveDataFolder);
        }


        public void LoadSaveData()
        {
            _saveLoaded = true;
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