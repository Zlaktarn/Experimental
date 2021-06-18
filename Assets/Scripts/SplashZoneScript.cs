using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashZoneScript : MonoBehaviour
{
    public float offsetFloat = 10;
    public float damage = 2;

    Transform zoneTransform;
    public bool deadly = false;

    public float power;
    public float radius;
    public float upwardMod;
    Vector3 explosionPos;

    public Transform exploPos;

    [HideInInspector]
    public bool knockBack;
    [HideInInspector]
    public bool sphereExplosion;
    [HideInInspector]
    public bool boxExplosion;
    [HideInInspector]
    public bool singleTargetExplosion;


    Explosion explosiveScript;

    void Start()
    {
        //zoneObject.transform.localPosition = new Vector3(0, 0.75f, offsetFloat);
        //zoneTransform = zoneObject.transform;
        //zonePos.z += offset;

        if (knockBack)
        {
            explosiveScript = GetComponent<Explosion>();
            explosiveScript.exploPos = exploPos;
        }
        else
            explosiveScript = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Damage()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 8)
        {
            KnightScript player = other.gameObject.GetComponent<KnightScript>();

            ExplosionStyle(other);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(explosionPos, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(radius, 1, radius));
    }

    void ExplosionStyle(Collider other)
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