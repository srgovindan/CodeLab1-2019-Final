using UnityEngine;

public class Enemy : MonoBehaviour
{
    //VARS
    public enum EnemyState
    {
        Move,
        Attack,
        Hurt
    }

    //GLOBAL ENEMY SPEED
    public static float EnemyMoveSpeed = .01f;

    //PLAYER REF
    public GameObject _player;

    //PRIVATE REFS
    private Animator Animator;
    public EnemyState CurrentEnemyState;

    private void Start()
    {
        _player = GameObject.Find("Player");
        Animator = GetComponent<Animator>();
        CurrentEnemyState = EnemyState.Move; //default enemy state to move
    }

    public void AttackPlayer()
    {
        Animator.SetBool("attacking", true);
        CurrentEnemyState = EnemyState.Attack;
    }

    private void DamagePlayer() //called from the animator
    {
        Debug.Log("Hit the player");
        _player.GetComponent<Player>().PlayerGotHit();
        EnemyMoveSpeed = 0.01f; //reset the enemy movement speed
        GameManager.GM.NumActiveEnemies--; //reduce the num of active enemies
    }

    public void EnemyHit()
    {
        Animator.SetBool("defeated", true);
        CurrentEnemyState = EnemyState.Hurt;
    }
}