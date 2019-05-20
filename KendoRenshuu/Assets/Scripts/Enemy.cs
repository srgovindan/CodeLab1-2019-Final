using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public GameObject _player;
    public static float EnemyMoveSpeed = .01f;
    
    private Animator Animator;

    void Start()
    {
        _player = GameObject.Find("Player");
        Animator = GetComponent<Animator>();
    }
//    void OnDestroy()
//    {
//        GameManager.GM.Score++;
//        GameManager.GM.NumActiveEnemies--;
//        EnemyMoveSpeed += 0.001f;
//    }
    
    public void AttackPlayer()
    {
        Animator.SetBool("attacking",true);
    }
    
    public void EnemyHit()
    {
        Animator.SetBool("defeated",true);
    }

    public virtual void DestroyEnemy()
    {
        GameManager.GM.Score++;
        GameManager.GM.NumActiveEnemies--;
        EnemyMoveSpeed += 0.001f;
    }
}
