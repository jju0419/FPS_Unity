using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState //좀비 상태 열거형 변수
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    EnemyState m_State;
    public float findDistance = 8f; //플레이어 발견 범위 
    public float attackDistance = 2f; //공격 가능 범위
    public float moveSpeed = 5f; // 이동 속도
    CharacterController cc; //캐릭터 컨트롤러 컴포넌트
    Transform player; // 플레이어 트랜스폼
    float currentTime = 0; //누적 시간
    float attackDelay = 1f; // 공격 딜레이 시간
    public int attackPower = 15; //좀비의 공격력
    public static EnemyFSM eFSM;

    Vector3 originPos; //좀비의 초기 위치 저장
    Quaternion originRot; //좀비의 초기 회전값 저장
    public float moveDistance = 20f; //좀비가 이동가능한 범위
    public int hp = 100; // 좀비의 현재 체력
    public int maxHp = 100; // 좀비의 최대 체력
    public Slider hpSlider;

    Animator anim; // 애니메이터 컴포넌트

    NavMeshAgent smith; // 내비게이션 에이전트 변수




    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle; //좀비의 초기상태는 대기상태
        //플레이어의 트랜스폼 컴포넌트 받아오기
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        originPos = transform.position; //좀비 초기 위치 저장
        originRot = transform.rotation; //좀비 초기 회전값 저장
        //자식 오브젝트로부터 애니메이터 컴포넌트 받아오기
        anim = transform.GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //현재 상태를 실시간으로 체크해 정해진 기능을 수행
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
        //플레이어와의 거리가 8미터 이내라면 무브 상태로 전환
        if(Vector3.Distance(transform.position, player.position)< findDistance)
        {
            m_State = EnemyState.Move;
           // print("상태전환: Idle->Move");
            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //초기위치에서 이동가능범위를 넘어간다면..
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
           //print("상태전환: Move->Return");
        }
        //플레이어와의 거리가 공격범위 밖이라면 플레이어를 향해 이동
        else if (Vector3.Distance(transform.position, player.position) >
            attackDistance)
        {
            /*  Vector3 dir = (player.position - transform.position).normalized; //이동 방향 설정
              cc.Move(dir * moveSpeed * Time.deltaTime); //이동
              transform.forward = dir; // 플레이어를 향해 방향 전환  */ //내비게이션 추가로 인한 삭제
           
            smith.isStopped = true; // 내비 매쉬 에이전트의 이동을 멈춤
            smith.ResetPath(); // 경로 초기화

            //내비게이션의 목적지를 최소 거리로 설정(공격 가능 범위 까지만 쫒아옴)
            smith.stoppingDistance = attackDistance;
            // 내비게이션 목적지를 플레이어 위치로 설정
            smith.destination = player.position;
        }
        else // 2미터 이내라면 공격상태로 전환
        {
            m_State = EnemyState.Attack;
           //print("상태 전환: Move -> Attack");
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
    }
    void Attack()
    {
        //플레이어가 공격범위 이내에 있다면 공격
        if (Vector3.Distance(transform.position, player.position)
            < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //플레이어의 PlayerMove.cs안에 있는 함수 호출
          //      player.GetComponent<PlayerMove>().
            //        DamageAction(attackPower);
               //print("공격");
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else //공격범위 밖이라면 재추격 실시
        {
            m_State = EnemyState.Move;
            //print("상태 전환: Attack->Move");
            currentTime = 0;
            anim.SetTrigger("AttackToMove"); //무브 애니메이션 실행
        }
    }
    public void AttackAction()
    {
        //플레이어의 PlayerMove.cs안에 있는 함수 호출
             player.GetComponent<PlayerMove>().
            DamageAction(attackPower);
    }
    void Return()
    {
        //초기위치에서 거리가 0.1이상이라면 초기위치로 이동
        if (Vector3.Distance(transform.position, originPos) >  0.1f)
        {
            /* Vector3 dir = (originPos - transform.position).
                 normalized;
             cc.Move(dir * moveSpeed * Time.deltaTime);
             transform.forward = dir; //초기 위치 방향으로 좀비 전환 */ //내비게이션 추가로 인한 삭제2

            smith.destination = originPos;
            smith.stoppingDistance = 0; // 목적지와의 거리가 0일때 까지

        }
        else // 그렇지 않다면 초기위치로 지정, 현재상태 대기로 전환
        {
            smith.isStopped= true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            m_State = EnemyState.Idle;
            hp = maxHp;// 체력을 다시 꽉 채움
            //print("상태전환: Return->Idle");
            anim.SetTrigger("MoveToIdle"); //대기 애니메이션 실행
        }
    }
    public void HitEnemy(int hitPower) //좀비가 맞았을 때 호출되는 함수
    {
        //피격상태이거나, 사망 상태, 복귀상태라면 아무런 처리를 하지않고 함수를 종료
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die ||
            m_State== EnemyState.Return) { return; }

        hp -= hitPower;

        smith.isStopped= true;
        smith.ResetPath();


        if (hp > 0) // 총에 맞았을 때 좀비의 체력이 0보다 크다면
        {
            m_State = EnemyState.Damaged;
            //print("상태 전환: Any State -> Damaged");
            anim.SetTrigger("Damaged"); //Damaged 애니메이션 실행
            Damaged();
        }
        else //좀비의 체력이 0보다 작다면..
        {
            m_State = EnemyState.Die;
            //print("상태 전환: Any State -> Die");
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
        //피격 애니메이션 재생 시간만큼 대기
        yield return new WaitForSeconds(1f);
        m_State = EnemyState.Move;
        //print("상태 전환: Damaged -> Move");
    }

    void Die() //죽었을 때
    {
        StopAllCoroutines(); //기존에 실행되고 있던 코루틴함수들 모두 종료
        StartCoroutine(DieProcess()); // DieProcess 코루틴 함수 실행
        GameManager.gm.money += 50; 
    }
    IEnumerator DieProcess()
    {
        //오브젝트는 SetActive()로 활성/비활성, 컴포넌트는 enabled로 활성/비활성
        cc.enabled = false; //좀비의 캐릭터컨트롤러를 비활성화
        yield return new WaitForSeconds(2f); //2초 대기
        //print("소멸");
        Destroy(gameObject); //자기 자신(좀비)을 파괴
    }
}
