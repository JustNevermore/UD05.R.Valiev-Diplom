using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        public int Gold;
        public int Shards;
        public ItemSaveData[] InvItemDatas;
        public ItemSaveData[] StashItemDatas;

        public GameSaveData(int gold, int shards, int invCount, int stashCount)
        {
            Gold = gold;
            Shards = shards;
            
            InvItemDatas = new ItemSaveData[invCount];
            
            for (int i = 0; i < invCount; i++)
            {
                InvItemDatas[i] = new ItemSaveData();
            }
            
            StashItemDatas = new ItemSaveData[stashCount];
            
            for (int i = 0; i < stashCount; i++)
            {
                StashItemDatas[i] = new ItemSaveData();
            }
        }
    }
}