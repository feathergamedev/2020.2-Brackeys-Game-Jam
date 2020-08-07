using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageInfo : MonoBehaviour
{

    public Vector2Int WarriorInitPos;

    public Vector2Int WarriorTargetPos;

    public int BestSteps;

    public Transform m_gridParent, m_chessParent;

    [SerializeField]
    private CanvasGroup m_initGroup, m_finalGroup;

    public bool HaveTreasureBox, HaveScorpion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            PeekInitState();

        if (Input.GetKeyUp(KeyCode.I))
            ResumeFinalState();
    }

    public void Initialize()
    {
        m_finalGroup.alpha = 1.0f;
        m_initGroup.alpha = 0.0f;
    }

    public void PeekInitState()
    {
        DOTween.To(() => m_finalGroup.alpha, x => m_finalGroup.alpha = x, 0f, 0.4f);
        DOTween.To(() => m_initGroup.alpha, x => m_initGroup.alpha = x, 1f, 0.5f);
    }

    public void ShowRewindLayout()
    {
        DOTween.To(() => m_finalGroup.alpha, x => m_finalGroup.alpha = x, 0.5f, 0.4f / 2f);
        DOTween.To(() => m_initGroup.alpha, x => m_initGroup.alpha = x, 1f, 0.5f / 2f);
    }

    public void ResumeFinalState()
    {
        DOTween.To(() => m_finalGroup.alpha, x => m_finalGroup.alpha = x, 1f, 0.5f);
        DOTween.To(() => m_initGroup.alpha, x => m_initGroup.alpha = x, 0f, 0.4f);
    }
}
