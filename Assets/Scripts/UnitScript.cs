using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public Camera cam;
    Transform objectHit;
    Vector3 pos;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, 2 * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            pos = MousePos();
            print(MousePos());
        }
    }

    Vector3 MousePos()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            objectHit = hit.transform;
            print(objectHit);
        }

        return objectHit.position;
    }
}
