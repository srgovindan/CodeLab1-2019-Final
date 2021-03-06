﻿using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Attack,
        Hurt
    }
    
    public PlayerState CurrentPlayerState;

    private int _facing;
    
    private Animator Animator;
    
    private readonly int enemyLayerMask = 1 << 9; //layerMask should only hit enemies layer

    //attack ranges
    public float highRange = .1f;
    public float lowRange = .1f;
    public float midRange = .15f;

    private void Start()
    {
        _facing = 1;
        CurrentPlayerState = PlayerState.Idle;
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.Idle:
                _playerTurn();
                _playerAttack();
                break;

            case PlayerState.Attack:
                break;
            case PlayerState.Hurt:
                break;
        }
    }


    private void _playerTurn()
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

    private void _playerAttack()
    {
        CurrentPlayerState = PlayerState.Attack; //set player state to attack
        var attackRay = new Ray2D(); //create an attack ray
        var attackHit = new RaycastHit2D(); //create a ray2d hit info obj to store hit info


        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("high");
            AudioManager.AM.PlayClipName("High");
            
            Animator.SetBool("highAttack", true);

            attackRay = new Ray2D(transform.position, (Vector2.up + Vector2.right * _facing) * highRange);
            attackHit = Physics2D.Raycast(attackRay.origin, attackRay.direction, highRange, enemyLayerMask);
            Debug.DrawRay(attackRay.origin, attackRay.direction, Color.yellow);
            if (attackHit)
                attackHit.transform.gameObject.GetComponent<Enemy>().EnemyHit();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("med");
            AudioManager.AM.PlayClipName("Med");

            Animator.SetBool("medAttack", true);

            attackRay = new Ray2D(transform.position, Vector2.right * _facing * midRange);
            attackHit = Physics2D.Raycast(attackRay.origin, attackRay.direction, midRange, enemyLayerMask);
            Debug.DrawRay(attackRay.origin, attackRay.direction, Color.yellow);
            if (attackHit)
                attackHit.transform.gameObject.GetComponent<Enemy>().EnemyHit();
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("low");
            AudioManager.AM.PlayClipName("Low");

            Animator.SetBool("lowAttack", true);

            attackRay = new Ray2D(transform.position, (Vector2.down + Vector2.right * _facing) * lowRange);
            attackHit = Physics2D.Raycast(attackRay.origin, attackRay.direction, lowRange, enemyLayerMask);
            Debug.DrawRay(attackRay.origin, attackRay.direction, Color.yellow);
            if (attackHit)
                attackHit.transform.gameObject.GetComponent<Enemy>().EnemyHit();
        }

        CurrentPlayerState = PlayerState.Idle; //set player state back to idle 
    }

    public void PlayerGotHit()
    {
        CurrentPlayerState = PlayerState.Hurt;
        Animator.SetBool("hurt", true);
        GameManager.GM.Lives--;
    }


    private void _resetAnimationBools()
    {
        CurrentPlayerState = PlayerState.Idle;
        Animator.SetBool("idle", true);
        Animator.SetBool("hurt", false);
        Animator.SetBool("lowAttack", false);
        Animator.SetBool("medAttack", false);
        Animator.SetBool("highAttack", false);
    }
}