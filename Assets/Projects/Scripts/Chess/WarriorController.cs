using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum WarriorState
{
    Move,
    Pause,
    Dead,
}

public class WarriorController : Chess
{
    public static WarriorController instance;

    public WarriorState CurState;

    [SerializeField]
    private ActionController m_actionController;

    public int CurHP;

    [SerializeField]
    private Sprite m_ghostSprite, m_normalSprite, m_deadSprite;

    private Tween m_moveTween;
    private Image m_image;

    private Coroutine m_actionCoroutine;

    private void Awake()
    {
        instance = this;

        CurState = WarriorState.Move;
        m_image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Die();
    }

    public void ReadyToTakeAction()
    {
        m_actionCoroutine = StartCoroutine(TakeAction());


    }

    public IEnumerator TakeAction()
    {
        while (CurState != WarriorState.Dead)
        {
            if (GetAdjacentInteractable() != null)
            {
                Debug.Log("Yes!");

                var chess = GetAdjacentInteractable();

                switch (chess.Type)
                {
                    case ChessType.TreasureBox:
                        chess.GetComponent<TreasureBox>().Open();
                        SoundManager.instance.PlaySound(SoundType.OpenTreasure);
                        break;
                }

                yield return new WaitForSeconds(0.45f);
            }
            else
            {
                if (m_actionController.HasNextAction() == false)
                    break;

                var nextDirecion = m_actionController.GetNextAction();
                var nextCoordinate = Coordinate + nextDirecion.ToCoordinate();

                var targetPos = BoardManager.instance.GetGridPos(nextCoordinate);

                m_moveTween = transform.DOMove(targetPos, 0.45f).SetEase(Ease.Linear);

                SoundManager.instance.PlaySound(SoundType.Walk);

                yield return new WaitForSeconds(0.45f);

                Coordinate = nextCoordinate;

                var encounteredChess = BoardManager.instance.HasChessAt(Coordinate);
                if (encounteredChess) //如果踩到東西
                {
                    OverlapProcess(encounteredChess);
                }



                yield return null;
            }

            GameManager.instance.ReadyToEnterNextCycle();
        }

        GameManager.instance.ReadyToCheckAnswer();

        yield return null;
    }

    private Chess GetAdjacentInteractable()
    {
        if (BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(1, 0)) != null)
            return BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(1, 0));
        else if (BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(-1, 0)) != null)
            return BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(-1, 0));
        else if (BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(0, 1)) != null)
            return BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(0, 1));
        else if (BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(0, -1)) != null)
            return BoardManager.instance.GetInteractableAt(Coordinate + new Vector2Int(0, -1));
        else
            return null;
    }

    private void OverlapProcess(Chess chess)
    {
        switch (chess.Type)
        {
            case ChessType.Spike:
                chess.GetComponent<Spike>().Activate();
                Die();
                break;
        }
    }

    public void Die()
    {
        m_image.sprite = m_deadSprite;
        m_image.SetNativeSize();
        CurState = WarriorState.Dead;
        SoundManager.instance.PlaySound(SoundType.Killed);
    }

    public void EnterRewindMode()
    {
        m_image.sprite = m_normalSprite;
        m_image.SetNativeSize();
    }

    public void Reset(Vector2Int initCoordinate)
    {

        if (m_actionCoroutine != null)
        {
            StopCoroutine(m_actionCoroutine);
            m_actionCoroutine = null;
        }

        if (m_moveTween != null)
        {
            m_moveTween.Kill();
        }

        var targetPos = BoardManager.instance.GetGridPos(initCoordinate);
        transform.position = targetPos;
        Coordinate = initCoordinate;

        m_image.sprite = m_ghostSprite;
        m_image.SetNativeSize();

        CurState = WarriorState.Move;
    }
}
