using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoalChecker : MonoBehaviour
{

    [SerializeField]
    private GameObject m_content;

    [SerializeField]
    private Image m_checker;

    [SerializeField]
    private float m_duration, m_strength;

    [SerializeField]
    private int m_vibrato;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            NotAchieved();
    }

    public void GetChecked()
    {
        m_checker.enabled = true;
    }

    public void NotAchieved()
    {
        m_content.transform.DOShakePosition(m_duration, m_strength, m_vibrato);
    }

    public void Reset()
    {
        m_checker.enabled = false;      
    }

    public void SetCheckerActive(bool isActive)
    {
        m_content.SetActive(isActive);
    }
}
