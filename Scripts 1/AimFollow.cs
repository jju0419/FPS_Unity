using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFollow : MonoBehaviour
{
    public Camera cm;
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = cm.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
