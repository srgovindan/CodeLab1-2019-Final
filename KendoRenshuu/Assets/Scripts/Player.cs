using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _facing;
    
    public enum PlayerState
    {
        Idle,
        Attack,
    }
    public PlayerState CurrentPlayerState;
    
    void Start()
    {
        _facing = 1;
        CurrentPlayerState = PlayerState.Idle;

    }

    void Update()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.Idle:
               _playerTurn();
               _playerAttack();
               break;
            
            case PlayerState.Attack:
                break;
        }
    }


    void _playerTurn()
    {
        if (Input.GetAxis("Horizontal") < 0 && _facing == 1)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            _facing = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0 && _facing == -1)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            _facing = 1;
        }
    }
    void _playerAttack()
    {
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
