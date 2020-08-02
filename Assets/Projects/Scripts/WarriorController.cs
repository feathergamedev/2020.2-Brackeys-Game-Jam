using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarriorController : MonoBehaviour
{
    [SerializeField]
    private ActionController m_actionController;

    public Vector2Int Coordinate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            TakeAction();
    }

    private void TakeAction()
    {
        if (m_actionController.HasNextAction() == false)
            return;

        var nextDirecion = m_actionController.GetNextAction();
        var nextCoordinate = Coordinate + nextDirecion.ToCoordinate();

        var targetPos = BoardManager.instance.GetGridPos(nextCoordinate);

        transform.DOMove(targetPos, 0.2f).SetEase(Ease.Linear);

        Coordinate = nextCoordinate;
    }
}
