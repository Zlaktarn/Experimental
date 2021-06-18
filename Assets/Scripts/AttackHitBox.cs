using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public GameObject enemyHit;
    public LayerMask enemyLayer;
    BoxCollider hitbox; 

    bool enemyStruck = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHit = other.gameObject;
            print("wooopSLSLA");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == enemyLayer)
        {
            enemyHit = collision.gameObject;
            print("wooopSLSLA");
        }
    }
}
