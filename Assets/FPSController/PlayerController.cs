using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public Slider PlayerHP;

    public int curHp = 20;
    public float jumpPower = 10f;
    public float moveSpeed = 7f;
    public float gravity = 9.8f;

    private Vector3 MoveDir;

    int maxHp = 20;
    float rotSpeed = 200f;
    float mx = 0;

    bool isLive;

    void Start()
    {
        PlayerHP.maxValue = maxHp;
        curHp = maxHp;
        //hitEffect.SetActive(false);
        cc = GetComponent<CharacterController>();
        isLive = true;
    }

    CharacterController cc;
    void Update()
    {
        if (isLive)
        {
            // 현재 캐릭터가 땅에 있는가?
            if (cc.isGrounded)
            {
                // 위, 아래 움직임 셋팅. 
                MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
                MoveDir = transform.TransformDirection(MoveDir);

                // 스피드 증가.
                MoveDir *= moveSpeed;

                // 캐릭터 점프
                if (Input.GetButton("Jump"))
                    MoveDir.y = jumpPower;

            }

            // 캐릭터에 중력 적용.
            MoveDir.y -= gravity * Time.deltaTime;

            // 캐릭터 움직임.
            cc.Move(MoveDir * Time.deltaTime);

            // 카메라 움직임

            float mouse_X = Input.GetAxis("Mouse X");
            mx += mouse_X * rotSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, mx, 0);
        }
        //public void DamageAction(int damage)
        //{
        //    hp -= damage;
        //    if (hp > 0)
        //    {
        //        StartCoroutine(PlayHitEffect());
        //    }
        //}
        //IEnumerator PlayHitEffect()
        //{
        //    hitEffect.SetActive(true);
        //    yield return new WaitForSeconds(0.3f);
        //    hitEffect.SetActive(false);
        //}
    }
    private void CheckHP()
    {
        PlayerHP.value = curHp;
    }
    public void Damaged(int AttackPower)
    {
        curHp -= AttackPower;
        CheckHP();
    }
}
