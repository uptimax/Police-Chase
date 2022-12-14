using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movecam : MonoBehaviour
{
    public GameObject cam;
    public float nwPos;

    // Start is called before the first frame update
    void Start()
    {
        nwPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        nwPos = cam.transform.position.x;  
    }
}
