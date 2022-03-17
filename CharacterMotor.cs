using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMotor : MonoBehaviour
{
    PlayerInventory playerInv;
    Animation animations;

    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public float attackCooldown;
    private bool isAttacking;
    private float currentCooldown;

    public Vector3 jumpSpeed;
    CapsuleCollider playerCollider;

    public bool isDead = false;
    public float attackRange;
    private GameObject rayHit;
    private GameObject enemy;

    [Header("Paramètre des sorts")]
    private GameObject raySpell;
    private GameObject SpellHolderImg;
    private int currentSpell = 1;
    public int totalSpell;

    public float lightningspellCost;
    public GameObject lightningspellGO;
    public float lightningspellSpeed;
    public int lightningSpellID;
    public Sprite lightningSpellImage;

    public float healSpellCost;
    public float healSpellAmount;
    public GameObject healSpellGO;
    public int healSpellID;
    public Sprite healSpellImage;

    void Start()
    {
        animations = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        rayHit = GameObject.Find("RayHit");
        raySpell = GameObject.Find("RaySpell");
        playerInv = gameObject.GetComponent<PlayerInventory>();
        SpellHolderImg = GameObject.Find("SpellHolderImg");
    }
    bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.09f, 3);
    }

    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, walkSpeed * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    attack();
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    spell();
                }

            }
            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, runSpeed * Time.deltaTime);
                animations.Play("run");
            }
            if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    attack();
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    spell();
                }
            }
            if (Input.GetKey(inputLeft))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
            }
            if (Input.GetKey(inputRight))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }
            if (!Input.GetKey(inputBack) && !Input.GetKey(inputFront))
            {
                if (!isAttacking)
                {
                    animations.Play("idle");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    attack();
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    spell();
                }

            }
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
                v.y = jumpSpeed.y;

                gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
            }

            if (isAttacking)
            {
                currentCooldown -= Time.deltaTime;
            }
            if (currentCooldown <= 0)
            {
                currentCooldown = attackCooldown;
                isAttacking = false;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentSpell <= totalSpell && currentSpell != 1)
                {
                    currentSpell -= 1;
                }
            }

            // forward
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentSpell >= 0 && currentSpell != totalSpell)
                {
                    currentSpell += 1;
                }
            }
            if (currentSpell == lightningSpellID)
            {
                SpellHolderImg.GetComponent<Image>().sprite = lightningSpellImage;
            }
            if (currentSpell == healSpellID)
            {
                SpellHolderImg.GetComponent<Image>().sprite = healSpellImage;
            }
        }
    }
    public void attack()
    {
        if (!isAttacking)
        {
            animations.Play("attack");
            RaycastHit hit;
            if (Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
            {
                Debug.DrawLine(rayHit.transform.position, hit.point, Color.red);
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<enemyAI>().ApplyDammage(playerInv.currentDamage);

                }
            }
            isAttacking = true;
        }
    }

    public void spell()
    {
        if (currentSpell == lightningSpellID && !isAttacking && playerInv.currentMana >= lightningspellCost)
        {
            animations.Play("attack");
            GameObject lightningSpell = Instantiate(lightningspellGO, raySpell.transform.position, transform.rotation);
            lightningSpell.GetComponent<Rigidbody>().AddForce(transform.forward * lightningspellSpeed);
            playerInv.currentMana -= lightningspellCost;
            isAttacking = true;
        }
        if (currentSpell == healSpellID && !isAttacking && playerInv.currentMana >= healSpellCost && playerInv.currentHealth < playerInv.maxHealth)
        {
            animations.Play("attack");
            Instantiate(healSpellGO, raySpell.transform.position, transform.rotation);
            playerInv.currentMana -= healSpellCost;
            playerInv.currentHealth += healSpellAmount;
            isAttacking = true;

        }
    }
}

