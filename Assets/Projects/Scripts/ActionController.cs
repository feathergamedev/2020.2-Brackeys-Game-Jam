using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    private List<Direction> m_actionRecord;
    private int m_actionIndex;

    public Vector2Int CurCoordinate;

    [SerializeField]
    private Image m_arrowPrefab;

    [SerializeField]
    private Transform m_arrowParent;

    [SerializeField]
    private List<Sprite> m_arrowSprites;

    private List<Image> m_actionIcons;

    [SerializeField]
    private float m_arrowIconDistance;

    private void Awake()
    {
        CurCoordinate = Vector2Int.zero;
        m_actionRecord = new List<Direction>();
        m_actionIcons = new List<Image>();
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

        // Mac 的 delete 還要搭配 fn 才能用，所以多加一個 k 的按鈕
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteAction();
        }
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

        if (BoardManager.instance.HasGridAt(nextCoordinate) == false)
        {
            Debug.Log(string.Format("Don't have grid at [{0},{1}]", nextCoordinate.x, nextCoordinate.y));
            return;
        }

        CreateArrowIcon(dir);
        m_actionRecord.Add(dir);

        CurCoordinate = nextCoordinate;
    }

    public void DeleteAction()
    {
        var lastIdx = m_actionRecord.Count - 1;

        if (lastIdx < 0)
            return;

        CurCoordinate -= m_actionRecord[lastIdx].ToCoordinate();

        Destroy(m_actionIcons[lastIdx]);
        m_actionRecord.RemoveAt(lastIdx);
        m_actionIcons.RemoveAt(lastIdx);

        m_arrowParent.transform.localPosition += new Vector3(m_arrowIconDistance/2, 0, 0);
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
        m_arrowParent.transform.localPosition = Vector3.zero;
        CurCoordinate = Vector2Int.zero;
        m_actionIndex = 0;
    }


    private void CreateArrowIcon(Direction dir)
    {
        var newPosX = m_arrowIconDistance * m_actionRecord.Count;
        var newArrowIcon = Instantiate(m_arrowPrefab);

        newArrowIcon.transform.parent = m_arrowParent;
        newArrowIcon.transform.localScale = Vector3.one;
        newArrowIcon.transform.localPosition = new Vector3(newPosX, 0, 0);

        m_arrowParent.transform.localPosition = new Vector3(-newPosX / 2, 0, 0);

        newArrowIcon.sprite = m_arrowSprites[(int)dir];
        m_actionIcons.Add(newArrowIcon);
    }
}
