using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    public float speed = 10f;   // this is the projectile's speed
    public float lifetime = 3f; // this is the projectile's lifespan (in seconds)
    
    public Transform exploPos;


    public bool knockBack = false;
    public bool sphereExplosion;
    public bool boxExplosion;
    public bool singleTargetExplosion;

    public float damage;
    public float power;
    public float radius;
    public float upwardMod;

    Explosion explosiveScript;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if(exploPos == null)
        {
            exploPos = transform;
        }

        if (knockBack)
        {
            explosiveScript = GetComponent<Explosion>();
            explosiveScript.exploPos = exploPos;
        }
        else
            explosiveScript = null;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(gameObject, lifetime);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        print("Wooop");
    //        MovementScript player = collision.gameObject.GetComponent<MovementScript>();
    //        player.hitpoint -= 1;
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            MoveScript player = other.gameObject.GetComponent<MoveScript>();
            ExplosionStyle(other, player, damage);

            Destroy(gameObject);
        }
    }

    void ExplosionStyle(Collider other, MoveScript player, float damage)
    {
        if (knockBack)
        {
            if (boxExplosion)
            {
                explosiveScript.AoeExplosionBox(power, radius, upwardMod, damage);
            }
            else if (sphereExplosion)
            {
                explosiveScript.AoeExplosionSphere(power, radius, upwardMod, damage);
            }
            else if (singleTargetExplosion)
            {
                explosiveScript.SingleTargetExplosion(other, power, radius, upwardMod, damage);
            }
            return;
        }
    }
}
