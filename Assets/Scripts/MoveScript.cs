using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public GameObject healthBar;
    public float maxHitpoint;
    public float hitpoint;
    float sizeBar;

    [SerializeField]
    float normalSpeed = 5;
    [HideInInspector]
    float sprintSpeed = 8;
    [SerializeField]
    float dodgeBonusSpeed = 4;
    public float speed = 0;

    Vector3 playerVelocity;
    [HideInInspector]
    public CharacterController controller;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    float input;
    [HideInInspector]
    public bool dodging = false;
    bool dodgeAvailable = true;
    public float dodgeDelay = 1.05f/*0.4f*/;
    public float dodgeInterval = 0.83f;

    float oldInputX = 0;
    float oldInputZ = 0;

    [HideInInspector]
    public bool attacking = false;
    [HideInInspector]
    public bool hurt = false;
    [HideInInspector]
    public bool dead = false;

    Rigidbody rb;

    public bool flying;

    public bool isGrounded;

    GameObject attackHit;
    public GameObject basicAttackHit;

    [Header("Ragdoll")]
    public BoxCollider[] ragBox;
    public CapsuleCollider[] ragCapsule;
    public SphereCollider ragSphere;
    public Animator anim;
    public Rigidbody[] ragRbs;
    public Transform hipTransform;
    BoxCollider thisBox;

    void Start()
    {
        StartMethod();
        thisBox = GetComponent<BoxCollider>();

        for (int i = 0; i < ragRbs.Length; i++)
        {
            ragRbs[i].isKinematic = true;
        }
    }

    public void StartMethod()
    {
        hitpoint = maxHitpoint;
        //ActivateRagdoll(false);
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        FixedUpdateMethod();
    }

    public void MousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 target = hit.point;
            target.y = 0;
            target.y = transform.localScale.y / 2f;

            transform.LookAt(target);
        }
    }

    public void FixedUpdateMethod()
    {
        if(!dead)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            Movement();

            Health();
        }

        Flying(flying);
    }

    void Health()
    {
        sizeBar = hitpoint / maxHitpoint;
        Vector3 healthBarScale = new Vector3(1, sizeBar, 0);
        healthBar.transform.localScale = healthBarScale;
    }


    void Flying(bool flying)
    {
        if(!flying)
        {
            for (int i = 0; i < ragBox.Length; i++)
            {
                ragBox[i].enabled = false;
            }
            for (int i = 0; i < ragCapsule.Length; i++)
            {
                ragCapsule[i].enabled = false;
            }
            ragSphere.enabled = false;
            for (int i = 0; i < ragRbs.Length; i++)
            {
                ragRbs[i].isKinematic = true;
            }
            //thisBox.enabled = true;
            anim.enabled = true;
        }
        else
        {
            transform.position = hipTransform.position;

            for (int i = 0; i < ragBox.Length; i++)
            {
                ragBox[i].enabled = true;
            }
            for (int i = 0; i < ragCapsule.Length; i++)
            {
                ragCapsule[i].enabled = true;
            }
            ragSphere.enabled = true;

            for (int i = 0; i < ragRbs.Length; i++)
            {
                ragRbs[i].isKinematic = false;
            }
            //thisBox.enabled = false;
            anim.enabled = false;
        }

    }

    public void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(inputX, 0, inputZ);
        move.Normalize();

        if(isGrounded)
        {
            if (!dodging)
            {
                oldInputX = inputX;
                oldInputZ = inputZ;
                speed = normalSpeed;

                //Direction of character
                if (move != Vector3.zero)
                {
                    gameObject.transform.forward = move;
                }
            }
            else
            {
                speed = normalSpeed + dodgeBonusSpeed;
                move = new Vector3(oldInputX, 0, oldInputZ);
            }

            rb.MovePosition(transform.position + move * Time.deltaTime * speed);

            //Graphic//
            input = Mathf.Abs(inputX) + Mathf.Abs(inputZ);
            if (input >= 1)
                input = 1;
        }
        
    }

    private void Update()
    {
        UpdateMethod();
    }

    public void UpdateMethod()
    {
        if (!hurt && !dead)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && dodgeAvailable)
            {
                Dodge();
            }
        }

        if (hitpoint <= 0 || Input.GetKeyDown(KeyCode.G))
            Death();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            BasicAttack();
    }

    void Death()
    {
        dead = true;
        //ActivateRagdoll(true);
    }

    void Dodge()
    {
        dodgeAvailable = false;
        dodging = true;

        Invoke(nameof(DodgeReset), dodgeDelay);
    }

    void DodgeReset()
    {
        dodging = false;
        dodgeAvailable = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            flying = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }

    public void BasicAttack(/*string animation, float attackDelay*/)
    {
        attacking = true;
        MousePos();

        Invoke(nameof(Attacking), 0.15f);
    }

    public void Attacking()
    {
        Vector3 attackPos = transform.position + new Vector3(0, 0, 1);
        attackHit = Instantiate(basicAttackHit, attackPos, Quaternion.identity);
        //basicAttackHitbox.SetActive(true);
        //if (aHitbox.enemyHit != null)
        //    aHitbox.t 

        Invoke(nameof(AttackReset), 0.25f);
    }

    public void AttackReset()
    {
        attacking = false;
    }
}