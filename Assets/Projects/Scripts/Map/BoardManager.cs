using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    private Dictionary<Vector2Int, Grid> m_grids;
    private Dictionary<Vector2Int, Chess> m_allChess;

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasGridAt(Vector2Int coordinate)
    {
        return m_grids.ContainsKey(coordinate);
    }

    public Chess HasChessAt(Vector2Int coordinate)
    {
        return m_allChess.ContainsKey(coordinate) ? m_allChess[coordinate] : null;
    }

    public Chess GetInteractableAt(Vector2Int coordinate)
    {

        if (m_allChess.ContainsKey(coordinate))
        {
            var chess = m_allChess[coordinate];

            Debug.Log(chess.gameObject.name);

            switch (chess.Type)
            {
                case ChessType.Scorpion:
                    //根據血量判斷要不要回傳
                    return chess;
                case ChessType.TreasureBox:
                    Debug.Log("HI");
                    if (chess.GetComponent<TreasureBox>().IsOpened == false)
                        return chess;
                    else
                        return null;
                default:
                    return null;
            }
        }

        return null;
    }

    public bool IsWalkable(Vector2Int coordinate)
    {
        if (HasGridAt(coordinate) == false)
            return false;

        if (m_allChess.ContainsKey(coordinate) == false)
            return true;

        var chess = m_allChess[coordinate];

        switch (chess.Type)
        {
            case ChessType.Stone:
            case ChessType.FinishPoint:
            case ChessType.TreasureBox:
                return false;
            default:
                return true;
        }
    }

    public Vector3 GetGridPos(Vector2Int coordinate)
    {
        if (m_grids.ContainsKey(coordinate) == false)
            return Vector3.zero;

        return m_grids[coordinate].transform.position;
    }

    public Chess GetChessAt(Vector2Int coordinate)
    {
        return m_allChess[coordinate];      
    }

    public void BoardSetup(Transform gridTransform, Transform chessTransform)
    {
        RegisterAllGrid(gridTransform);
        RegisterAllChess(chessTransform);
    }

    private void RegisterAllGrid(Transform parent)
    {
        m_grids = new Dictionary<Vector2Int, Grid>();
        for (int i = 0; i < parent.childCount; i++)
        {
            var grid = parent.GetChild(i).GetComponent<Grid>();
            m_grids.Add(grid.Coordinate, grid);
        }
    }

    private void RegisterAllChess(Transform parent)
    {
        m_allChess = new Dictionary<Vector2Int, Chess>();

        for (int i=0; i<parent.childCount; i++)
        {
            var chess = parent.GetChild(i).GetComponent<Chess>();
            m_allChess.Add(chess.Coordinate, chess);
        }
    }

    public void ChessMove(Chess chess, Vector2Int destination)
    {
        m_allChess.Remove(chess.Coordinate);
        m_allChess.Add(destination, chess);
    }

    public void ResetAllChess()
    {
        foreach(KeyValuePair<Vector2Int, Chess> kvp in m_allChess)
        {
            kvp.Value.Reset();
        }
    }
}
