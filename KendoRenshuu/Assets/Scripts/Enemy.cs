using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public static float EnemyMoveSpeed = .01f;
    void OnDestroy()
    {
        GameManager.GM.Score++;
        GameManager.GM.NumActiveEnemies--;
        EnemyMoveSpeed += 0.001f;
    }
}
