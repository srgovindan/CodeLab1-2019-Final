using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    //PLAYER REF
    public GameObject _player;
    
    //GLOBAL ENEMY SPEED
    public static float EnemyMoveSpeed = .01f;
    
    //PRIVATE REFS
    private Animator Animator;
    
    //VARS
    public enum EnemyState
    {
        Move,
        Attack,
        Hurt,
    }
    public EnemyState CurrentEnemyState;
    
    void Start()
    {
        _player = GameObject.Find("Player");
        Animator = GetComponent<Animator>();
        CurrentEnemyState = EnemyState.Move; //default enemy state to move
    }
    
    public void AttackPlayer()
    {
        Animator.SetBool("attacking",true);
        CurrentEnemyState = EnemyState.Attack;
    }
    
    void DamagePlayer() //called from the animator
    {
        Debug.Log("Hit the player");
        _player.GetComponent<Player>().PlayerGotHit(); 
        EnemyMoveSpeed = 0.01f; //reset the enemy movement speed
        GameManager.GM.NumActiveEnemies--; //reduce the num of active enemies
    }
    
    public void EnemyHit()
    {
        Animator.SetBool("defeated",true);
        CurrentEnemyState = EnemyState.Hurt;
    }

}
