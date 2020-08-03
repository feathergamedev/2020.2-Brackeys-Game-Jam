using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Edit,
    Rewind,
    LevelComplete,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState CurGameState;

    [SerializeField]
    private ActionController m_actionController;

    [SerializeField]
    private WarriorController m_warriorController;

    [SerializeField]
    private PuzzleManager m_puzzleManager;

    [SerializeField]
    private float m_actionCallCooldown;

    [SerializeField]
    private GameObject m_startButton, m_stopButton, m_restartButton;

    private Coroutine m_rewindCoroutine;

    [SerializeField]
    private GameObject m_levelCompletePage;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        CurGameState = GameState.Edit;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CurGameState == GameState.Edit)
                StartRewind();
            else if (CurGameState == GameState.Rewind)
                StopRewind();
        }

        if (CurGameState == GameState.Edit && Input.GetKeyDown(KeyCode.R))
            ResetLevel();
    }

    public void LevelComplete()
    {
        m_levelCompletePage.SetActive(true);
    }

    public void StartRewind()
    {
        m_rewindCoroutine = StartCoroutine(Rewind());

        m_startButton.SetActive(false);
        m_stopButton.SetActive(true);
        m_restartButton.SetActive(false);

        CurGameState = GameState.Rewind;

        m_puzzleManager.FadeOutAllFinalChess();
    }

    private IEnumerator Rewind()
    {
        while (m_actionController.HasNextAction())
        {
            m_warriorController.TakeAction();
            yield return new WaitForSeconds(m_actionCallCooldown);
        }

        yield return new WaitForSeconds(m_actionCallCooldown * 2);

        PuzzleManager.instance.CheckFinalAnswer();
    }

    public void StopRewind()
    {
        if (m_rewindCoroutine != null)
        {
            StopCoroutine(m_rewindCoroutine);
            m_rewindCoroutine = null;
        }

        m_warriorController.Reset();
        m_actionController.ResetActionIndex();

        m_startButton.SetActive(true);
        m_stopButton.SetActive(false);
        m_restartButton.SetActive(true);

        CurGameState = GameState.Edit;

        m_puzzleManager.FadeInAllFinalChess();
    }

    public void ResetLevel()
    {
        if (m_rewindCoroutine != null)
        {
            StopCoroutine(m_rewindCoroutine);
            m_rewindCoroutine = null;
        }

        m_startButton.SetActive(true);
        m_stopButton.SetActive(false);

        m_actionController.DeleteAllAction();
        m_puzzleManager.FadeInAllFinalChess();
        m_warriorController.Reset();

        CurGameState = GameState.Edit;
    }
}
