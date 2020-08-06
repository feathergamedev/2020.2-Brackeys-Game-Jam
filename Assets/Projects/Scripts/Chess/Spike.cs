using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spike : Chess
{
    private Image m_image;

    [SerializeField]
    private GameObject m_hide, m_show;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        m_hide.SetActive(false);
        m_show.SetActive(true);
    }

    public override void Reset()
    {
        m_hide.SetActive(true);
        m_show.SetActive(false);
    }
}