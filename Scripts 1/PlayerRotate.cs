using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 200f;
    float mx = 0; //ȸ���� ���� (Y�ุ ȸ��)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���°� ���� �� ���¾ƴ϶�� Update()�� ������� ����� �Է��� ���ް���
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        if (GameManager.gm.gState != GameManager.GameState.Shoping)
        {
            float mouse_X = Input.GetAxis("Mouse X");
            //ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������ ��Ų��.
            mx += mouse_X * rotSpeed * Time.deltaTime;
            //ȸ�� �������� Y�� ȸ���� �Ѵ�.
            transform.eulerAngles = new Vector3(0, mx, 0);
        }
    }
}
