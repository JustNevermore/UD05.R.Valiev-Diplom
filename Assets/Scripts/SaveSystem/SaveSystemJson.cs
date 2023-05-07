using UnityEngine;

namespace SaveSystem
{
    public class SaveSystemJson
    {
        private const string SAVE_KEY = "GameSaveData";

        public void SaveData(GameSaveData data)
        {
            string jsonFile = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SAVE_KEY, jsonFile);
        }

        public GameSaveData LoadData()
        {
            string jsonFile = PlayerPrefs.GetString(SAVE_KEY);
            var gameData = JsonUtility.FromJson<GameSaveData>(jsonFile);
            return gameData;
        }
    }
}