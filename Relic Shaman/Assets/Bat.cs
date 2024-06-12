using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] protected float stunDuration;
    float timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Bat_Idle);

    }

    protected override void Update()
    {
        base.Update();
        if(!PlayerController.Instance.pState.alive)
        {
            // transform.position = Vector2.MoveTowards
            //     (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
            //     speed * Time.deltaTime);
            ChangeState(EnemyStates.Bat_Idle);
        }
    }

    protected override void UpdateEnemyStates()
    {   
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Bat_Idle:
                if(_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Bat_Chase);
                }
            break;
            case EnemyStates.Bat_Chase:
                rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));
                
                flipBat();

            break;
            case EnemyStates.Bat_Stuned:
                timer += Time.deltaTime;

                if(timer > stunDuration)
                {
                    ChangeState(EnemyStates.Bat_Idle);
                    timer = 0;
                }
            break;
            case EnemyStates.Bat_Death:
                Death(5f);
            break;

        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
        if(health > 0)
        {
            ChangeState(EnemyStates.Bat_Stuned);
        }
        else
        {
            ChangeState(EnemyStates.Bat_Death);
        }
    }

    protected override void Death(float _destroyTime)
    {
        rb.gravityScale = 12;
        base.Death(_destroyTime);
    }
    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Bat_Idle);
        anim.SetBool("Chase", GetCurrentEnemyState == EnemyStates.Bat_Chase);
        anim.SetBool("Stuned", GetCurrentEnemyState == EnemyStates.Bat_Stuned);

        if(GetCurrentEnemyState == EnemyStates.Bat_Death)
        {
            anim.SetTrigger("Death");
        }
    }
    void flipBat()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    }            

}
