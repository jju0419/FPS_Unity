using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform target;
   
    // Update is called once per frame
    void Update()
    {
        //자기자신(에너미의 체력바 캔버스)의 방향을 카메라의 방향과 일치시킨다. 
        transform.forward = target.forward;
        
    }
}
