using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditPageController : MonoBehaviour
{

    [SerializeField]
    private GameObject m_creditPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPage()
    {
        m_creditPage.SetActive(true);
    }

    public void ClosePage()
    {
        m_creditPage.SetActive(false);
    }
}
