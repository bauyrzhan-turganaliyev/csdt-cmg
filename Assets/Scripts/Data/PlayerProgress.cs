using System;
using System.Collections.Generic;
using Configs;
using UnityEngine;

public class PlayerProgress
{
    public ScoreData ScoreData;
    public GridData GridData;

    public PlayerProgress()
    {
        ScoreData = new ScoreData();
        GridData = new GridData();
    }
}

[Serializable]
public class CardData
{
    public int contentIndex;
    public bool isMatched;
}

[Serializable]
public class GridData
{
    public int width;
    public int height;
    public PoolType PoolType;
    public List<CardData> cards;
}