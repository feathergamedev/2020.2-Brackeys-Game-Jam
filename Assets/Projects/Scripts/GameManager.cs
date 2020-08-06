using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public StageInfo CurStage;

    public int CurStageID;

    public List<StageInfo> AllStages;

    [SerializeField]
    private ActionController m_actionController;

    [SerializeField]
    private WarriorController m_warrior;

    [SerializeField]
    private float m_actionCallCooldown;

    [SerializeField]
    private GameObject m_startButton, m_stopButton, m_restartButton;

    private Coroutine m_rewindCoroutine;

    [SerializeField]
    private GameObject m_levelCompletePage;

    [SerializeField]
    private Transform m_stageParent;

    private void Awake()
    {
        if (instance == null)
            instance = this;


    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        LoadStage(CurStageID);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            GoNextStage();


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
        StartCoroutine(StartRewindTransition());
    }

    private IEnumerator StartRewindTransition()
    {
        m_startButton.SetActive(false);
        m_stopButton.SetActive(true);
        m_restartButton.SetActive(false);

        CurGameState = GameState.Rewind;

        m_actionController.SetCurPosIndicatorActive(false);

        m_warrior.EnterRewindMode();

        ShowStageRewindLayout();

        yield return new WaitForSeconds(0.5f);

        m_warrior.ReadyToTakeAction();
        //m_rewindCoroutine = StartCoroutine(Rewind());
    }


    private IEnumerator Rewind()
    {
        yield return m_warrior.StartCoroutine("TakeAction");


        yield return new WaitForSeconds(m_actionCallCooldown * 2);

        CheckFinalAnswer();
    }

    public void StopRewind()
    {
        if (m_rewindCoroutine != null)
        {
            StopCoroutine(m_rewindCoroutine);
            m_rewindCoroutine = null;
        }

        m_warrior.Reset(CurStage.WarriorInitPos);

        m_actionController.ResetActionIndex();
        m_actionController.SetCurPosIndicatorActive(true);

        m_startButton.SetActive(true);
        m_stopButton.SetActive(false);
        m_restartButton.SetActive(true);

        BoardManager.instance.ResetAllChess();

        ResumeStageFinalState();

        CurGameState = GameState.Edit;
    }

    public void ResetLevel()
    {
        LoadStage(CurStageID);
    }

    public void LoadStage(int stageID)
    {
        if (m_rewindCoroutine != null)
        {
            StopCoroutine(m_rewindCoroutine);
            m_rewindCoroutine = null;
        }

        if (CurStage != null)
            Destroy(CurStage.gameObject);

        CurStageID = stageID;
        CurStage = Instantiate<StageInfo>(AllStages[CurStageID], Vector3.zero, Quaternion.identity, m_stageParent);

        BoardManager.instance.BoardSetup(CurStage.m_gridParent, CurStage.m_chessParent);

        m_startButton.SetActive(true);
        m_stopButton.SetActive(false);

        m_warrior.Reset(CurStage.WarriorInitPos);
        m_actionController.Reset(CurStage.WarriorInitPos);


        CurGameState = GameState.Edit;
    }

    public void PeekStageInitState()
    {
        CurStage.PeekInitState();
    }

    public void ResumeStageFinalState()
    {
        CurStage.ResumeFinalState();
    }

    public void ShowStageRewindLayout()
    {
        CurStage.ShowRewindLayout();
    }

    public void CheckFinalAnswer()
    {
        var result = (m_warrior.Coordinate == CurStage.WarriorTargetPos);

        if (result)
        {
            m_levelCompletePage.SetActive(true);
            SoundManager.instance.PlaySound(SoundType.StageComplete);
            Debug.Log("Win!");
        }
        else
        {
            Debug.Log("Not win...");
        }
    }

    public void GoNextStage()
    {
        StartCoroutine(EnterNextStagePerform());
    }

    private IEnumerator EnterNextStagePerform()
    {
        var nextStageID = (CurStageID + 1) % AllStages.Count;

        Destroy(m_stageParent.transform.GetChild(0).gameObject);

        LoadStage(nextStageID);

        yield return null;
    }
}