namespace SaveSystem
{
    public interface ISaveSystem
    {
        public GameSaveData GameData { get; }
        public void SaveData();

        public void LoadData();
    }
}