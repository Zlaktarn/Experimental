using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonsterAI
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            ActivateRagdoll(true);

        Behaviour();
    }
}
