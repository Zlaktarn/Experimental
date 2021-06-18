using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MovementScript
{
    public bool blocking;
    public float attackDelay = 0.5f;

    public GameObject basicAttackHitbox;

    AttackHitBox aHitbox;

    BoxCollider weaponHitbox;

    [SerializeField]
    GameObject projectile;
    public float projectileSpeed;

    bool HeroicLeaping = false;

    Vector3 target = Vector3.zero;
    Vector3 goalPos = Vector3.zero;
    float distance = 0;

    public float sizeBar;

    void Start()
    {
        StartMethod();
        basicAttackHitbox.SetActive(false);
        weaponScript.enabled = false;

        weaponHitbox = weapon.GetComponent<BoxCollider>();
    }

    void Update()
    {
        UpdateMethod();

        sizeBar = hitpoint / maxHitpoint;
        Vector3 healthBarScale = new Vector3(1, sizeBar, 0);
        healthBar.transform.localScale = healthBarScale;

        if (!hurt && !dead)
        {
            Keybinds();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            goalPos = Vector3.zero;
            HeroicLeaping = true;
        }

        if (HeroicLeaping)
            HeroicLeap();
    }

    void FixedUpdate()
    {
        FixedUpdateMethod();

        if (!hurt && !dead)
        {
            if (!Blocking() && !attacking)
                Movement();
        }
    }

    void Keybinds()
    {
        if (Input.GetButtonDown("Fire1") && !dodging && !attacking)
        {
            BasicAttack("Attack 2 0", attackDelay);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !dodging && !attacking)
        {
            ShieldThrow();
        }
    }

    public bool Blocking()
    {
        if ((Input.GetKey(KeyCode.Mouse1)|| Input.GetButton("Dodge")) && !dodging)
        {
            graphic.anim.SetBool("Block", true);
            MousePos();
            return true;
        }
        else
        {
            graphic.anim.SetBool("Block", false);
            return false;
        }
    }

    public void BasicAttack(string animation, float attackDelay)
    {
        graphic.anim.Play(animation);
        attacking = true;
        MousePos();

        Invoke(nameof(Attacking), 0.15f);
    }

    public void ShieldThrow()
    {
        GameObject bullet;
        Vector3 bullPos = new Vector3(transform.position.x, 1, transform.position.z);
        bullet = Instantiate(projectile, bullPos, transform.rotation);
        EnemyProjectileScript bScript = bullet.GetComponent<EnemyProjectileScript>();
        bScript.speed = projectileSpeed;
    }

    public void Attacking()
    {
        //basicAttackHitbox.SetActive(true);
        //if (aHitbox.enemyHit != null)
        //    aHitbox.t 
        weaponHitbox.enabled = true;

        Invoke(nameof(AttackReset), 0.25f);
    }

    public void AttackReset()
    {
        attacking = false;
        weaponHitbox.enabled = false;
        basicAttackHitbox.SetActive(false);
    }

    void AttackCone()
    {

    }

    void HeroicLeap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        RaycastHit hit;

        if (goalPos == Vector3.zero)
        {
            if (Physics.Raycast(ray, out hit))
            {
                goalPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                distance = Vector2.Distance(new Vector3(transform.position.x, transform.position.z), new Vector3(goalPos.x, goalPos.z));

            }
        }
        else
        {
            if (transform.position == goalPos)
            {
                controller.enabled = true;
                HeroicLeaping = false;
            }
            else
            {
                float currDistance = Vector2.Distance(transform.position, goalPos);

                if(currDistance >= distance / 2)
                {
                    goalPos.y += currDistance * Time.deltaTime;
                }
                else if(currDistance >= distance / 2)
                {
                    goalPos.y -= 20 * Time.deltaTime;
                }

                transform.position = Vector3.MoveTowards(transform.position, goalPos, 20 * Time.deltaTime);



                controller.enabled = false;
                HeroicLeaping = true;
            }
        }


        //target.y = 0;
        //target.y = transform.localScale.y / 2f;
        //Vector3 telPos = hit.point;
        //    controller.enabled = false;

        //    telPos = new Vector3(hit.point.x, 3, hit.point.z);
        //    Vector3 newPos = telPos;
        //    telPos.y = 5f;

        //    if (newPos != telPos)
        //    {
        //        transform.position += newPos * Time.deltaTime / 10;
        //        if (transform.position == hit.point)
        //        {
        //        }
        //        else
        //        {
        //            moving = false;
        //        }
        //    }
        //    else
        //    {
        //        HeroicLeaping = false;
        //        controller.enabled = true;
        //    }


        //    transform.position = newPos;

        //    //controller.Move(telPos);

        //    return;
        //transform.LookAt(target);
    }
}
