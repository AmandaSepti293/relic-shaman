using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{
    float timer;
    [SerializeField] private float flipWaitTime;
    [SerializeField] private float LedgeCheckX;
    [SerializeField] private float LedgeCheckY;
    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
            ChangeState(EnemyStates.Crawler_Idle);
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
       if(_collision.gameObject.CompareTag("Enemy"))
       {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        // ChangeState(EnemyStates.Crawler_Flip);
       } 
    }

    protected override void UpdateEnemyStates()
    {   
         if(health <= 0)
        {
            Death(0.05f);
        }
        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Crawler_Idle:
            Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(LedgeCheckX, 0) : new Vector3(-LedgeCheckX, 0);
            Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

            if(!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down , LedgeCheckY, whatIsGround)
            || Physics2D.Raycast(transform.position, _wallCheckDir, LedgeCheckX, whatIsGround))
            
            {
                ChangeState(EnemyStates.Crawler_Flip);
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
            case EnemyStates.Crawler_Flip:
                timer += Time.deltaTime;
                if(timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.Crawler_Idle);
                }
                break;    
        }
    }

}
