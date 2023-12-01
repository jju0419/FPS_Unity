using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerFire : MonoBehaviour
{

    public float retroActionForce = 0.3f;
    public Text wModeText; //���� ��� �ؽ�Ʈ
    public GameObject firePosition; // �߻� ��ġ 
    public GameObject bombFactory; // ��ô ���� ������Ʈ
    float throwPower = 40f;
    public int bombnum = 3;
    public GameObject bulletEffect; // �ǰ� ����Ʈ ������Ʈ
    ParticleSystem ps; // �ǰ� ����Ʈ ��ƼŬ �ý���
    int weaponPower = 25; 
    Animator anim;
    public Text BM;//��ź�� ����
    public GameObject[] eff_Flash; // �� �߻� ȿ�� ������Ʈ �迭
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
    WeaponMode wMode; //���� ��� ����
    bool ZoomMode = false; // ī�޶� Ȯ�� Ȯ�ο� ����
    // Start is called before the first frame update
    void Start()
    {

        UnityEngine.Cursor.lockState = CursorLockMode.Locked; //���콺 Ŀ�� ���
        //�ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal; // ���� �⺻ ���� ��� ���� ����
    }

    // Update is called once per frame
    void Update()
    {
        originPos = GameObject.Find("CamPosition").transform.position;
        BM.text = "BOMB " + bombnum.ToString();
        //���� ���°� ���� �� ���¾ƴ϶�� Update()�� ������� ����� �Է��� ���ް���
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        if (GameManager.gm.gState != GameManager.GameState.Shoping)
        {
            if (Input.GetMouseButtonDown(1)) //���콺 ������ ��ư�� �����ٸ�
            {
                
                switch (wMode)
                {
                    case WeaponMode.Normal:
                        if (bombnum > 0)
                        {
                            //����ź�� ������ �� ����ź ���� ��ġ�� firePosition���� �Ѵ�.
                            GameObject bomb = Instantiate(bombFactory);
                            bomb.transform.position = firePosition.transform.position;

                            //����ź ������Ʈ�� ������ٵ� ������ ����
                            Rigidbody rb = bomb.GetComponent<Rigidbody>();
                            //AddForce�� �̿��� ����ź �̵�
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
                        if (!ZoomMode) //�� ��尡 �ƴ϶�� ī�޶� Ȯ���ϰ� �ܸ���..
                        {
                            Camera.main.fieldOfView = 15f;
                            ZoomMode = true;
                        }
                        else // �� ����� ī�޶� Ȯ�븦 �������·�, �� ��� ����
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







                    // �ѱ� �ݵ� �ڷ�ƾ ����
                    //StopAllCoroutines();


                    timer += Time.deltaTime;

                    if (timer >= attackCoolTime)
                    {

                        StartCoroutine(Shake());
                        StartCoroutine(Shoot(0.3f));
                        StartCoroutine(ShootEffectOn(0.03f)); //�� ����Ʈ �ǽ�
                        timer = 0;

                    }
                    
                    

                    //========================================================================================================================================================================================
                    /* IEnumerator RetroActionCoroutine()
                     {
                         Vector3 recoilBack = new Vector3(retroActionForce, originPos.y, originPos.z);     // ������ �� ���� ���� �ִ� �ݵ�

                         gameObject.transform.localPosition = originPos;

                         // �ݵ� ����
                         while (gameObject.transform.localPosition.x <= retroActionForce - 0.02f)
                         {
                             gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, recoilBack, 0.4f);
                             yield return null;
                         }

                         // ����ġ
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
                        //Random.insideUnitSphere : �������� 1�� ���� �������� �����ϰ� �ϳ��� ������
                    }
                    //========================================================================================================================================================================================
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {//���콺 ���� ��ư(�Ѿ�)
                    if (anim.GetFloat("MoveMotion") == 0)
                    {
                        anim.SetTrigger("Attack");
                    }
                    //���̸� ������ �� �߻� ��ġ�� ������ ����
                    Ray ray = new Ray(Camera.main.transform.position,
                        Camera.main.transform.forward);
                    //���̰� �ε��� ����� ������ ����ü�� ����
                    RaycastHit hitInfo = new RaycastHit();




                    //���̸� �߻��� �� �ε��� ��ü�� �ִٸ�...
                    if (Physics.Raycast(ray, out hitInfo))
                    {

                        //���̰� �ε��� ������Ʈ�� Enemy���..
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
                            //�ǰ� ����Ʈ�� forward ������ �ε��� ��ü��
                            //���� ���Ϳ� ��ġ ��Ų��.
                            bulletEffect.transform.forward = hitInfo.normal;
                            ps.Play();
                        }
                    }

                    StartCoroutine(ShootEffectOn(0.05f)); //�� ����Ʈ �ǽ�
                }
            }
            
            //����Ű 1�� ������ ��ָ��, ī�޶� ���� ���·�..
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
    IEnumerator ShootEffectOn(float duration) //�ѱ� ����Ʈ �ڷ�ƾ �Լ�
    {
        int num = Random.Range(0, eff_Flash.Length); // �����ϰ� ���ڸ� �̴´�.
        eff_Flash[num].SetActive(true); //�ѱ�ȿ�� �� �ϳ��� Ȱ��ȭ
        yield return new WaitForSeconds(duration);
        eff_Flash[num].SetActive(false); //��Ȱ��ȭ
    }

    IEnumerator Shoot(float duration) //�ѱ� ����Ʈ �ڷ�ƾ �Լ�
    {
        if (anim.GetFloat("MoveMotion") == 0)
        {
            anim.SetTrigger("Attack");
        }
        
        //���̸� ������ �� �߻� ��ġ�� ������ ����
        Ray ray = new Ray(Camera.main.transform.position,
            Camera.main.transform.forward);
        //���̰� �ε��� ����� ������ ����ü�� ����
        RaycastHit hitInfo = new RaycastHit();
        //���̸� �߻��� �� �ε��� ��ü�� �ִٸ�...
        if (Physics.Raycast(ray, out hitInfo))
        {

            //���̰� �ε��� ������Ʈ�� Enemy���..
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
                //�ǰ� ����Ʈ�� forward ������ �ε��� ��ü��
                //���� ���Ϳ� ��ġ ��Ų��.
                bulletEffect.transform.forward = hitInfo.normal;
                ps.Play();
            }
        }
        yield return new WaitForSeconds(duration);


    }
}
