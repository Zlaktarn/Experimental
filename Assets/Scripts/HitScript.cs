using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    public int hitPoints = 1;
    MeshRenderer mesh;
    BoxCollider hitbox;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        hitbox = GetComponent<BoxCollider>();

    }

    void Update()
    {
        if (hitPoints <= 0)
            DestroyLimb();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Projectile(Clone)")
        {
            --hitPoints;
            Destroy(other.gameObject);
        }
    }

    public void DestroyLimb()
    {
        mesh.enabled = false;
        hitbox.enabled = false;
    }
}
