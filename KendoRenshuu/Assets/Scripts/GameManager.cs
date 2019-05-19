using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    
    public Text ScoreTextUI;

    private int score; //score property sets ui whenever score changes
    public int Score
    {
        get
        {
            return score;
            
        }
        set
        {
            score = value;
            ScoreTextUI.text = "Score: " + value;
        }
    }

    void Start()
    {
        //SINGLETON
        if (GM == null)
        {
            GM = this;
            DontDestroyOnLoad(GM);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

}
