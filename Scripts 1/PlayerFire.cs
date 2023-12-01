using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerFire : MonoBehaviour
{

    public float retroActionForce = 0.3f;
    public Text wModeText; //무기 모드 텍스트
    public GameObject firePosition; // 발사 위치 
    public GameObject bombFactory; // 투척 무기 오브젝트
    float throwPower = 40f;
    public int bombnum = 3;
    public GameObject bulletEffect; // 피격 이펙트 오브젝트
    ParticleSystem ps; // 피격 이펙트 파티클 시스템
    int weaponPower = 25; 
    Animator anim;
    public Text BM;//폭탄의 개수
    public GameObject[] eff_Flash; // 총 발사 효과 오브젝트 배열
    public float magnitude;
    public Camera cm;
    private float attackCoolTime =0.05f ;
private float timer = 0;

     Vector3 originPos;

    enum WeaponMode
    {
        Normal,
        Sniper,
        Auto
    }
    WeaponMode wMode; //무기 모드 변수
    bool ZoomMode = false; // 카메라 확대 확인용 변수
    // Start is called before the first frame update
    void Start()
    {

        UnityEngine.Cursor.lockState = CursorLockMode.Locked; //마우스 커서 잠금
        //피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal; // 무기 기본 모드는 노멀 모드로 설정
    }

    // Update is called once per frame
    void Update()
    {
        originPos = GameObject.Find("CamPosition").transform.position;
        BM.text = "BOMB " + bombnum.ToString();
        //게임 상태가 게임 중 상태아니라면 Update()를 종료시켜 사용자 입력을 못받게함
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        if (GameManager.gm.gState != GameManager.GameState.Shoping)
        {
            if (Input.GetMouseButtonDown(1)) //마우스 오른쪽 버튼을 눌렀다면
            {
                
                switch (wMode)
                {
                    case WeaponMode.Normal:
                        if (bombnum > 0)
                        {
                            //수류탄을 생성한 후 수류탄 생성 위치를 firePosition으로 한다.
                            GameObject bomb = Instantiate(bombFactory);
                            bomb.transform.position = firePosition.transform.position;

                            //수류탄 오브젝트의 리지드바디 정보를 얻어옴
                            Rigidbody rb = bomb.GetComponent<Rigidbody>();
                            //AddForce를 이용해 수류탄 이동
                            rb.AddForce(Camera.main.transform.forward * throwPower,
                                ForceMode.Impulse);
                            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
                            bombnum -= 1; break;
                        }
                        else
                        {

                            break;
                        }
                        

                    case WeaponMode.Sniper:
                        if (!ZoomMode) //줌 모드가 아니라면 카메라를 확대하고 줌모드로..
                        {
                            Camera.main.fieldOfView = 15f;
                            ZoomMode = true;
                        }
                        else // 줌 모드라면 카메라 확대를 원래상태로, 줌 모드 해제
                        {
                            Camera.main.fieldOfView = 60f;
                            ZoomMode = false;
                        }
                        break;

                    
                        
                        
                }


            }
            if (wMode == WeaponMode.Auto)
            {
                if (Input.GetMouseButton(0))
                {

                    weaponPower = weaponPower / 3;







                    // 총기 반동 코루틴 실행
                    //StopAllCoroutines();


                    timer += Time.deltaTime;

                    if (timer >= attackCoolTime)
                    {

                        StartCoroutine(Shake());
                        StartCoroutine(Shoot(0.3f));
                        StartCoroutine(ShootEffectOn(0.03f)); //총 이펙트 실시
                        timer = 0;

                    }
                    
                    

                    //========================================================================================================================================================================================
                    /* IEnumerator RetroActionCoroutine()
                     {
                         Vector3 recoilBack = new Vector3(retroActionForce, originPos.y, originPos.z);     // 정조준 안 했을 때의 최대 반동

                         gameObject.transform.localPosition = originPos;

                         // 반동 시작
                         while (gameObject.transform.localPosition.x <= retroActionForce - 0.02f)
                         {
                             gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, recoilBack, 0.4f);
                             yield return null;
                         }

                         // 원위치
                         while (gameObject.transform.localPosition != originPos)
                         {
                             gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, originPos, 0.1f);
                             yield return null;
                         }
                     }*/
                    IEnumerator Shake()
                    {

                        cm.transform.localPosition = (Vector3)Random.insideUnitSphere * 0.1f + originPos;


                        yield return new WaitForSeconds(0.5f);

                        StopCoroutine(Shake());
                        cm.transform.localPosition = originPos;
                        //Random.insideUnitSphere : 반지름이 1인 구의 내부점을 랜덤하게 하나를 가져옴
                    }
                    //========================================================================================================================================================================================
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {//마우스 왼쪽 버튼(총알)
                    if (anim.GetFloat("MoveMotion") == 0)
                    {
                        anim.SetTrigger("Attack");
                    }
                    //레이를 생성한 후 발사 위치와 방향을 설정
                    Ray ray = new Ray(Camera.main.transform.position,
                        Camera.main.transform.forward);
                    //레이가 부딪힌 대상의 정보를 구조체에 저장
                    RaycastHit hitInfo = new RaycastHit();




                    //레이를 발사한 후 부딪힌 물체가 있다면...
                    if (Physics.Raycast(ray, out hitInfo))
                    {

                        //레이가 부딪힌 오브젝트가 Enemy라면..
                        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                        {
                            if (hitInfo.collider.name == "Head")
                            {
                                EnemyFSM eFSM = hitInfo.transform.parent.GetComponent<EnemyFSM>();
                                print("ss");
                                eFSM.HitEnemy(weaponPower * 2);
                            }
                            else
                            {
                                EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                                print("1");
                                eFSM.HitEnemy(weaponPower);
                            }

                        }

                        else
                        {
                            bulletEffect.transform.position = hitInfo.point;
                            //피격 이펙트의 forward 방향을 부딪힌 물체의
                            //법선 벡터와 일치 시킨다.
                            bulletEffect.transform.forward = hitInfo.normal;
                            ps.Play();
                        }
                    }

                    StartCoroutine(ShootEffectOn(0.05f)); //총 이펙트 실시
                }
            }
            
            //숫자키 1을 누르면 노멀모드, 카메라 원래 상태로..
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                wMode = WeaponMode.Normal;
                Camera.main.fieldOfView = 60f;
                wModeText.text = "Normal Mode";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                wMode = WeaponMode.Sniper;
                wModeText.text = "Sniper Mode";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                wMode = WeaponMode.Auto;
                wModeText.text = "Auto Mode";
            }
        }

    }
    IEnumerator ShootEffectOn(float duration) //총구 이펙트 코루틴 함수
    {
        int num = Random.Range(0, eff_Flash.Length); // 랜덤하게 숫자를 뽑는다.
        eff_Flash[num].SetActive(true); //총구효과 중 하나가 활성화
        yield return new WaitForSeconds(duration);
        eff_Flash[num].SetActive(false); //비활성화
    }

    IEnumerator Shoot(float duration) //총구 이펙트 코루틴 함수
    {
        if (anim.GetFloat("MoveMotion") == 0)
        {
            anim.SetTrigger("Attack");
        }
        
        //레이를 생성한 후 발사 위치와 방향을 설정
        Ray ray = new Ray(Camera.main.transform.position,
            Camera.main.transform.forward);
        //레이가 부딪힌 대상의 정보를 구조체에 저장
        RaycastHit hitInfo = new RaycastHit();
        //레이를 발사한 후 부딪힌 물체가 있다면...
        if (Physics.Raycast(ray, out hitInfo))
        {

            //레이가 부딪힌 오브젝트가 Enemy라면..
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hitInfo.collider.name == "Head")
                {
                    EnemyFSM eFSM = hitInfo.transform.parent.GetComponent<EnemyFSM>();
                    print("ss");
                    eFSM.HitEnemy(weaponPower * 2);
                }
                else
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    print("1");
                    eFSM.HitEnemy(weaponPower);
                }

            }

            else
            {
                bulletEffect.transform.position = hitInfo.point;
                //피격 이펙트의 forward 방향을 부딪힌 물체의
                //법선 벡터와 일치 시킨다.
                bulletEffect.transform.forward = hitInfo.normal;
                ps.Play();
            }
        }
        yield return new WaitForSeconds(duration);


    }
}
