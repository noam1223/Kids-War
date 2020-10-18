using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health;

    private IEnemyState currentState;

    private Animator animator;

    public Animator MyAnimator { get { return animator; } }

    private Rigidbody2D myRigidbody;

    public float movementSpeed = 2f;

    public bool facingRight = true;

    private GameObject target;

    public GameObject Target { get { return target; }
        set { target = value; }
    }

    private bool isAttacking = false;

    public bool EnemyAttack
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public bool IsDamaged
    {
        get { return MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"); }
    }


    [SerializeField]
    private float meleeRange;

    public bool InMeleeRange
    {
        get
        {
            if(Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    public bool IsDead
    {
        get { return health <= 0; }
    }


    [SerializeField]
    private List<string> damageSource;



    [SerializeField]
    private EdgeCollider2D attackCollider;

    private bool isAtEndPlatform = false;

    public bool IsAtEndPlatfrom
    {
        get { return isAtEndPlatform; }
        set { isAtEndPlatform = value; }
    }

    private GameManager gameManager;

    private Slider hpSlider;

    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ChangeState(new IdleState());
        attackCollider.enabled = false;
        hpSlider = transform.Find("EnemyCanvas/HPSliderEnemy").GetComponent<Slider>();
        hpSlider.maxValue = health;
        hpSlider.value = health;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            if (!IsDamaged)
            {
                currentState.Execute();
            }

            LookAtTarget();
        }
    }


    public void LookAtTarget()
    {
        if(Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if(xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }


    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }


        currentState = newState;

        currentState.Enter(this);

    }


    public void Move()
    {

        if (!EnemyAttack && !MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {

                MyAnimator.SetFloat("speed", 1f);

                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            
        }

 

        
    }


    public Vector2 GetDirection()
    {

        if (facingRight)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject triger = other.gameObject;

        if (damageSource.Contains(triger.tag) && !IsDead)
        {
            Target = GameObject.FindGameObjectWithTag("Ninja") as GameObject;

            if (triger.tag == "Kunai")
            {
                triger.SetActive(false);
            }

            StartCoroutine(TakeDamage());
            GameObject hitEffectPrefab = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hitEffectPrefab, 1f);
        }

        if (other.gameObject.tag == "Edge")
        {
            ChangeDirection();

            if (Target != null)
            {
                Target = null;
            }
        }

        LookAtTarget();


        currentState.OnTriggerEnter(other);

    }


    public void ChangeDirection()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    public IEnumerator TakeDamage()
    {
        health -= 10f;
        hpSlider.value = health;

        if(IsDead)
        {
            hpSlider.value = health;
            animator.SetTrigger("death");
            gameManager.EnemyDeathCound();
            Destroy(this.gameObject, 2f);
            yield return null;
        }
        else
        {
            animator.SetTrigger("damage");
            transform.Translate(GetDirection() * (-0.2f));
        }
    }


    public void AttackCollider()
    {
        attackCollider.enabled = !attackCollider.enabled;
    }

}


