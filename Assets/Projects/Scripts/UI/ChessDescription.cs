using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChessDescription : MonoBehaviour
{
    public static ChessDescription instance;

    [SerializeField]
    private Text m_description;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDescription(ChessType type, Vector2Int coordinate)
    {
        var pos = BoardManager.instance.GetGridPos(coordinate);

        switch (type)
        {
            case ChessType.Stone:
                m_description.text = "You shall not pass!";
                break;
            case ChessType.TreasureBox:
                m_description.text = "Will automatically open when pass by.";
                break;
        }

        transform.position = pos;
    }
}
