using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
   
    private GameObject _player;

    public float attackingRange;
    private float _distToPlayer;

    private Animator Animator;

    void Start()
    {
        _player = GameObject.Find("Player");
        Animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        _distToPlayer = Vector2.Distance(transform.position, _player.transform.position); //calculate dist to player
        if (_distToPlayer > attackingRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position,_player.transform.position, EnemyMoveSpeed);
    }


    void AttackPlayer()
    {
        Animator.SetBool("attacking",true);
    }

    void DamagePlayer() //called from the animator
    {
        //TODO: damage the player here
        Debug.Log("Hit the player");
        _player.GetComponent<Player>().PlayerGotHit();
        EnemyMoveSpeed = 0.02f;


        //TODO: then call a different animation to make the skeleton disappear 
    }
}
