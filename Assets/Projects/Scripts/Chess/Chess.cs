using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType
{
    Warrior,
    Scorpion,
    TreasureBox,
    Stone,
    Spike,
    FinishPoint,
}

public class Chess : MonoBehaviour
{
    public ChessType Type;
    public Vector2Int Coordinate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void GetClicked()
    {
        Debug.LogWarning("Get Clicked not implemented.");
    }

    public virtual void Reset()
    {

    }
}
