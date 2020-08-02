using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    private Grid[,] m_grids;

    public List<Grid> LevelInfo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_grids = new Grid[5, 5];

        for (int i=0; i<LevelInfo.Count; i++)
        {
            var grid = LevelInfo[i];
            m_grids[grid.Coordinate.x, grid.Coordinate.y] = grid;

            Debug.Log(string.Format("[{0},{1}]", grid.Coordinate.x, grid.Coordinate.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasGridAt(Vector2Int coordinate)
    {
        if (coordinate.y < 0 || coordinate.y >= m_grids.GetLength(1))
            return false;

        if (coordinate.x < 0 || coordinate.x >= m_grids.GetLength(0))
            return false;

        return (m_grids[coordinate.x, coordinate.y] != null);
    }

    public Vector3 GetGridPos(Vector2Int coordinate)
    {
        Debug.Assert(HasGridAt(coordinate) == true);

        return m_grids[coordinate.x, coordinate.y].transform.position;
    }
}
