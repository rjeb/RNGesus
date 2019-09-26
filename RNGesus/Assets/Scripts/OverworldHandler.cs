using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldHandler : MonoBehaviour
{
    public GameObject mapNode;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(mapNode, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
