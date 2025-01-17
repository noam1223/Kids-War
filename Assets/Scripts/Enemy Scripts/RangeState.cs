﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeState : IEnemyState
{

    private Enemy enemy;

    private float throwTimer;
    private float throwDuration = 3;
    private bool canThrow = true;


    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {

        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        else if(enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        //if(other.tag == "Edge")
        //{
        //    enemy.IsAtEndPlatfrom = true;
        //}
    }
}
