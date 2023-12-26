using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        target.forward = transform.forward;
    }
}
