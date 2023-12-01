using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    //���ŵ� �ð� ����
    public float destroyTime = 1.5f;
    float currentTime = 0f; //��� �ð� ������ ����
   
    // Update is called once per frame
    void Update()
    {
        if(currentTime > destroyTime) //����ð��� ���ŵ� �ð����� ������
        {
            Destroy(gameObject); //�ڽ� �ڽ��� ����
        }
        currentTime += Time.deltaTime; //��� �ð��� ����
    }
}
