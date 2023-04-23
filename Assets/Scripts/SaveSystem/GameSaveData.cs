using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        // public int Gold;
        // public int Shards;
        public ItemSaveData[] SaveItemDatas;

        public GameSaveData(int count)
        {
            SaveItemDatas = new ItemSaveData[count];
            
            for (int i = 0; i < count; i++)
            {
                SaveItemDatas[i] = new ItemSaveData();
            }
        }
    }
}