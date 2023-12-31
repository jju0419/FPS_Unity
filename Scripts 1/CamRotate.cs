using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    public float rotSpeed = 200f; //회전 속도 변수

    float mx = 0; //회전 값 변수
    float my = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //게임 상태가 게임 중 상태아니라면 Update()를 종료시켜 사용자 입력을 못받게함
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        //사용자 마우스 입력을 받음
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //회전값 변수에 마우스 입력 값 만큼 미리 누적을 시킨다.
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;
        //마우스 상하 이동 회전 변수(my)의 값을 -90~90도 사이로 제한한다. 
        my = Mathf.Clamp(my, -90f, 90f);
        transform.eulerAngles = new Vector3(-my, mx,0);

        //마우스 입력 값을 이용해 회전 방향을 결정
        /* Vector3 dir = new Vector3 (-mouse_Y, mouse_X, 0);

         //회전 방향으로 물체를 회전 시킨다.
         transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

         //X축 회전(상하 회전) 값을 -90도~90도 사이로 제한한다.
         Vector3 rot = transform.eulerAngles;
         rot.x = Mathf.Clamp(rot.x, -90f, 90f);
         transform.eulerAngles = rot;
        */
    }
}
