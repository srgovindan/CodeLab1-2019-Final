using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private float _moveSpeed = .01f;
    private GameObject _player;


    void Start()
    {
        _player = GameObject.Find("Player");
    }

   
    void Update()
    {
        //skeleton walks towards player
        //transform.position = transform.position + new Vector3(_moveSpeed, 0, 0);
        
        
        transform.position = Vector3.MoveTowards(transform.position,_player.transform.position, _moveSpeed);
        
        //TODO: if skeleton is within attacking range, stop moving and attack player
        
        
    }

  
}
