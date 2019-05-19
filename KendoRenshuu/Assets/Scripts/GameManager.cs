using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    void Start()
    {
        //SINGLETON
        if (GM == null)
        {
            DontDestroyOnLoad(GM);
            GM = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }


    void Update()
    {
        
    }
}
