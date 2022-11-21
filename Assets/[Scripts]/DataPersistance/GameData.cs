using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int totalCoins;
    public List<Enhancement> unlockedEnhancements;

    public GameData()
    {
        this.totalCoins = 0;
        unlockedEnhancements = new List<Enhancement>();
    }
}
