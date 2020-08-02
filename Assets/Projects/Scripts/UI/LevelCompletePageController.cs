using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePageController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplayLevel()
    {
        GameManager.instance.ResetLevel();
        gameObject.SetActive(false);
    }
}
