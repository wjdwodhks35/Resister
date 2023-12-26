using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        this.gameObject.transform.position = new Vector3(player.position.x, 90, player.position.z);
    }
}
