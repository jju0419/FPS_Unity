using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform target;
   
    // Update is called once per frame
    void Update()
    {
        //�ڱ��ڽ�(���ʹ��� ü�¹� ĵ����)�� ������ ī�޶��� ����� ��ġ��Ų��. 
        transform.forward = target.forward;
        
    }
}
