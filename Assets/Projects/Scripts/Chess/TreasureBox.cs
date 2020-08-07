using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : Chess
{

    bool IsInitiallyOpened;
    public bool IsOpened;

    public bool ExpectedState;

    [SerializeField]
    private GameObject m_closeSprite, m_openSprite;

    private void Awake()
    {
        IsInitiallyOpened = IsOpened;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        if (IsOpened)
            return;

        m_closeSprite.SetActive(false);
        m_openSprite.SetActive(true);

        IsOpened = true;
    }

    public override void Reset()
    {
        IsOpened = IsInitiallyOpened;
        m_openSprite.SetActive(IsOpened);
        m_closeSprite.SetActive(!IsOpened);
    }
}
