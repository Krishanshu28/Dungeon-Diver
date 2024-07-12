using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public int mana = 100;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    [Header("Aimimg Part")]
    private Camera _cam;
    public ThrowAttack throwAttack;
    public GameObject throwSwordPos;

    //Dash Control
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;
    public float DashCollisionOffset = 1.5f;


    //Inventory Script ref
    public InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        manaRecover();
        
    }

    private void Update()
    {

        if (isDashing )
            return;

        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        /*Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
       // throwSwordPos.transform.up = mousePosition - rb.position;

        //transform.up = Vector3.MoveTowards(transform.up,mousePosition, _rotationSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(throwAttack, throwSwordPos.transform.position, Quaternion.identity).Init(throwSwordPos.transform.up);
        }*/
    }

    private void FixedUpdate()
    {
        if(isDashing )
        {
            return;
        }
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {
                        
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            //Direction to sprite
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
               
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;

            }
        }
        
    }

    private bool TryMove(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                
                /*if(Input.GetKey(KeyCode.T) && canDash)
                {
                    print("Dashing");
                    StartCoroutine(Dash(direction));
                }*/
               
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else { return false; }
        
    }
    void OnLook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        /*Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;*/
        throwSwordPos.transform.up = mousePosition - rb.position;
    }
    void OnAimPad(InputValue aimInput)
    {
        Vector2 aim = aimInput.Get<Vector2>();
        throwSwordPos.transform.up = aim;
    }

    void OnSecondary_Fire()
    {
        animator.SetTrigger("LongswordAttack");
        if (!inventoryManager.menuActivated && mana >= 10)
        {
            mana -= 10;
            Instantiate(throwAttack, throwSwordPos.transform.position, Quaternion.identity).Init(throwSwordPos.transform.up);
        }
        
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>().normalized;
    }
    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
    #region Dash Control
    void OnDash()
    {
        if (canDash)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryDash(movementInput);

                if (!success)
                {
                    success = TryDash(new Vector2(movementInput.x, 0));
                    if (!success)
                    {

                        success = TryDash(new Vector2(0, movementInput.y));
                    }
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            //Direction to sprite
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;

            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;

            }
        }
    }

    private bool TryDash(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + DashCollisionOffset);
            if (count == 0)
            {

                
                   StartCoroutine(Dash(direction));
                

                //rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else { return false; }

    }
    #endregion

    #region SwordAttack
    public void SwordAttack()
    {
        print("SwordAttt");
        LockMovement();
        if (spriteRenderer.flipX == true)
            swordAttack.AttackLeft();
        else 
            swordAttack.AttackRight();
    }

    public void EndSwordAttack()
    {
        UnLockMovememt();
        swordAttack.StopAttack();
    }    
    public void LockMovement()
    {
        canMove = false;
    }

    public void UnLockMovememt()
    {
        canMove = true;
    }
    #endregion
    private IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        canDash = false;
        // dash krne se obstacles ke aar paar ja rha hai
        if (canMove)
            rb.velocity = new Vector2(direction.x * dashSpeed, direction.y * dashSpeed);
        else
            rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void manaRecover()
    {
        if (mana < 100)
        {
            mana+=2;
        }
        Invoke("manaRecover",1);

    }
    public void Death()
    {
        Time.timeScale = 0;
        Destroy(gameObject);
    }
}
