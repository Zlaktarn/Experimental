using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    BoxCollider hitbox;
    [SerializeField]
    KnightScript player;

    Vector3 rot;

    void Start()
    {
        hitbox = GetComponent<BoxCollider>();
    }

    void Update()
    {
        //For reflecting projectiles
        rot = transform.rotation.eulerAngles;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Projectile(Clone)" && player.Blocking())
        {
            Destroy(other.gameObject);
            print("Wo?");
        }
    }
}