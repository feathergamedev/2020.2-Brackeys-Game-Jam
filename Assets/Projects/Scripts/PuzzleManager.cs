using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    [SerializeField]
    private Transform m_finalStateParent;

    [SerializeField]
    private List<Chess> m_FinalStateChess;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RegisterAllFinalChess();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RegisterAllFinalChess()
    {
        m_FinalStateChess = new List<Chess>();
        for (int i = 0; i < m_finalStateParent.childCount; i++)
            m_FinalStateChess.Add(m_finalStateParent.GetChild(i).GetComponent<Chess>());
    }

    public void FadeOutAllFinalChess()
    {
        foreach (Chess c in m_FinalStateChess)
        {
            c.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        }
    }

    public void FadeInAllFinalChess()
    {
        foreach (Chess c in m_FinalStateChess)
        {
            c.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void CheckFinalAnswer()
    {
        var curChessList = BoardManager.instance.GetCurChess();

        var isCorrect = true;

        isCorrect = (curChessList[0].Coordinate == m_FinalStateChess[0].Coordinate);

        if (isCorrect)
        {
            GameManager.instance.LevelComplete();
        }

        //TODO : 設計確認答案的演算法：棋子種類、位置、狀態(ex.血量)
    }
}