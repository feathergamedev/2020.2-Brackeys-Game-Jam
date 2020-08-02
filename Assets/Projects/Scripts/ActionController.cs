using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    private List<Direction> m_ActionRecord;
    private int m_actionIndex;

    public Vector2Int CurCoordinate;

    [SerializeField]
    private Image m_arrowPrefab;

    [SerializeField]
    private Transform m_arrowParent;

    [SerializeField]
    private List<Sprite> m_arrowSprites;

    [SerializeField]
    private float m_arrowIconDistance;

    private void Awake()
    {
        CurCoordinate = Vector2Int.zero;
        m_ActionRecord = new List<Direction>();
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
        else if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxis("Vertical") > 0)
                AddAction(Direction.Up);
            else
                AddAction(Direction.Down);
        }
    }

    public bool HasNextAction()
    {
        return (m_actionIndex < m_ActionRecord.Count);
    }

    public Direction GetNextAction()
    {
        return m_ActionRecord[m_actionIndex++];
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
        m_ActionRecord.Add(dir);

        CurCoordinate = nextCoordinate;

        Debug.Log(string.Format("The {0}th action : {1}", m_ActionRecord.Count, dir.ToString()));
    }

    private void CreateArrowIcon(Direction dir)
    {
        var newPosX = m_arrowIconDistance * m_ActionRecord.Count;
        var newArrowIcon = Instantiate(m_arrowPrefab);

        newArrowIcon.transform.parent = m_arrowParent;
        newArrowIcon.transform.localScale = Vector3.one;
        newArrowIcon.transform.localPosition = new Vector3(newPosX, 0, 0);

        m_arrowParent.transform.localPosition = new Vector3(-newPosX / 2, 0, 0);

        newArrowIcon.sprite = m_arrowSprites[(int)dir];
    }
}
