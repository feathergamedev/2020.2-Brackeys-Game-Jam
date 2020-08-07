using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

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

    [Header("目標清單")]

    [SerializeField]
    private GoalChecker m_warriorChecker;

    [SerializeField]
    private GoalChecker m_treasureBoxChecker;

    [SerializeField]
    private GoalChecker m_scorpionChecker;

    [Header("回合數相關")]

    public int CurCycle;

    [Header("蠍子投射物")]

    [SerializeField]
    private List<ScorpionProjectile> m_allProjectiles;

    [Header("Rewind Mode相關")]
    [SerializeField]
    private PostProcessVolume m_postProcessingVolume;

    [SerializeField]
    private AudioSource m_BGM;

    [SerializeField]
    private Animator m_eyeAnimator;

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

        if (GameManager.instance.CurGameState == GameState.Edit)
        {
            if (Input.GetKeyDown(KeyCode.I))
                PeekStageInitState();
            if (Input.GetKeyUp(KeyCode.I))
                ResumeStageFinalState();
        }
    }

    public void LevelComplete()
    {
        m_levelCompletePage.SetActive(true);
    }

    public void StartRewind()
    {
        StartCoroutine(StartRewindTransition());

        SoundManager.instance.PlaySound(SoundType.ClickButton);
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
    }

    public void StopRewind()
    {
        SoundManager.instance.PlaySound(SoundType.ClickButton);

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

        m_warriorChecker.Reset();
        m_treasureBoxChecker.Reset();
        m_scorpionChecker.Reset();

        ResumeStageFinalState();

        foreach (var p in m_allProjectiles)
            if (p != null)
                Destroy(p.gameObject);

        m_allProjectiles.Clear();

        CurCycle = 0;

        CurGameState = GameState.Edit;
    }

    public void ResetLevel()
    {
        SoundManager.instance.PlaySound(SoundType.ClickButton);

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

        CurStage.Initialize();

        BoardManager.instance.BoardSetup(CurStage.m_gridParent, CurStage.m_chessParent);

        m_startButton.SetActive(true);
        m_stopButton.SetActive(false);

        m_warrior.Reset(CurStage.WarriorInitPos);
        m_actionController.Reset(CurStage.WarriorInitPos);

        // Goal Checker
        m_warriorChecker.Reset();
        m_treasureBoxChecker.Reset();
        m_scorpionChecker.Reset();

        PrepareGoalChecker();

        m_allProjectiles.Clear();

        CurCycle = 0;

        m_postProcessingVolume.weight = 0f;
        m_BGM.pitch = 1f;

        CurGameState = GameState.Edit;
    }

    private void PrepareGoalChecker()
    {
        m_treasureBoxChecker.SetCheckerActive(CurStage.HaveTreasureBox);
        m_scorpionChecker.SetCheckerActive(CurStage.HaveScorpion);
    }

    public void PeekStageInitState()
    {
        if (CurGameState != GameState.Edit)
            return;

        CurStage.PeekInitState();

        DOTween.To(()=>m_postProcessingVolume.weight, x=>m_postProcessingVolume.weight = x, 1.0f, 0.5f);
        DOTween.To(() =>m_BGM.pitch, x => m_BGM.pitch = x, 0.85f, 0.5f);
//        DOTween.To(() => m_BGM.volume, x => m_BGM.volume = x, 0.5f, 0.5f);

        m_eyeAnimator.SetBool("Open", true);

    }

    public void ResumeStageFinalState()
    {
        CurStage.ResumeFinalState();

        DOTween.To(() => m_postProcessingVolume.weight, x => m_postProcessingVolume.weight = x, 0.0f, 0.5f);
        DOTween.To(() => m_BGM.pitch, x => m_BGM.pitch = x, 1.0f, 0.5f);
//        DOTween.To(() => m_BGM.volume, x => m_BGM.volume = x, 0.8f, 0.5f);

        m_eyeAnimator.SetBool("Open", false);
    }

    public void ShowStageRewindLayout()
    {
        CurStage.ShowRewindLayout();
    }

    public void ReadyToCheckAnswer()
    {
        StartCoroutine(CheckFinalAnswer());
    }

    private IEnumerator CheckFinalAnswer()
    {
        yield return new WaitForSeconds(0.5f);

        // Warrior_Goal
        var warriorGoalAchieved = (m_warrior.CurState == WarriorState.Dead && m_warrior.Coordinate == CurStage.WarriorTargetPos);

        if (warriorGoalAchieved)
            m_warriorChecker.GetChecked();
//        else
//            m_warriorChecker.NotAchieved();

        var treasureBoxGoalAchieved = true;
        if (CurStage.HaveTreasureBox)
        {
            var allTreasureBox = BoardManager.instance.GetAllTreasureBox();

            foreach(TreasureBox box in allTreasureBox)
            {
                if (box.IsOpened != box.ExpectedState)
                {
                    treasureBoxGoalAchieved = false;
                    break;
                }
            }

            if (treasureBoxGoalAchieved)
               m_treasureBoxChecker.GetChecked();
//            else
//                m_treasureBoxChecker.NotAchieved();
        }

        var scorpionGoalAchieved = true;
        if (CurStage.HaveScorpion)
        {
            /*
            if ()
                scorpionGoalAchieved.GetChecked();
            else
                scorpionGoalAchieved.NotAchieved();
            */
        }

        var isCompleted = warriorGoalAchieved && treasureBoxGoalAchieved && scorpionGoalAchieved;

        if (isCompleted)
        {
            yield return new WaitForSeconds(0.5f);

            m_levelCompletePage.SetActive(true);
            SoundManager.instance.PlaySound(SoundType.StageComplete);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            if (!warriorGoalAchieved)
                m_warriorChecker.NotAchieved();

            if (!treasureBoxGoalAchieved)
                m_treasureBoxChecker.NotAchieved();

            SoundManager.instance.PlaySound(SoundType.WrongAnswer);
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

    public void ReadyToEnterNextCycle()
    {
        foreach (ScorpionProjectile p in m_allProjectiles)
        {
            if (p != null)
                p.Move();
        }

        CurCycle++;
    }

    public void AddToProjectileList(ScorpionProjectile projectile)
    {
        m_allProjectiles.Add(projectile);
    }

    public void RemoveFromProjectileList(ScorpionProjectile projectile)
    {
        m_allProjectiles.Remove(projectile);
    }
}