using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashZoneInitiator : MonoBehaviour
{
    public float damage;
    public GameObject deadlyZone;
    SplashZoneScript splashZoneScript;
    public float delay;

    void Start()
    {
        deadlyZone.SetActive(false);
        splashZoneScript = deadlyZone.GetComponent<SplashZoneScript>();
        splashZoneScript.damage = damage;
        StartCoroutine(ActivateDmgZone(delay));
    }

    // Update is called once per frame
    void Update()
    {
        Invoke(nameof(DestroyObject), delay+0.2f);
    }

    void ActivateDeadlyZone()
    {

    }

    IEnumerator ActivateDmgZone(float delay)
    {
        yield return new WaitForSeconds(delay);
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
        deadlyZone.SetActive(true);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
