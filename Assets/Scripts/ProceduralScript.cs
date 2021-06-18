using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProceduralScript : MonoBehaviour
{
    public GameObject ground;
    public GameObject block;

    public GameObject[] blocks;
    public float minHeight;
    public float minMoisture;
    public float minHeat;

    int xSize;
    int zSize;
    int k = 0;
    Vector3[] blocked;

        void Start()
    {
        xSize = (int)ground.transform.localScale.x;
        zSize = (int)ground.transform.localScale.z;

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                Vector3 blockPos = new Vector3(i - xSize / 2 + block.transform.localScale.x / 2, 1, j - zSize / 2 + block.transform.localScale.z / 2);
                
                if (i == 0 || i == xSize-1 || j == 0 || j == zSize-1)
                {
                    Instantiate(block, blockPos, Quaternion.identity);
                    //blocked[k] = blockPos;
                    k++;
                }


                int random = Random.Range(0, 40);
                if(random == 0 /*|| blockPos != blocked[k]*/)
                {
                    Instantiate(block, blockPos, Quaternion.identity);
                    //blocked[k] = blockPos;
                    k++;
                }
            }
        }
    }
}
