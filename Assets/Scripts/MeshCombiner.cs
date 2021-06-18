using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            CombineMeshes();
    }

    private void Start()
    {
        CombineMeshes();
    }

    public void CombineMeshes()
    {
        Quaternion oldRot = transform.localRotation;
        Vector3 oldPos = transform.localPosition;

        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            if (filters[i].transform == transform)
                continue;

            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = filters[i].sharedMesh;
            combiners[i].transform = filters[i].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes(combiners);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;
        transform.localRotation = oldRot;
        transform.localPosition = oldPos;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

    }
}
