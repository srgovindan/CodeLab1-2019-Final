using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Keycodes")]
    public KeyCode AttackHigh;
    public KeyCode AttackMed;
    public KeyCode AttackLow;
    
    private int _facing;

    public enum PlayerState
    {
        Idle,
        Attack,
    }
    
    void Start()
    {
        _facing = 1;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0 && _facing==1)
        {
            transform.Rotate(new Vector3(0,180,0));
            _facing = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0 && _facing ==-1)
        {
            transform.Rotate(new Vector3(0,180,0));
            _facing = 1;
        }


        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("high");
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("med");
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("low");
        }
    }
}
