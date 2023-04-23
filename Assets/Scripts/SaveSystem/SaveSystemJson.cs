using UnityEngine;

namespace SaveSystem
{
    public class SaveSystemJson
    {
        private const string SAVE_KEY = "GameSaveData";

        private GameSaveData _gameData;

        public GameSaveData GameData => _gameData;

        public void SaveData(GameSaveData data)
        {
            string jsonFile = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SAVE_KEY, jsonFile);
        }

        public void LoadData()
        {
            string jsonFile = PlayerPrefs.GetString(SAVE_KEY);
            _gameData = JsonUtility.FromJson<GameSaveData>(jsonFile);
        }
    }
}