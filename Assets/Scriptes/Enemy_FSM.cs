using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy_FSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        //Return,
        Damaged,
        Die
    }

    EnemyState m_State;

    Transform player;
    NavMeshAgent smith;
    Vector3 moveDir;

    public Slider curHPSlider;

    public float findDistance = 8f;
    public float attackDistance = 2f;
    public float moveSpeed = 5f;
    public float moveDistance = 20f;
    public int attackPower = 3;
    public int maxHP = 15;
    public int curHP = 15;

    float currentTime = 0;
    float attackDelay = 2f;
    float gravity = 9.8f;

    CharacterController cc;
    Animator anim;

    void Start()
    {
        smith = GetComponent<NavMeshAgent>();

        curHP = maxHP;

        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        cc = GetComponent<CharacterController>();

        anim = transform.GetComponentInChildren<Animator>();
        curHPSlider.maxValue = maxHP;
    }
    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded)
        {
            switch (m_State)
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
                    //Damaged();
                    break;
                case EnemyState.Die:
                    //Die();
                    break;
            }
        }
        // 중력 적용
        moveDir.y -= gravity * Time.deltaTime;
        // 동작
        cc.Move(moveDir * Time.deltaTime);
    }
    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //if (Vector3.Distance(transform.position, originPos) > moveDistance)
        //{
        //    //Return();
        //    print("상태 전환: Move -> Return");
        //}
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //Vector3 dir = (player.position - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            smith.isStopped = true;
            smith.ResetPath();

            smith.stoppingDistance = attackDistance;

            smith.destination = player.position;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
    }
    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= attackDelay)
            {
                print("공격");
                currentTime = 0;
                player.GetComponent<PlayerController>().Damaged(attackPower);
                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;
            anim.SetTrigger("AttackToMove");
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }
    public void HitEnemy(int hitPower)
    {
        curHP -= hitPower;
        CheckcurHP();

        smith.isStopped = true;
        smith.ResetPath();

        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }
        if (curHP > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damged");
            //anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any State -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }
    public void CheckcurHP()
    {
        curHPSlider.value = curHP;
    }
    //public void AttackAction()
    //{
    //    player.GetComponent<CharController_Motor>().DamageAction(attackPower);
    //}
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);

        m_State = EnemyState.Move;
        print("상태 전환: Damage -> Move");
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }
}