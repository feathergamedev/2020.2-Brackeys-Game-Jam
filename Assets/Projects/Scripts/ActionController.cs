using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    private List<Direction> m_actionRecord;
    private int m_actionIndex;

    [Header("移動路線標記")]

    public Vector2Int CurCoordinate;

    private List<Image> m_allFootprints;

    [SerializeField]
    private Transform m_footprintParent;

    [SerializeField]
    private Image m_footprintPrefab;

    [SerializeField]
    private GameObject m_curPosIndicator;

    [Header("步驟記錄")]

    [SerializeField]
    private Image m_arrowPrefab;

    [SerializeField]
    private Transform m_arrowParent;

    private List<Image> m_actionIcons;

    [SerializeField]
    private float m_arrowIconDistance;

    private void Awake()
    {
        CurCoordinate = Vector2Int.zero;
        m_actionRecord = new List<Direction>();
        m_actionIcons = new List<Image>();
        m_allFootprints = new List<Image>();        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    public void PlayerInput()
    {
        if (GameManager.instance.CurGameState != GameState.Edit)
            return;

        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") > 0)
                AddAction(Direction.Right);
            else
                AddAction(Direction.Left);
        }

        if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxis("Vertical") > 0)
                AddAction(Direction.Up);
            else
                AddAction(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
            DeleteAction();
    }

    public bool HasNextAction()
    {
        return (m_actionIndex < m_actionRecord.Count);
    }

    public Direction GetNextAction()
    {
        return m_actionRecord[m_actionIndex++];
    }

    public void ResetActionIndex()
    {
        m_actionIndex = 0;
    }

    public void AddAction(Direction dir)
    {
        var nextCoordinate = CurCoordinate + dir.ToCoordinate();

        if (BoardManager.instance.IsWalkable(nextCoordinate) == false)
            return;

        CreateArrowIcon(dir);
        m_actionRecord.Add(dir);

        //如果不是起點，就畫腳印
        //TODO:把[0,0]改成動態吃每一關不同的起始座標
//        if (CurCoordinate != Vector2Int.zero)
//        {
            DrawFootprint(CurCoordinate, dir);
//        }

        CurCoordinate = nextCoordinate;

        UpdateCurPosIndicator(CurCoordinate);

        SoundManager.instance.PlaySound(SoundType.InsertAction);
    }

    public void DeleteAction()
    {
        var lastIdx = m_actionRecord.Count - 1;

        if (lastIdx < 0)
            return;

        CurCoordinate -= m_actionRecord[lastIdx].ToCoordinate();

        EraseFootprint();
        UpdateCurPosIndicator(CurCoordinate);

        Destroy(m_actionIcons[lastIdx]);
        m_actionRecord.RemoveAt(lastIdx);
        m_actionIcons.RemoveAt(lastIdx);

        m_arrowParent.transform.localPosition += new Vector3(m_arrowIconDistance/2, 0, 0);

        SoundManager.instance.PlaySound(SoundType.InsertAction);
    }

    public void DeleteAllAction()
    {
        for (int i=0; i<m_actionRecord.Count; i++)
        {
            var icon = m_actionIcons[i];
            Destroy(icon);
        }

        m_actionIcons.Clear();
        m_actionRecord.Clear();
        m_arrowParent.transform.localPosition = new Vector3(0, 37, 0);
        CurCoordinate = Vector2Int.zero;
        m_actionIndex = 0;
    }

    private void UpdateCurPosIndicator(Vector2Int pos)
    {
        m_curPosIndicator.transform.position = BoardManager.instance.GetGridPos(pos);
    }

    public void SetCurPosIndicatorActive(bool isActive)
    {
        m_curPosIndicator.SetActive(isActive);
    }

    private void DrawFootprint(Vector2Int pos, Direction dir)
    {
        var newFootprint = Instantiate(m_footprintPrefab, m_footprintParent);
        newFootprint.transform.localScale = Vector3.one;

        newFootprint.transform.position = BoardManager.instance.GetGridPos(pos);

        float rotateZ = 0f;

        switch(dir)
        {
            case Direction.Left:
                rotateZ = 90f;
                break;
            case Direction.Right:
                rotateZ = -90f;
                break;
            case Direction.Up:
                rotateZ = 0f;
                break;
            case Direction.Down:
                rotateZ = -180f;
                break;
        }

        newFootprint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));

        m_allFootprints.Add(newFootprint);
    }

    private void EraseFootprint()
    {
        var lastIdx = m_allFootprints.Count - 1;

        if (lastIdx < 0)
            return;

        Destroy(m_allFootprints[lastIdx]);
        m_allFootprints.RemoveAt(lastIdx);
    }

    private void EraseAllFootprint()    
    {
        var count = m_allFootprints.Count;

        for (int i=0; i<count; i++)
        {
            var footprint = m_allFootprints[i];
            Destroy(footprint);
        }

        m_allFootprints.Clear();
    }

    private void CreateArrowIcon(Direction dir)
    {
        var newPosX = m_arrowIconDistance * m_actionRecord.Count;
        var newArrowIcon = Instantiate<Image>(m_arrowPrefab);

        newArrowIcon.transform.SetParent(m_arrowParent);
        newArrowIcon.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        newArrowIcon.transform.localPosition = new Vector3(newPosX, 0, 0);

        switch (dir)
        {
            case Direction.Left:
                newArrowIcon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case Direction.Right:
                break;
            case Direction.Up:
                newArrowIcon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case Direction.Down:
                newArrowIcon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
        }

        var initY = m_arrowParent.transform.localPosition.y;
        m_arrowParent.transform.localPosition = new Vector3(-newPosX / 2, initY, 0);

        m_actionIcons.Add(newArrowIcon);
    }

    public void Reset(Vector2Int pos)
    {
        DeleteAllAction();
        UpdateCurPosIndicator(pos);
        SetCurPosIndicatorActive(true);
        EraseAllFootprint();
    }
}
