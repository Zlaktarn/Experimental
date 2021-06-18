using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    Vector3 explosionPos;
    [HideInInspector]
    public Transform exploPos;

    void Start()
    {
        if (exploPos == null)
            exploPos = this.transform;
    }

    public void AoeExplosionBox(float power, float radius, float upwardMod, float damage)
    {
        explosionPos = exploPos.position;
        Collider[] collidersBox = Physics.OverlapBox(explosionPos, new Vector3(radius, 1, radius));
        foreach (Collider hit in collidersBox)
        {
            if (hit.gameObject.tag == "Player")
            {
                MoveScript player = hit.gameObject.GetComponent<MoveScript>();
                player.hitpoint -= damage;
                player.flying = true;

            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, upwardMod, ForceMode.Impulse);
            }
        }
    }

    public void AoeExplosionSphere(float power, float radius, float upwardMod, float damage)
    {
        explosionPos = exploPos.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "Player")
            {
                MoveScript player = hit.gameObject.GetComponent<MoveScript>();
                player.flying = true;
                player.hitpoint -= damage;

            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, upwardMod, ForceMode.Impulse);
            }
        }
    }

    public void SingleTargetExplosion(Collider collider, float power, float radius, float upwardMod, float damage)
    {
        Vector3 explosionPos = transform.position;
        Rigidbody rbCol = collider.gameObject.GetComponent<Rigidbody>();
        MoveScript player = collider.gameObject.GetComponent<MoveScript>();
        player.hitpoint -= damage;
        rbCol.AddExplosionForce(power, explosionPos, radius, upwardMod, ForceMode.Impulse);
    }
}
