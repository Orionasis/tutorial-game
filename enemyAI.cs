using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public GameObject[] loots;
    private float Distance;
    private float DistanceBase;
    private Vector3 basePositions;
    public Transform Target;
    private UnityEngine.AI.NavMeshAgent agent;
    public float chaseRange;
    public float attackRepeatTime;
    public float attackRange;
    private float attackTime;
    public float TheDammage;
    private Animator animations;
    public float ennemyHealth;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animations = gameObject.GetComponent<Animator>();
        attackTime = Time.time;
        basePositions = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Target = GameObject.Find("Player").transform;
            Distance = Vector3.Distance(Target.position, transform.position);
            DistanceBase = Vector3.Distance(basePositions, transform.position);
            if (Distance > chaseRange && DistanceBase<=1)
            {
                idle();
            }
            if (Distance < chaseRange && Distance > attackRange)
            {
                chase();
            }
            if (Distance < attackRange)
            {
                attack();
            }
            if(Distance > chaseRange && DistanceBase > 1)
            {
                backBase();
            }
        }
    }
    void chase()
    {
        animations.Play("Walk");
        agent.destination = Target.position;
    }
    void attack()
    {
        agent.destination = transform.position;
        if (Time.time > attackTime)
        {
            animations.Play("Attack(1)");
            Target.GetComponent < PlayerInventory>().ApplyDamage(TheDammage);
            Debug.Log("L'ennemi a envoyé "  + TheDammage  +  " points de dégâts");
            attackTime = Time.time + attackRepeatTime;
        }
    }
    void idle()
    {
        animations.Play("Idle");
    }
    public void ApplyDammage(float TheDammage)
    {
        if (!isDead)
        {
            ennemyHealth = ennemyHealth - TheDammage;
            damagetaken();
            print(gameObject.name + " a subit " + TheDammage + " points de dégâts.");
            if (ennemyHealth <= 0)
            {
                dead();
            }
        }
    }
    public void backBase()
    {
        animations.Play("Walk");
        agent.destination = basePositions;
    }
    public void damagetaken()
    {
        animations.Play("Get_hit");
    }
    public void dead() {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        isDead = true;
        animations.Play("Dead");
        int randomNumber = Random.Range(0, loots.Length);
        GameObject finalLoot = loots[randomNumber];
        Instantiate(finalLoot, transform.position, transform.rotation);
        Destroy(transform.gameObject, 5);
    }
}
