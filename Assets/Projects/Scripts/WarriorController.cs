using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WarriorController : Chess
{
    [SerializeField]
    private ActionController m_actionController;

    public int CurHP;

    [SerializeField]
    private Vector2Int m_initCoordinate;

    [SerializeField]
    private Sprite m_placeholderSprite, m_normalSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeAction()
    {
        if (m_actionController.HasNextAction() == false)
            return;

        var nextDirecion = m_actionController.GetNextAction();
        var nextCoordinate = Coordinate + nextDirecion.ToCoordinate();

        var targetPos = BoardManager.instance.GetGridPos(nextCoordinate);

        transform.DOMove(targetPos, 0.2f).SetEase(Ease.Linear);

        Coordinate = nextCoordinate;

        GetComponent<Image>().sprite = m_normalSprite;
    }

    public void Reset()
    {
        var targetPos = BoardManager.instance.GetGridPos(m_initCoordinate);
        transform.position = targetPos;
        Coordinate = m_initCoordinate;

        GetComponent<Image>().sprite = m_placeholderSprite;

    }
}
