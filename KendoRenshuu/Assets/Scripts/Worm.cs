using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
    public float attackingRange;
    private float _distToPlayer;
   
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

    void DamagePlayer() //called from the animator
    {
        Debug.Log("Hit the player");
        _player.GetComponent<Player>().PlayerGotHit();
        EnemyMoveSpeed = 0.02f;
    }
    
    public override void DestroyEnemy()
    {
        GameManager.GM.Score++;
        GameManager.GM.NumActiveEnemies--;
        EnemyMoveSpeed += 0.001f;
        ObjectPool.Pool.AddWorm(gameObject); 
    }
}
