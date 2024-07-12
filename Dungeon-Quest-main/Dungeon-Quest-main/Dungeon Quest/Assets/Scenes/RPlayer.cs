using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

interface IInteractable
{
    void Interact();
}

public class RPlayer : MonoBehaviour
{
    public int gold = 0;
    public float moveSpeed = 5f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public int mana = 100;
    public int Maxmana = 100;
    public bool lichDied = false;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public int healthPotion = 0, manaPotion = 0;
    bool canMove = true;
    public TextMeshProUGUI HealthText, ManaText, GoldText;
    [Header("Aimimg Part")]
    private Camera _cam;
    public ThrowAttack throwAttack;
    public GameObject throwSwordPos, FireBallSpawner;
    public Health health;
    public GameObject EscMenu,Bindings,DeathScreen;
    public TextMeshProUGUI Message, DeathMessage;
    private bool Died = false;
    private int goldLost=0;
    //Dash Control
    float dashSpeed = 35f;
    float dashDuration = .2f;
    public float dashCooldown = 3f;
    bool isDashing;
    bool canDash = true;
    public float DashCollisionOffset = 1.5f;
    public Image dashCircle;
    public float dashTimer = 0f;
    public bool canDo = true, swordAudio = true, fireAudio = true;
    public Slider VolumeSlider;
    public float volume = 1f; 
    public Image fillHealth, fillMana;
    public Slider sliderHealth, sliderMana;
    // Start is called before the first frame update

    public void SavePlayer()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Diff Player") || Died)
        {
            SaveSystem.SavePlayer(this);
            GameObject tp = GameObject.FindWithTag("Teleporter");
            if(tp != null)
            {
                tp.GetComponent<Teleporter>().SavetheVillage();
            }
            Message.SetText("Progress Saved");
            Died = false;
        }
        else
        {
            Message.SetText("Cannot save in Dungeon");
        }
    }
    
    public void LoadPlayer()
    {
        if (MainMenu.newGamePlayer)
        {
            gold = 0;
            moveSpeed = 5;
            Maxmana = 100;
            healthPotion = 0;
            manaPotion = 0;
            dashCooldown = 3;
            throwAttack.damage = 10;
            swordAttack.damage = 5;
            health.maxHealth = 100;
            health.Armour = 0;
            volume = 1;
            health.currentHealth = 100;
            mana = 100;
            lichDied = false;
            MainMenu.newGamePlayer = false;
            SavePlayer();
        }
        else 
        { 
            Saving data = SaveSystem.LoadPlayerData();

            gold = data.gold;
            moveSpeed = data.moveSpeed;
            Maxmana = data.Maxmana;
            healthPotion = data.healthPotion;
            manaPotion = data.manaPotion;
            dashCooldown = data.dashCooldown;
            throwAttack.damage = data.throwdamage;
            swordAttack.damage = data.swordAttack;
            health.maxHealth = data.maxHealth;
            health.Armour = data.Armour;
            volume = data.Volume;
            lichDied = data.LichDead;
            health.currentHealth = data.Health;
            mana = data.Mana;
        }
        HealthText.text = healthPotion.ToString();
        ManaText.text = manaPotion.ToString();
        sliderMana.maxValue = (float)(Maxmana / 100);
        VolumeSlider.value = volume;
        if (health.maxHealth < 200)
        {
            sliderHealth.maxValue = (float)(health.maxHealth / 100);
        }
        else
        {
            sliderHealth.maxValue = (float)(health.maxHealth / 200);
        }
    }
    public void OnEsc(InputAction.CallbackContext context)
    {
        if(!canDo)
        { return; }
        if (context.performed)
        {
            Cursor.visible = true;
            Message.SetText("");
            EscMenu.SetActive(true);
            Time.timeScale = 0;
            canDo = false;
        }
    }
    public void Continue()
    {
        canDo = true;
        EscMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        if(Died)
        {
            SavePlayer();
        }
        SceneManager.LoadScene("Main Menu");
    }
    public void Binding()
    {
        EscMenu.SetActive(false);
        Bindings.SetActive(true);    
    }
    public void escBindng()
    {
        Bindings.SetActive(false);
    }
    public void deathContinue()
    {
        health.currentHealth = 1;
        SavePlayer();
        Time.timeScale = 1;
        health.StopRumbleNow();
        SceneManager.LoadScene("Village");
    }
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GoldText.text = gold.ToString();
        manaRecover();

    }
    void Awake()
    {
        LoadPlayer();
    }
    void Update()
    {
        print(health.currentHealth);
        print(mana);
        GoldText.text = gold.ToString();
        sliderMana.value = (float)mana / Maxmana;
        sliderHealth.value = (float)health.currentHealth / health.maxHealth;
        if (dashTimer / dashCooldown != 1)
        {
            dashTimer += Time.deltaTime;
            dashCircle.fillAmount = dashTimer/ dashCooldown;
        }
        if (TPisHolding)
        {
            LockMovement();
            TPHeldTime += Time.deltaTime;
            TPImage.fillAmount = TPHeldTime / TPHoldTime;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
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
        if (direction != Vector2.zero)
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
    public void OnLook()
    {
        if(!canDo)
        { return; }
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /*Vector2 aimDirection = mousePosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = aimAngle;*/
            throwSwordPos.transform.up = mousePosition - rb.position;

    }
    public void OnAimPad(InputAction.CallbackContext aimInput)
    {
        if (!canDo)
        { return; }
        Vector2 aim = aimInput.ReadValue<Vector2>();
        if (aim.x == 0f && aim.y == 0f)
        { return; }
        else
        { 
            throwSwordPos.transform.up = aim;
        }
    }

    public void OnSecondary_Fire(InputAction.CallbackContext context)
    {
        if (!canDo)
        { return; }
        if (context.started)
        {
            if (mana >= 10)
            {
                if (fireAudio)
                { 
                    FindObjectOfType<AudioManager>().Play("PlayerFireBall");
                }
                fireAudio = false;
                animator.SetTrigger("LongswordAttack");
                LockMovement();
            }
        }
    }
    public void FireballShoot()
    {
        
        mana -= 10;
        Instantiate(throwAttack, FireBallSpawner.transform.position, Quaternion.identity).Init(throwSwordPos.transform.up);
        animator.ResetTrigger("LongswordAttack");
        UnLockMovememt();
        fireAudio = true;
    }
    public void OnMove(InputAction.CallbackContext movementValue)
    {
        movementInput = movementValue.ReadValue<Vector2>().normalized;
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!canDo)
        { return; }
        if (context.started)
        {
            if (swordAudio)
            {
                FindObjectOfType<AudioManager>().Play("PlayerSword");
            }
            swordAudio = false;
            animator.SetTrigger("swordAttack");
        }
    }
    #region Dash Control
    public  void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }

    private bool TryDash(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + DashCollisionOffset);
            if (count == 0)
            {
                FindObjectOfType<AudioManager>().Play("PlayerDash");

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
        swordAudio = true;
    }
    public void LockMovement()
    {
        if (animator != null)
        {
            canMove = false;
            animator.SetBool("isMoving", false);
        }
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
        dashTimer = 0;
        dashCircle.fillAmount = 0;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void manaRecover()
    {
        if (mana < Maxmana)
        {
            mana += 2;
        }
        Invoke("manaRecover", 1);

    }
    public void Death()
    {
        canDo = false;
        Time.timeScale = 0;
        Died = true;
        goldLost = gold * 25/100;
        gold-=goldLost;
        DeathMessage.SetText("Gold Lost : " + goldLost);
        Cursor.visible = true;
        DeathScreen.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("HealthPick"))
        {
            FindObjectOfType<AudioManager>().Play("GoldDrop");
            healthPotion++;
            HealthText.text = healthPotion.ToString();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("ManaPick"))
        {
            FindObjectOfType<AudioManager>().Play("GoldDrop");
            manaPotion++;
            ManaText.text = manaPotion.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("GoldPick"))
        {
            FindObjectOfType<AudioManager>().Play("GoldDrop");
            gold += collision.gameObject.GetComponent<GoldDrop>().gold; 
            GoldText.text = gold.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("MaxHealthPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            health.maxHealth += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("MaxManaPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            Maxmana += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("FireballPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            throwAttack.damage += 5;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ArmourPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            health.Armour += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SpeedPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            moveSpeed++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SwordPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            swordAttack.damage += 5;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("DashPick"))
        {
            FindObjectOfType<AudioManager>().Play("Upgrade");
            dashCooldown -= 1;
            Destroy(collision.gameObject);
        }

    }
    public void OnHealUp(InputAction.CallbackContext context)
    {
        if (!canDo)
        { return; }
        if (context.performed)
        {
            if (healthPotion > 0 && health.maxHealth > health.currentHealth)
            {
                FindObjectOfType<AudioManager>().Play("Potion");
                healthPotion--;
                HealthText.text = healthPotion.ToString();
                health.Increasehealth(40);
            }
        }
    }
    public void OnManaUp(InputAction.CallbackContext context)
    {
        if (!canDo)
        { return; }
        if (context.performed)
        {
            if (manaPotion > 0 && mana != Maxmana)
            {
                FindObjectOfType<AudioManager>().Play("Potion");
                manaPotion--;
                ManaText.text = manaPotion.ToString();
                if (mana >= Maxmana - 50)
                {
                    mana = Maxmana;
                }
                else
                {
                    mana += 50;
                }
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!canDo)
        { return; }
        if (context.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z), Vector2.up, .1f);
            if (hit.collider != null)
            {
                print(hit.collider.gameObject.name);
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }

            }
        }
    }
    public Image TPImage;
    private bool TPisHolding= false;
    private float TPHoldTime = 3.5f;
    private float TPHeldTime = 0f;
    public TextMeshProUGUI PlayerMessage;
    public void OnTP(InputAction.CallbackContext context) 
    {
        if (!canDo)
        { 
            TPisHolding = false;
            FindObjectOfType<AudioManager>().Stop("Teleport");
            TPHeldTime = 0f;
            TPImage.fillAmount = 0;
            return; 
        }
        if (context.started)
        {
            FindObjectOfType<AudioManager>().Play("Teleport");
            TPisHolding = true;
            LockMovement();
        }
        if (context.performed)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Village"))
            {
                PlayerMessage.text = "Use the village teleporter";
                Invoke("resetMessage", 3f);
            }
            else if(health.currentHealth > 0)
            {
                health.StopRumbleNow();
                TPHeldTime = 0f;
                Died = true;
                SavePlayer();
                SceneManager.LoadScene("Village");
            }

        }
        if (context.canceled) 
        {
            FindObjectOfType<AudioManager>().Stop("Teleport");
            TPisHolding = false;
            TPHeldTime = 0f;
            TPImage.fillAmount = 0;
            UnLockMovememt();
        }
    }
    public void resetMessage()
    {
        PlayerMessage.text = "";
    }
}

