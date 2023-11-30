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

    EnemyState m_State;              // enemy�� ����

    Transform player;                // �÷��̾� ��ġ �������� ���ؼ� ���
    NavMeshAgent Robot;              // enemy.AI
    CharacterController cc;          // ĳ���� ��Ʈ�ѷ�(��� ���� ��;)
    Animator anim;                   // enemy ���� �ִϸ��̼�

    public float findDistance = 8f;  // enemy�� ������ �� �ִ� ����
    public float attackDistace = 2f; // enemy ���� ��ġ
    public float moveSpeed = 5f;     // enemy �̵� �ӵ�
    public int attackPower = 3;      // enemy ���ݷ�
    public int hp = 15;              // enemy ü��

    int maxHP;                       // enemy �ִ� ü��
    float currectTime = 0;           // enemy�� �����ϱ� ���� ī��Ʈ �ٿ�
    float attackDelay = 2f;          // enemy ���� ������

    void Start()
    {
        Robot = GetComponent<NavMeshAgent>();       // AI�� ����ϱ� ���� ������Ʈ ������
        cc = GetComponent<CharacterController>();   // charactercontroller�� ����ϱ� ���� ������Ʈ ������
        anim = GetComponent<Animator>();            // enemy�� �ִϸ��̼��� ����ϱ� ���� ������Ʈ ������
        maxHP = hp; // ü��
        m_State = EnemyState.Idle;  // enemy �⺻ ����
        player = GameObject.Find("Player").transform;   // ���� ������Ʈ �߿��� Player�� ã�� �� Player�� ��ġ �� ������
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
        if(Vector3.Distance(transform.position, player.position)<findDistance)  // enemy�� player�� �Ÿ��� findDistance���� ���� �� �۵�
        {
            m_State = EnemyState.Move;  // enemy ���� ����
            print("���� ��ȯ: Idle -> Move");
            anim.SetTrigger("IdleToMove");  // �ִϸ��̼�
        }
    }
    void Move()
    {
        if(Vector3.Distance(transform.position,player.position)<attackDistace)  // �÷��̾ ���ݹ��� �ȿ� ������ ��
        {
            m_State = EnemyState.Attack;     // enemy ���� ����
            print("���� ��ȯ: Move -> Attack");
            currectTime = attackDelay;       // ���� �غ� �Ϸ� ����
            anim.SetTrigger("MoveToAttackDelay");   // �ִϸ��̼�
        }
        else
        {
            Robot.isStopped = false;
            Robot.stoppingDistance = attackDistace; // ���� �Ÿ�
            Robot.destination = player.position;    // enemy�� �������� �÷��̾��� ������
        }
    }
    void Attack()
    {
        if(Vector3.Distance(transform.position, player.position)<attackDistace)
        {
            currectTime += Time.deltaTime;
            if(currectTime > attackDelay)
            {
                print("����");
                currectTime = 0;    // ���� ������ ī��Ʈ �ʱ�ȭ
                anim.SetTrigger("StartAttack"); // �ִϸ��̼�
            }
        }
        else
        {
            m_State = EnemyState.Move;  // enemy ���� ����
            print("���� ��ȯ: Attack -> Move");
            anim.SetTrigger("AttackToMove");    // �ִϸ��̼�
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());    // �ڷ�ƾ ����(�������� ����)
    }
    void Die()
    {
        StopAllCoroutines();    // ��� �ڷ�ƾ ����
        StartCoroutine(DieProcess());   // �ڷ�ƾ ����(����)
    }
    public void HitEnemy(int hitPower)  // �ٸ� ��ũ��Ʈ���� ȣ��
    {
        hp -= hitPower;

        Robot.isStopped = true; // ����
        Robot.ResetPath();      // ������ ����(���� ���� ����)

        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return; // �׾��ų� �̹� �°� ������ ��ȯ
        }
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;   // enemy ���� ����
            print("���� ��ȯ: Any State -> Damaged");
            anim.SetTrigger("Damaged"); // �ִϸ��̼�
            Damaged();  // �Լ� ȣ��
        }
        else
        {
            m_State = EnemyState.Die;   // enemy ���� ����
            print("Any Stae -> Die");
            anim.SetTrigger("Die"); // �ִϸ��̼�
            Die();  // �Լ� ȣ��
        }
    }
    public void AttackAction()
    {
        // �÷��̾� ������Ʈ�� �����ϱ� ����
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);    // 1�� ��ٸ���

        m_State = EnemyState.Move;  // enemy ���� ����
        print("���� ��ȯ: Damaged -> Move");
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);    // ������Ʈ �ı�
    }
}
