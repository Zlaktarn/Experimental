using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrowScript : MonoBehaviour
{

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        ArrowPos();
    }

    public void ArrowPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        RaycastHit hit;

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point); // Look at the point
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)); // Clamp the x and z rotation

            //if (Physics.Raycast(ray, out hit))
            //{



            //    Transform  target = Quaternion.EulerRotation(hit.point, 0);
            //    target.y = 0;
            //    target.y = transform.localScale.y / 2f;

            //    transform.eulerAngles = new Vector3(target.x, target.rotation.eulerAngles.y, target.z);
            //    //transform.LookAt(target);
            //}
        }
    }
}
