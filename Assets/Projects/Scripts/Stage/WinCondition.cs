using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WinConditionType
{
    Warrior,
    TreasureBox,
    Monster,
}

[System.Serializable]
public struct WinCondition
{
    public WinConditionType type;

    [Header("-----Warrior Only-----")]

    //Warrior
    public Vector2Int DieCoordinate;

    [Header("-----TreasureBox Only-----")]

    //TreasureBox
    public TreasureBox Treasure;
    public bool IsOpened;
}
