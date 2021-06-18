using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public GameObject healthBar;
    public float maxHitpoint;
    public float hitpoint;


    [SerializeField]
    float normalSpeed = 5;
    [SerializeField]
    float dodgeBonusSpeed = 4;
    float speed = 0;

    Vector3 playerVelocity;
    [HideInInspector]
    public CharacterController controller;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    [SerializeField]
    public PlayerGraphicScript graphic;


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

    public WeaponScript weaponScript;
    public GameObject weapon;
    Rigidbody rb;
    

    void Start()
    {
    }

    public void StartMethod()
    {
        hitpoint = maxHitpoint;
        //controller = GetComponent<CharacterController>();
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
        

        playerVelocity.y += gravityValue * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);
    }
    
    public void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(inputX, 0, inputZ);
        

        if (!dodging)
        {
            oldInputX = inputX;
            oldInputZ = inputZ;
            speed = normalSpeed;
            graphic.anim.SetBool("Sprint", false);

            move.Normalize();
            rb.MovePosition(transform.position + move * Time.deltaTime * speed);

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

        //move.Normalize();
        //controller.Move(move * Time.deltaTime * speed);

        //Graphic//
        input = Mathf.Abs(inputX) + Mathf.Abs(inputZ);
        if (input >= 1)
            input = 1;


        GraphicAnimations(input);
    }

    void Gravity()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
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
    }

    void Death()
    {
        dead = true;
        graphic.anim.Play("Dead");
        //ActivateRagdoll(true);
        controller.enabled = false;
    }

    public void Hurt(int damage)
    {
        //hurt = true;
        hitpoint -= damage;
        //graphic.anim.Play("Falling Back Death");
        Invoke(nameof(Recovered), 3.3f);
    }

    void Recovered()
    {
        hurt = false;
    }

    void Dodge()
    {
        dodgeAvailable = false;
        dodging = true;

        graphic.anim.Play("Dodge");

        Invoke(nameof(DodgeReset), dodgeDelay);
    }

    void DodgeReset()
    {
        dodging = false;
        dodgeAvailable = true;
    }

    void GraphicAnimations(float input)
    {
        graphic.input = input;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            EnemyProjectileScript pScript = other.GetComponent<EnemyProjectileScript>();
            Hurt(1);
            Destroy(other.gameObject);
        }
    }
}