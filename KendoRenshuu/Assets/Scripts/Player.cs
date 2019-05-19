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

    private Animator Animator;
        
    private int enemyLayerMask = 1 << 9; //layerMask should only hit enemies layer
    //attack ranges
    private float highRange = .1f;
    private float midRange = .1f;
    private float lowRange = .1f;
    
    void Start()
    {
        _facing = 1;
        CurrentPlayerState = PlayerState.Idle;
        
        Animator = GetComponent<Animator>();
        
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
        
        
//        Animator.parameters.SetValue(1,"AnimationState");
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
        CurrentPlayerState = PlayerState.Attack; //set player state to attack
        Ray2D attackRay = new Ray2D(); //create an attack ray
        RaycastHit2D attackHit = new RaycastHit2D(); //create a ray2d hit info obj to store hit info
        
        
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("high");

            Animator.SetBool("highAttack",true);
            
            attackRay = new Ray2D(transform.position,(Vector2.up + Vector2.right)*_facing*midRange);
            attackHit = Physics2D.Raycast(attackRay.origin, attackRay.direction, highRange, enemyLayerMask);
            Debug.DrawRay(attackRay.origin,attackRay.direction,Color.yellow);
            if (attackHit)
            {
                Destroy(attackHit.transform.gameObject);
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("med");
            
            Animator.SetBool("medAttack",true);
            
            attackRay = new Ray2D(transform.position,Vector2.right*_facing*midRange);
            attackHit = Physics2D.Raycast(attackRay.origin, attackRay.direction, midRange, enemyLayerMask);
            Debug.DrawRay(attackRay.origin,attackRay.direction,Color.yellow);
            if (attackHit)
            {
              Debug.Log("Hit Enemy");
              Destroy(attackHit.transform.gameObject);
                  //TODO: call a enemy destroyed function on the enemy
              
            }
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("low");
            
            Animator.SetBool("lowAttack",true);
            
            attackRay = new Ray2D(transform.position,(Vector2.down + Vector2.right)*_facing*midRange);
            attackHit = Physics2D.Raycast(attackRay.origin, attackRay.direction, lowRange, enemyLayerMask);
            Debug.DrawRay(attackRay.origin,attackRay.direction,Color.yellow);
            if (attackHit)
            {
                Destroy(attackHit.transform.gameObject);
            }
        }


        CurrentPlayerState = PlayerState.Idle; //set player state back to idle 
        
        
    }


    void _resetAnimationBools()
    {
        Animator.SetBool("idle",true);
        Animator.SetBool("lowAttack",false);
        Animator.SetBool("medAttack",false);
        Animator.SetBool("highAttack",false);
    }
}
