using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Marking : MonoBehaviour
{
    public Transform Object_Marking;
    void Update()
    {
        this.gameObject.transform.position = new Vector3(Object_Marking.transform.position.x,89, Object_Marking.transform.position.z);
    }
}
