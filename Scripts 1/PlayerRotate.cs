using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 200f;
    float mx = 0; //회전값 변수 (Y축만 회전)
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
        if (GameManager.gm.gState != GameManager.GameState.Shoping)
        {
            float mouse_X = Input.GetAxis("Mouse X");
            //회전 값 변수에 마우스 입력 값만큼 미리 누적을 시킨다.
            mx += mouse_X * rotSpeed * Time.deltaTime;
            //회전 방향으로 Y축 회전을 한다.
            transform.eulerAngles = new Vector3(0, mx, 0);
        }
    }
}
