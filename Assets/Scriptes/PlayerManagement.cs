using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour
{
    enum AttackState
    {
        doAttack,
        doNotAttack,
        Reload
    }
    AttackState attackState;

    public Text bulletCount;

    [Header("ÃÑ¾Ë°ú ÃÑ±¸ÀÇ À§Ä¡")]
    public GameObject bulletFactory;
    public Transform bulletHolePos;

    public List<GameObject> bulletObjectPool;
    [Header("ÀåÅº ¼ö")]
    [SerializeField]
    private int poolSize = 20;
    
    private float attackDelay = 0.1f;
    private float currentDelay;

    void Start()
    {
        bulletObjectPool = new List<GameObject>();
        for(int i = 0;i<poolSize;i++)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bulletObjectPool.Add(bullet);
            bullet.SetActive(false);
        }
    }
    void Update()
    {
        SetText();
        if (Input.GetMouseButton(0) && poolSize > 0)
        {
            attackState = AttackState.doAttack;
        }
        else if(bulletObjectPool.Count == 0||Input.GetKey(KeyCode.R))
        {
            attackState = AttackState.Reload;
        }
        else
        {
            attackState = AttackState.doNotAttack;
        }
        //if (Input.GetButton(KeyCode.R.ToString()) && attackState == AttackState.doNotAttack || currentBulletCount == 0)
        //{
        //    attackState = AttackState.Reload;
        //}
        switch (attackState)
        {
            case AttackState.doAttack:
                doAttack();
                break;
            case AttackState.doNotAttack:
                doNotAttack();
                break;
            case AttackState.Reload:
                Reload();
                break;
        }
    }
    private void SetText()
    {
        bulletCount.text = bulletObjectPool.Count.ToString() + "/" + poolSize.ToString();
    }
    private void doNotAttack()
    {
        if(Input.GetKey(KeyCode.R))
        {
            attackState = AttackState.Reload;
        }
    }
    private void doAttack()
    {
        currentDelay += Time.deltaTime;
        if (currentDelay > attackDelay&&bulletObjectPool.Count>0)
        {
            currentDelay = 0;
            GameObject bullet = bulletObjectPool[0];
            bullet.SetActive(true);
            bulletObjectPool.Remove(bullet);
            bullet.transform.position = bulletHolePos.transform.position;
            bullet.transform.rotation = bulletHolePos.transform.rotation;
        }
    }
    private void Reload()
    {
        print("ÀçÀåÀü");
        for (int i = 0; i < poolSize; i++)
        {
            if (bulletObjectPool.Count >= poolSize)
                break;
            GameObject bullet = Instantiate(bulletFactory);
            bulletObjectPool.Add(bullet);
            bullet.SetActive(false);
        }
    }
}
