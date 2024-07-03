using System;

[Serializable]
public class ScoreData
{
    public int Score;
    public int Flips;
    public int Combo;

    public ScoreData()
    {
        Combo = 1;
    }
}