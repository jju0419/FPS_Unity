using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f; //이동 속도 변수
    CharacterController cc; // 캐릭터컨트롤러 변수
    float gravity = -20f; //중력 변수
    float yVelocity = 0f; //수직 속력 변수
    public float jumpPower = 10f;
    public bool isJumping = false; //점프 상태변수
    public int hp = 100; //플레이어의 현재 체력
    public int maxHp = 100;//플레이어의 최대 체력
    public Slider hpSlider; // hp 슬라이더 변수

    public GameObject hitEffect; //좀비에게 맞을 때 hit 효과 오브젝트
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        //Player의 자식 오브젝트에 있는 애니메이터 컴포넌트 할당
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //게임 상태가 게임 중 상태아니라면 Update()를 종료시켜 사용자 입력을 못받게함
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        if (GameManager.gm.gState != GameManager.GameState.Shoping)
        {
            //사용자 입력을 받는다.
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            //이동 방향 설정
            Vector3 dir = new Vector3(h, 0, v);
            dir = dir.normalized;
            //파라미터 이름, 파라미터에 적용할 값
            anim.SetFloat("MoveMotion", dir.magnitude);

            //메인 카메라를 기준으로 방향을 변환한다. 
            dir = Camera.main.transform.TransformDirection(dir);

            //이동 속도에 맞춰 이동한다
            //  transform.position += dir * moveSpeed * Time.deltaTime;
            //만일 바닥에 닿아있다면..
            if (cc.collisionFlags == CollisionFlags.Below)
            {
                if (isJumping) //만일 점핑 중이라면
                {
                    isJumping = false; //점프 전 상태로 초기화
                }
                yVelocity = 0;
            }

            //만일 키보드 SpaceBar를 눌렀고, 점프하지 않은 상태라면
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                yVelocity = jumpPower; //수직 속도에 점프력을 적용
                isJumping = true;
            }

            yVelocity += gravity * Time.deltaTime; // 캐릭터 수직속도에 중력 값을 적용
                                                   //    Debug.Log(yVelocity.ToString());
            dir.y = yVelocity; // 중력값을 적용한 수직속도 할당
                               //이동 속도에 맞춰 이동
            cc.Move(dir * moveSpeed * Time.deltaTime);

            hpSlider.value = (float)hp / (float)maxHp;
        }

        
    }
    //플레이어의 피격 함수
    public void DamageAction(int damage)
    {
        hp -= damage;
        if(hp > 0) //플레이어 체력이 0보다 크다면
        {
            StartCoroutine(PlayHitEffect()); //HitEffect 코루틴 함수 실행
        }
    }

    IEnumerator PlayHitEffect() {
        hitEffect.SetActive(true); //피격 이펙트를 활성화 한 후 
        yield return new WaitForSeconds(0.3f); // 0.3초간 대기,
        hitEffect.SetActive(false); //피격 이펙트를 비활성화
    }
}
