using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScorpionProjectile : MonoBehaviour
{
    [SerializeField]
    private Vector2Int m_coordinate;

    [SerializeField]
    private int m_moveSpeed;

    [SerializeField]
    private GameObject m_performer;

    private Vector2Int m_moveVector;

    private Coroutine m_moveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Vector2Int coordinate, Direction dir)
    {
        m_coordinate = coordinate;

        Quaternion newRotation = Quaternion.Euler(1, 0, 0);

        switch (dir)
        {
            case Direction.Left:
                newRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                m_moveVector = new Vector2Int(-1, 0);
                break;
            case Direction.Right:
                newRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                m_moveVector = new Vector2Int(1, 0);
                break;
            case Direction.Up:
                newRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                m_moveVector = new Vector2Int(0, 1);
                break;
            case Direction.Down:
                newRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                m_moveVector = new Vector2Int(0, -1);
                break;
        }

        m_performer.transform.rotation = newRotation;
    }

    public void Move()
    {
        m_moveCoroutine = StartCoroutine(MoveSequence());
    }

    private IEnumerator MoveSequence()
    {
        Vector3 nextPos = transform.localPosition + new Vector3(-320f, 0f, 0f);
        var lastCoordinate = m_coordinate;

        transform.DOLocalMove(nextPos, 0.45f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.225f);

        if (WarriorController.instance.Coordinate == m_coordinate - new Vector2Int(1, 0))
        {
            WarriorController.instance.Die();
            StopCoroutine(m_moveCoroutine);
            Destroy(this.gameObject);
        }

        yield return new WaitForSeconds(0.225f);

        if (WarriorController.instance.Coordinate == m_coordinate - new Vector2Int(2, 0))
        {
            WarriorController.instance.Die();
            Destroy(this.gameObject);
        }

        m_coordinate -= new Vector2Int(2, 0);

        if (BoardManager.instance.HasChessAt(m_coordinate))
        {
            var chess = BoardManager.instance.GetChessAt(m_coordinate);

            switch (chess.Type)
            {
                case ChessType.Scorpion:
                case ChessType.TreasureBox:
                case ChessType.Stone:
                    GameManager.instance.RemoveFromProjectileList(this);
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}
