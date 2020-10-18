using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{

    private Enemy enemy;

    private float attackTimer;
    private float attackDuration = 3;
    private bool canAttack = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;        
    }

    public void Execute()
    {

        Attack();

        if(!enemy.InMeleeRange)
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


    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
        {
            canAttack = true;
            enemy.EnemyAttack = false;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;
            enemy.EnemyAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
            enemy.MyAnimator.SetFloat("speed", 0);
        }

    }
}
