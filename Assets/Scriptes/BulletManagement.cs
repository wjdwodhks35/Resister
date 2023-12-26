using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManagement : MonoBehaviour
{
    private float bulletSpeed = 100f;
    void Start()
    {
        Destroy(this.gameObject, 3f);
        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * bulletSpeed);
    }

    public int attackPower = 1;

    public float explosionRadus = 3f;
    private void OnCollisionEnter(Collision collision)
    {
        //Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadus, 1 << 10);

        //for (int i = 0; i < cols.Length; i++)
        //{
        //    cols[i].GetComponent<Enemy_FSM>().HitEnemy(attackPower);
        //}
        if (!collision.gameObject.CompareTag("Enemy"))
            return;
        collision.gameObject.GetComponent<Enemy_FSM>().HitEnemy(attackPower);
        Destroy(gameObject);
    }
}
