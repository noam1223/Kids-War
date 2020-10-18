using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{

    private Enemy enemy;

    private float patrolTimer;

    private float patrolDuration;

    public void Enter(Enemy enemy)
    {
        patrolDuration = Random.Range(1, 10);
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        enemy.Move();

        if(enemy.Target != null)
        {
            enemy.ChangeState(new RangeState());
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D other)
    {
       
    }


    private void Patrol()
    {
        enemy.MyAnimator.SetFloat("speed", 0);

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
