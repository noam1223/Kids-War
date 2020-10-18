using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public float health;

    public float speed;
    public float jump;

    public bool isAttacking = false;
    public bool isSliding = false;
    public bool facingRight = true;
    public bool isJumping = false;
    public bool isGrounded = true;
    public bool isJumpAttack = false;
    public bool isThrowing = false;
    public bool isJumpThrowing = false;
    public bool isDied = false;
    private bool isImmortal = false;
    public bool canThrowAgain = true;

    [SerializeField]
    private float immortalTime;

    private Rigidbody2D rg;
    private Animator animator;
    private BoxCollider2D myCollider;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private GameObject kunaiPrefab;

    [SerializeField]
    private Transform kunaiPosition;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private EdgeCollider2D edgeSwordCollider;

    [SerializeField]
    private List<string> damageSource;

    [SerializeField]
    private Slider hpSlider;

    private Vector2 slideOffset = new Vector2(0f, -1.3f);
    private Vector2 slideSize = new Vector2(1.56f, 2f);

    private Vector2 size;
    private Vector2 offset;

    public Transform attackPoint;
    public LayerMask enemyLayer;
    public float attackRange;




    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        edgeSwordCollider.enabled = false;
        myCollider = GetComponent<BoxCollider2D>();
        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpSlider.maxValue = health;
        hpSlider.value = health;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isJumpAttack = true;
            isAttacking = true;

            //Collider2D[] enemyDamaged = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            //for(int i = 0; i < enemyDamaged.Length; i++)
            //{
            //    enemyDamaged[i].GetComponent<Enemy>().TakeDamage();
            //}

        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isSliding = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isThrowing = true;
            isJumpThrowing = true;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isDied)
        {
            HandleMovement();

            isGrounded = IsGrounded();

            HandleAttack();

            ResetValsues();
        }

    }



    private void HandleMovement()
    {

        if(rg.velocity.y < 0)
        {
            animator.SetBool("land", true);
        }

        //if(IsGrounded() && animator.GetCurrentAnimatorStateInfo(0).IsName("Take Off"))
        //{
        //    animator.SetBool("Idle", true);

        //}

        //if(!(rg.velocity.y < 0) && this.animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
        //{
        //    animator.SetBool("land", false);
        //    animator.ResetTrigger("jump");
        //}
            

        float horizintalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(horizintalInput));

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Throw")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
        {
            Flip(horizintalInput);
            rg.velocity = new Vector2(speed * horizintalInput, rg.velocity.y);
        }


        if(isGrounded && isJumping)
        {
            isGrounded = false;
            rg.velocity = new Vector2(rg.velocity.x, jump);
            animator.SetTrigger("jump");
        }


        if(isSliding && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            animator.SetBool("slide", true);
        }

    }


    public void SetSlideFalse()
    {
        animator.SetBool("slide", false);
    }


    private void HandleAttack()
    {
        if (isAttacking && isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("attack");
            rg.velocity = Vector2.zero;
            
        }

        if(isJumpAttack && !isGrounded
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
        {
            animator.SetBool("jumpAttack", true);
        }

        if(!isJumpAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
        {
            animator.SetBool("jumpAttack", false);
        }

        if (isThrowing && isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Throw")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump Throw"))
        {
            animator.SetTrigger("throw");
            rg.velocity = Vector2.zero;

        }else if (isThrowing && !isGrounded
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump Throw")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            animator.SetBool("jumpThrow", true);
            canThrowAgain = false;

        }else if (!isThrowing && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump Throw"))
        {
            animator.SetBool("jumpThrow", false);
        }

    }



    private void Flip(float horizontal)
    {

        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }



    private void ResetValsues()
    {
        isAttacking = false;
        isSliding = false;
        isJumping = false;
        isJumpAttack = false;
        isThrowing = false;
        isJumpThrowing = false;
    }


    private bool IsGrounded()
    {
        if(rg.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(point.position, groundRadius, groundLayerMask);

                for(int i = 0; i < collider2Ds.Length; i++)
                {
                    if (collider2Ds[i].gameObject != gameObject)
                    {
                        animator.ResetTrigger("jump");
                        animator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }



    public void ThrowKunai()
    {

        GameObject kunai;

        if (facingRight)
        {
            kunai = Instantiate(kunaiPrefab, kunaiPosition.position, Quaternion.Euler(new Vector3(0, 0, -90f)));
            kunai.GetComponent<KunaiScript>().getDirection(Vector2.right);
        }
        else
        {
            kunai = Instantiate(kunaiPrefab, kunaiPosition.position, Quaternion.Euler(new Vector3(0, 0, 90f)));
            kunai.GetComponent<KunaiScript>().getDirection(Vector2.left);
        }
    }


    public IEnumerator TakeDamage()
    {
        if (!isImmortal)
        {

            health -= 10f;

            if (health <= 0f)
            {
                hpSlider.value = health;
                animator.SetTrigger("death");
                isDied = true;
            }
            else
            {
                hpSlider.value = health;
                animator.SetTrigger("damage");
                isImmortal = true;
                yield return new WaitForSeconds(immortalTime);
                isImmortal = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject triger = collision.gameObject;

        if(damageSource.Contains(triger.tag) && !isDied)
        {
            StartCoroutine(TakeDamage());
        }

        if(triger.tag == "Ground")
        {
            animator.ResetTrigger("jump");
        }
    }


    public void SwordAttackCollider()
    {
        edgeSwordCollider.enabled = !edgeSwordCollider.enabled;
    }


    public void ChangeToSlideCollider()
    {
        size = myCollider.size;
        offset = myCollider.offset;

        myCollider.size = slideSize;
        myCollider.offset = slideOffset;
    }


    public void ChangeFromSlideCollider()
    {
        myCollider.size = size;
        myCollider.offset = offset;
    }


    //public void IgnoreSightCollision(GameObject sightEnemy)
    //{
    //    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), sightEnemy.GetComponent<Collider2D>(), true);
    //}
}
