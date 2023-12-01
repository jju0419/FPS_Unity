using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f; //�̵� �ӵ� ����
    CharacterController cc; // ĳ������Ʈ�ѷ� ����
    float gravity = -20f; //�߷� ����
    float yVelocity = 0f; //���� �ӷ� ����
    public float jumpPower = 10f;
    public bool isJumping = false; //���� ���º���
    public int hp = 100; //�÷��̾��� ���� ü��
    public int maxHp = 100;//�÷��̾��� �ִ� ü��
    public Slider hpSlider; // hp �����̴� ����

    public GameObject hitEffect; //���񿡰� ���� �� hit ȿ�� ������Ʈ
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();
        //Player�� �ڽ� ������Ʈ�� �ִ� �ִϸ����� ������Ʈ �Ҵ�
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���°� ���� �� ���¾ƴ϶�� Update()�� ������� ����� �Է��� ���ް���
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        if (GameManager.gm.gState != GameManager.GameState.Shoping)
        {
            //����� �Է��� �޴´�.
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            //�̵� ���� ����
            Vector3 dir = new Vector3(h, 0, v);
            dir = dir.normalized;
            //�Ķ���� �̸�, �Ķ���Ϳ� ������ ��
            anim.SetFloat("MoveMotion", dir.magnitude);

            //���� ī�޶� �������� ������ ��ȯ�Ѵ�. 
            dir = Camera.main.transform.TransformDirection(dir);

            //�̵� �ӵ��� ���� �̵��Ѵ�
            //  transform.position += dir * moveSpeed * Time.deltaTime;
            //���� �ٴڿ� ����ִٸ�..
            if (cc.collisionFlags == CollisionFlags.Below)
            {
                if (isJumping) //���� ���� ���̶��
                {
                    isJumping = false; //���� �� ���·� �ʱ�ȭ
                }
                yVelocity = 0;
            }

            //���� Ű���� SpaceBar�� ������, �������� ���� ���¶��
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                yVelocity = jumpPower; //���� �ӵ��� �������� ����
                isJumping = true;
            }

            yVelocity += gravity * Time.deltaTime; // ĳ���� �����ӵ��� �߷� ���� ����
                                                   //    Debug.Log(yVelocity.ToString());
            dir.y = yVelocity; // �߷°��� ������ �����ӵ� �Ҵ�
                               //�̵� �ӵ��� ���� �̵�
            cc.Move(dir * moveSpeed * Time.deltaTime);

            hpSlider.value = (float)hp / (float)maxHp;
        }

        
    }
    //�÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        hp -= damage;
        if(hp > 0) //�÷��̾� ü���� 0���� ũ�ٸ�
        {
            StartCoroutine(PlayHitEffect()); //HitEffect �ڷ�ƾ �Լ� ����
        }
    }

    IEnumerator PlayHitEffect() {
        hitEffect.SetActive(true); //�ǰ� ����Ʈ�� Ȱ��ȭ �� �� 
        yield return new WaitForSeconds(0.3f); // 0.3�ʰ� ���,
        hitEffect.SetActive(false); //�ǰ� ����Ʈ�� ��Ȱ��ȭ
    }
}
