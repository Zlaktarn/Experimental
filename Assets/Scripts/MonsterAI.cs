using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("General Stats")]
    public float maxHealth = 5;
    public float health = 5;
    public float speed = 3.5f;

    [Header("Senses")]
    public float sightRange;
    bool playerInSightRange;
    public float meleeRange;
    bool playerInMeleeRange;
    public float shootRange;
    bool playerInShootRange;
    public float shootTooCloseRange;
    bool playerTooCloseToRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;

    [Header("Melee Attacks")]

    [Header("Splash Attacks")]
    public float splashDamage = 1;
    public float splashPower;
    public float splashRadius;
    public float splashUpwardMod;
    public bool splashSphereExplosion;
    public bool splashBoxExplosion;
    public bool splashSingleTargetExplosion;
    public bool activateSplashZone;
    public GameObject splashZone;
    public float splashOffset;
    public float splashDelay = 0.8f;

    [Header("Range Attacks")]
    public bool isRange = false;
    public GameObject projectile;
    public float projectileDelay = 0.5f;
    public float projectileSpeed = 10;

    [Header("Knockback Range Attacks")]
    public float rangeDamage;
    public float rangePower;
    public float rangeRadius;
    public float rangeUpwardMod;
    public bool rangeSphereExplosion;
    public bool rangeBoxExplosion;
    public bool rangeSingleTargetExplosion;


    [Header("Patrolling")]
    public float walkPointRange;
    [HideInInspector]
    public Vector3 walkPoint;
    bool walkPointSet = false;
    bool detected = false;

    [Header("Other")]
    public GameObject healthbar;
    public LayerMask playerMask = 8;
    public LayerMask groundMask = 9;
    Transform player;
    [SerializeField]
    Animator anim;

    bool dead = false;

    protected NavMeshAgent agent;
    public CapsuleCollider[] Limbs;
    public SphereCollider head;
    public BoxCollider[] spineAndHead;

    float distance = 99;
    private Collider closestTarget;
    private Vector3 dirToTarget;

    bool staggered = false;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        ActivateRagdoll(false);
        splashZone.SetActive(false);

        health = maxHealth;
    }

    public void Behaviour()
    {
        if(!dead)
        {
            Vector3 healthVector3 = new Vector3(health / maxHealth, 1, 0);
            healthbar.transform.localScale = healthVector3;

            if(player == null)
                FindTarget();

            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
            playerInMeleeRange = Physics.CheckSphere(transform.position, meleeRange, playerMask);
            playerInShootRange = Physics.CheckSphere(transform.position, shootRange, playerMask);
            playerTooCloseToRange = Physics.CheckSphere(transform.position, shootTooCloseRange, playerMask);

            if (playerInSightRange)
                detected = true;

            //if (!playerInSightRange && !playerInAttackRange && !alreadyAttacked) Patrolling();
            if (agent.enabled)
            {
                if (!playerInSightRange) Patrolling();
                if (detected && !playerInMeleeRange && !playerInShootRange && !playerTooCloseToRange) ChasePlayer();
                if (playerInMeleeRange && detected) AttackPlayer(false);
                if (isRange)
                    if (playerInShootRange && detected && !playerInMeleeRange && !playerTooCloseToRange) AttackPlayer(true);
            }
        }
        else
        {
            anim.Play("Death");
            Destroy(healthbar);
        }
    }

    void Patrolling()
    {
        if (!walkPointSet) 
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, playerMask);

        for (int i = 0; i < colliders.Length; i++) //make a loop to check whats there
        {
            float distToTarget = Vector3.Distance(colliders[i].transform.position, transform.position);
            if (distToTarget < distance)
            {
                closestTarget = colliders[i];
                distance = distToTarget;
                player = colliders[i].transform;
            }
        }
    }

    void ResetTarget()
    {
        distance = 99;
        player = null;
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        anim.SetBool("Chasing", true);
        if(player.position != null)
            agent.SetDestination(player.position);
    }

    public void ActivateRagdoll(bool dead)
    {
        if(!dead)
        {
            for (int i = 0; i < Limbs.Length; i++)
            {
                Limbs[i].enabled = false;
            }
            for (int i = 0; i < spineAndHead.Length; i++)
            {
                spineAndHead[i].enabled = false;
            }
            head.enabled = false;

            anim.enabled = true;
            agent.enabled = true;
        }
        else
        {
            for (int i = 0; i < Limbs.Length; i++)
            {
                Limbs[i].enabled = true;
            }
            for (int i = 0; i < spineAndHead.Length; i++)
            {
                spineAndHead[i].enabled = true;
            }
            head.enabled = true;

            agent.enabled = false;
            anim.enabled = false;
        }
    }

    #region Attack
    void AttackPlayer(bool rangeAttack)
    {

        agent.SetDestination(transform.position);
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);
        //transform.LookAt(player);

        if (!alreadyAttacked)
        {
            /// Code for Animation ///
            anim.Play("Mutant Attack");
            anim.SetBool("Chasing", false);
            //////////////////////////

            splashZone.SetActive(true);
            agent.speed = 0;

            alreadyAttacked = true;
            Invoke(nameof(StartMoving), 0.5f);

            if (rangeAttack)
                Invoke(nameof(RangeAttack), projectileDelay);
            else
                Invoke(nameof(SplashAttack), 0.1f);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    #region Splash Attack
    void SplashAttack()
    {
        //Rotation of Cube
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);

        //Position splashZone at playerPos
        GameObject splashZoneObject;
        Vector3 splashPos = new Vector3(player.transform.position.x, 1, player.transform.position.z);
        splashZoneObject = Instantiate(splashZone, splashPos, transform.rotation);

        SplashZoneInitiator splashScript = splashZoneObject.GetComponent<SplashZoneInitiator>();
        splashScript.delay = splashDelay;
        splashScript.damage = splashDamage;
    }

    IEnumerator DeadlySplash(SplashZoneScript splashScript, float delay)
    {
        yield return new WaitForSeconds(delay);
        splashScript.deadly = true;
    }
    #endregion

    #region Range Attack
    void RangeAttack()
    {
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100);

        GameObject bullet;
        Vector3 bullPos = new Vector3(transform.position.x, 1, transform.position.z);
        bullet = Instantiate(projectile, bullPos, transform.rotation);
        EnemyProjectileScript bScript = bullet.GetComponent<EnemyProjectileScript>();
        bScript.damage = rangeDamage;
        KnockBackRangeMod(bScript);

        //projectile.GetComponent<Rigidbody>().velocity = Vector3.forward * 10;
    }

    void KnockBackRangeMod(EnemyProjectileScript bScript)
    {
        bScript.power = rangePower;
        bScript.radius = rangeRadius;
        bScript.upwardMod = rangeUpwardMod;
        bScript.sphereExplosion = rangeSphereExplosion;
        bScript.boxExplosion = rangeBoxExplosion;
        bScript.singleTargetExplosion = rangeSingleTargetExplosion;
        bScript.speed = projectileSpeed;
    }

    void ResetAttack()
    {
        splashZone.SetActive(false);
        alreadyAttacked = false;

        ResetTarget();
    }
    #endregion
    #endregion

    void StartMoving()
    {
        agent.speed = speed;
    }

    void Movement()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
        //transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    float Distance()
    {
        Vector2 playerPos = new Vector3(player.position.x, 0, player.position.z);
        Vector2 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
        float distance = Vector3.Distance(transform.position, player.position);

        return distance;
    }

    public void TakeDamage(float damage)
    {
        staggered = true;
        health -= damage;

        Invoke(nameof(StaggeredReset), 0.3f);

        if (health <= 0)
        {
            //ActivateRagdoll(true);
            dead = true;
            //Invoke(nameof(DestroyEnemy), 2f);
        }
    }

    void StaggeredReset()
    {
        staggered = false;
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Melee" && !staggered)
        {
            ///MANAGES DAMAGE///
            TakeDamage(1);
            ////////////////////
        }
    }
}