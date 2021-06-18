using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform playerTarget;
    [SerializeField]
    Vector3 offset;

    void Start()
    {
        playerTarget = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = playerTarget.position + offset;
    }
}
