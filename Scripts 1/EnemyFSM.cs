using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState //���� ���� ������ ����
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    EnemyState m_State;
    public float findDistance = 8f; //�÷��̾� �߰� ���� 
    public float attackDistance = 2f; //���� ���� ����
    public float moveSpeed = 5f; // �̵� �ӵ�
    CharacterController cc; //ĳ���� ��Ʈ�ѷ� ������Ʈ
    Transform player; // �÷��̾� Ʈ������
    float currentTime = 0; //���� �ð�
    float attackDelay = 1f; // ���� ������ �ð�
    public int attackPower = 15; //������ ���ݷ�
    public static EnemyFSM eFSM;

    Vector3 originPos; //������ �ʱ� ��ġ ����
    Quaternion originRot; //������ �ʱ� ȸ���� ����
    public float moveDistance = 20f; //���� �̵������� ����
    public int hp = 100; // ������ ���� ü��
    public int maxHp = 100; // ������ �ִ� ü��
    public Slider hpSlider;

    Animator anim; // �ִϸ����� ������Ʈ

    NavMeshAgent smith; // ������̼� ������Ʈ ����




    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle; //������ �ʱ���´� ������
        //�÷��̾��� Ʈ������ ������Ʈ �޾ƿ���
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        originPos = transform.position; //���� �ʱ� ��ġ ����
        originRot = transform.rotation; //���� �ʱ� ȸ���� ����
        //�ڽ� ������Ʈ�κ��� �ִϸ����� ������Ʈ �޾ƿ���
        anim = transform.GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���¸� �ǽð����� üũ�� ������ ����� ����
        switch(m_State)
        {
            case EnemyState.Idle: Idle();  break;
            case EnemyState.Move: Move();  break;
            case EnemyState.Attack: Attack(); break;
            case EnemyState.Return: Return();  break;
        //    case EnemyState.Damaged: break;
        //    case EnemyState.Die: break;
        }
        hpSlider.value = (float)hp / (float)maxHp;

        if (GameManager.gm.sState == GameManager.GameWave.Wave2)
        {
            attackPower += 15;
            maxHp += 30;
            hp += 30;
            attackDelay = 1.2f;
        }

        else if (GameManager.gm.sState == GameManager.GameWave.Wave3)
        {
            attackPower += 15;
            maxHp += 5;
            hp -= 5;
            attackDelay = 0.8f;
        }

    }
    void Idle()
    {
        //�÷��̾���� �Ÿ��� 8���� �̳���� ���� ���·� ��ȯ
        if(Vector3.Distance(transform.position, player.position)< findDistance)
        {
            m_State = EnemyState.Move;
           // print("������ȯ: Idle->Move");
            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //�ʱ���ġ���� �̵����ɹ����� �Ѿ�ٸ�..
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
           //print("������ȯ: Move->Return");
        }
        //�÷��̾���� �Ÿ��� ���ݹ��� ���̶�� �÷��̾ ���� �̵�
        else if (Vector3.Distance(transform.position, player.position) >
            attackDistance)
        {
            /*  Vector3 dir = (player.position - transform.position).normalized; //�̵� ���� ����
              cc.Move(dir * moveSpeed * Time.deltaTime); //�̵�
              transform.forward = dir; // �÷��̾ ���� ���� ��ȯ  */ //������̼� �߰��� ���� ����
           
            smith.isStopped = true; // ���� �Ž� ������Ʈ�� �̵��� ����
            smith.ResetPath(); // ��� �ʱ�ȭ

            //������̼��� �������� �ּ� �Ÿ��� ����(���� ���� ���� ������ �i�ƿ�)
            smith.stoppingDistance = attackDistance;
            // ������̼� �������� �÷��̾� ��ġ�� ����
            smith.destination = player.position;
        }
        else // 2���� �̳���� ���ݻ��·� ��ȯ
        {
            m_State = EnemyState.Attack;
           //print("���� ��ȯ: Move -> Attack");
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
    }
    void Attack()
    {
        //�÷��̾ ���ݹ��� �̳��� �ִٸ� ����
        if (Vector3.Distance(transform.position, player.position)
            < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //�÷��̾��� PlayerMove.cs�ȿ� �ִ� �Լ� ȣ��
          //      player.GetComponent<PlayerMove>().
            //        DamageAction(attackPower);
               //print("����");
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else //���ݹ��� ���̶�� ���߰� �ǽ�
        {
            m_State = EnemyState.Move;
            //print("���� ��ȯ: Attack->Move");
            currentTime = 0;
            anim.SetTrigger("AttackToMove"); //���� �ִϸ��̼� ����
        }
    }
    public void AttackAction()
    {
        //�÷��̾��� PlayerMove.cs�ȿ� �ִ� �Լ� ȣ��
             player.GetComponent<PlayerMove>().
            DamageAction(attackPower);
    }
    void Return()
    {
        //�ʱ���ġ���� �Ÿ��� 0.1�̻��̶�� �ʱ���ġ�� �̵�
        if (Vector3.Distance(transform.position, originPos) >  0.1f)
        {
            /* Vector3 dir = (originPos - transform.position).
                 normalized;
             cc.Move(dir * moveSpeed * Time.deltaTime);
             transform.forward = dir; //�ʱ� ��ġ �������� ���� ��ȯ */ //������̼� �߰��� ���� ����2

            smith.destination = originPos;
            smith.stoppingDistance = 0; // ���������� �Ÿ��� 0�϶� ����

        }
        else // �׷��� �ʴٸ� �ʱ���ġ�� ����, ������� ���� ��ȯ
        {
            smith.isStopped= true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            m_State = EnemyState.Idle;
            hp = maxHp;// ü���� �ٽ� �� ä��
            //print("������ȯ: Return->Idle");
            anim.SetTrigger("MoveToIdle"); //��� �ִϸ��̼� ����
        }
    }
    public void HitEnemy(int hitPower) //���� �¾��� �� ȣ��Ǵ� �Լ�
    {
        //�ǰݻ����̰ų�, ��� ����, ���ͻ��¶�� �ƹ��� ó���� �����ʰ� �Լ��� ����
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die ||
            m_State== EnemyState.Return) { return; }

        hp -= hitPower;

        smith.isStopped= true;
        smith.ResetPath();


        if (hp > 0) // �ѿ� �¾��� �� ������ ü���� 0���� ũ�ٸ�
        {
            m_State = EnemyState.Damaged;
            //print("���� ��ȯ: Any State -> Damaged");
            anim.SetTrigger("Damaged"); //Damaged �ִϸ��̼� ����
            Damaged();
        }
        else //������ ü���� 0���� �۴ٸ�..
        {
            m_State = EnemyState.Die;
            //print("���� ��ȯ: Any State -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        //�ǰ� �ִϸ��̼� ��� �ð���ŭ ���
        yield return new WaitForSeconds(1f);
        m_State = EnemyState.Move;
        //print("���� ��ȯ: Damaged -> Move");
    }

    void Die() //�׾��� ��
    {
        StopAllCoroutines(); //������ ����ǰ� �ִ� �ڷ�ƾ�Լ��� ��� ����
        StartCoroutine(DieProcess()); // DieProcess �ڷ�ƾ �Լ� ����
        GameManager.gm.money += 50; 
    }
    IEnumerator DieProcess()
    {
        //������Ʈ�� SetActive()�� Ȱ��/��Ȱ��, ������Ʈ�� enabled�� Ȱ��/��Ȱ��
        cc.enabled = false; //������ ĳ������Ʈ�ѷ��� ��Ȱ��ȭ
        yield return new WaitForSeconds(2f); //2�� ���
        //print("�Ҹ�");
        Destroy(gameObject); //�ڱ� �ڽ�(����)�� �ı�
    }
}
