using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphicScript : MonoBehaviour
{
    public float input;

    [HideInInspector]
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Input", input);
    }
}
