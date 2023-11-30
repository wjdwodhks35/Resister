using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_FSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    EnemyState m_State;              // enemy의 상태

    Transform player;                // 플레이어 위치 가져오기 위해서 사용
    NavMeshAgent Robot;              // enemy.AI
    CharacterController cc;          // 캐릭터 컨트롤러(사실 나도 모름;)
    Animator anim;                   // enemy 동작 애니메이션

    public float findDistance = 8f;  // enemy가 감지할 수 있는 범위
    public float attackDistace = 2f; // enemy 공격 리치
    public float moveSpeed = 5f;     // enemy 이동 속도
    public int attackPower = 3;      // enemy 공격력
    public int hp = 15;              // enemy 체력

    int maxHP;                       // enemy 최대 체력
    float currectTime = 0;           // enemy가 공격하기 위한 카운트 다운
    float attackDelay = 2f;          // enemy 공격 딜레이

    void Start()
    {
        Robot = GetComponent<NavMeshAgent>();       // AI를 사용하기 위해 컴포넌트 가져옴
        cc = GetComponent<CharacterController>();   // charactercontroller를 사용하기 위해 컴포넌트 가져옴
        anim = GetComponent<Animator>();            // enemy의 애니메이션을 사용하기 위해 컴포넌트 가져옴
        maxHP = hp; // 체력
        m_State = EnemyState.Idle;  // enemy 기본 상태
        player = GameObject.Find("Player").transform;   // 게임 오브젝트 중에서 Player를 찾고 그 Player의 위치 값 가져옴
    }

    void Update()
    {
        switch(m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                break;

        }
    }
    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position)<findDistance)  // enemy와 player의 거리가 findDistance보다 작을 때 작동
        {
            m_State = EnemyState.Move;  // enemy 상태 갱신
            print("상태 전환: Idle -> Move");
            anim.SetTrigger("IdleToMove");  // 애니메이션
        }
    }
    void Move()
    {
        if(Vector3.Distance(transform.position,player.position)<attackDistace)  // 플레이어가 공격범위 안에 들어왔을 때
        {
            m_State = EnemyState.Attack;     // enemy 상태 갱신
            print("상태 전환: Move -> Attack");
            currectTime = attackDelay;       // 공격 준비 완료 상태
            anim.SetTrigger("MoveToAttackDelay");   // 애니메이션
        }
        else
        {
            Robot.isStopped = false;
            Robot.stoppingDistance = attackDistace; // 정지 거리
            Robot.destination = player.position;    // enemy의 목적지를 플레이어의 포지션
        }
    }
    void Attack()
    {
        if(Vector3.Distance(transform.position, player.position)<attackDistace)
        {
            currectTime += Time.deltaTime;
            if(currectTime > attackDelay)
            {
                print("공격");
                currectTime = 0;    // 공격 딜레이 카운트 초기화
                anim.SetTrigger("StartAttack"); // 애니메이션
            }
        }
        else
        {
            m_State = EnemyState.Move;  // enemy 상태 갱신
            print("상태 전환: Attack -> Move");
            anim.SetTrigger("AttackToMove");    // 애니메이션
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());    // 코루틴 시작(데미지를 입음)
    }
    void Die()
    {
        StopAllCoroutines();    // 모든 코루틴 종료
        StartCoroutine(DieProcess());   // 코루틴 시작(죽음)
    }
    public void HitEnemy(int hitPower)  // 다른 스크립트에서 호출
    {
        hp -= hitPower;

        Robot.isStopped = true; // 경직
        Robot.ResetPath();      // 목적지 갱신(지울 수도 있음)

        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return; // 죽었거나 이미 맞고 있으면 반환
        }
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;   // enemy 상태 갱신
            print("상태 전환: Any State -> Damaged");
            anim.SetTrigger("Damaged"); // 애니메이션
            Damaged();  // 함수 호출
        }
        else
        {
            m_State = EnemyState.Die;   // enemy 상태 갱신
            print("Any Stae -> Die");
            anim.SetTrigger("Die"); // 애니메이션
            Die();  // 함수 호출
        }
    }
    public void AttackAction()
    {
        // 플레이어 컴포넌트에 공격하기 전달
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);    // 1초 기다리기

        m_State = EnemyState.Move;  // enemy 상태 갱신
        print("상태 전환: Damaged -> Move");
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);    // 오브젝트 파괴
    }
}
