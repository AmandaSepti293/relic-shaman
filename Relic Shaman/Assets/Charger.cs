using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    
    float timer;
    
    [SerializeField] private float LedgeCheckX;
    [SerializeField] private float LedgeCheckY;
    [SerializeField] private float ChargeSpeedMultiplier;
    [SerializeField] private float ChargeDuration;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Charger_Idle);
        rb.gravityScale = 12f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(!PlayerController.Instance.pState.alive)
        {
            // transform.position = Vector2.MoveTowards
            //     (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
            //     speed * Time.deltaTime);
            ChangeState(EnemyStates.Charger_Idle);
        }
    }

    protected override void UpdateEnemyStates()
    {   
         if(health <= 0)
        {
            Death(0.05f);
        }
        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(LedgeCheckX, 0) : new Vector3(-LedgeCheckX, 0);
        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Charger_Idle:
           
            if(!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down , LedgeCheckY, whatIsGround)
            || Physics2D.Raycast(transform.position, _wallCheckDir, LedgeCheckX, whatIsGround))
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }

            RaycastHit2D _hit = Physics2D.Raycast(transform.position + _ledgeCheckStart, _wallCheckDir, LedgeCheckX * 10);
                if(_hit.collider != null && _hit.collider.gameObject.CompareTag("Player"))
                {
                    ChangeState(EnemyStates.Charger_Surprised);
                }

                if(transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                break;
            case EnemyStates.Charger_Surprised:
                rb.velocity = new Vector2(0, JumpForce);
                ChangeState(EnemyStates.Charger_Charge);
                break;
            case EnemyStates.Charger_Charge:
                timer += Time.deltaTime;
                if(timer < ChargeDuration)
                {
                    if(Physics2D.Raycast(transform.position, Vector2.down, LedgeCheckY, whatIsGround))
                    {
                        if(transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(speed * ChargeSpeedMultiplier, rb.velocity.y);

                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed * ChargeSpeedMultiplier, rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                }
                break;       
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        if(GetCurrentEnemyState == EnemyStates.Charger_Idle)
        {
            anim.speed = 1;
        }
        if(GetCurrentEnemyState == EnemyStates.Charger_Charge)
        {
            anim.speed = ChargeSpeedMultiplier;
        }
    }

}

