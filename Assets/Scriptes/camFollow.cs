using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public Transform Target;
    public float Camera_h;
    void Update()
    {
        gameObject.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y + Camera_h, Target.transform.position.z);
    }
}
