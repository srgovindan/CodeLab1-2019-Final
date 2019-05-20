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

    //if the player hits the worm, the worm is destroyed, score increases, yada yada
    public void DestroyEnemy()
    {
        GameManager.GM.Score++;
        GameManager.GM.NumActiveEnemies--;
        EnemyMoveSpeed += 0.0002f;
        if (GameManager.GM.SpawnList.Count == 0)
        {
            GameManager.GM.LevelInd++; //change level index to the next level
            GameManager.GM.LevelCleared(); //tell the GM that the level was cleared
        }
        ObjectPool.Pool.AddWorm(gameObject);
    }
    
    //if the enemy hits the player, they deactivate after attack 
    public void WormHitPlayer()
    {
        GameManager.GM.NumActiveEnemies--;
        if (GameManager.GM.SpawnList.Count == 0)
        {
            GameManager.GM.LevelInd++; //change level index to the next level
            GameManager.GM.LevelCleared(); //tell the GM that the level was cleared
        }
        ObjectPool.Pool.AddWorm(gameObject);
    }
    
        
}
