using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenStoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_openStoryGameObject;

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DOTween.To(() => m_canvasGroup.alpha, x => m_canvasGroup.alpha = x, 0.0f, 0.5f);
            Destroy(gameObject, 1.0f);
        }
    }
}
