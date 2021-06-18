using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMobScript : MonsterAI
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            ActivateRagdoll(true);

        Behaviour();
    }
}