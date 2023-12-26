//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraRot : MonoBehaviour
//{
//    //float RotSpeed = 2000f;

//    //void Update()
//    //{
//    //    float mx = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;
//    //    float my = -Input.GetAxis("Mouse Y") * RotSpeed * Time.deltaTime;
//    //    mx = Mathf.Clamp(mx, -90, 90);
//    //    my = Mathf.Clamp(my, -30, 30);
//    //    transform.eulerAngles = new Vector3(my, mx, 0);
//    //}
//    public float rotSpeed = 200f;

//    float mx = 0;
//    float my = 0;
//    void Update()
//    {
//        // if (Input.GetMouseButton(0))
//        {
//            float mouse_X = Input.GetAxis("Mouse X");
//            float mouse_Y = Input.GetAxis("Mouse Y");

//            mx += mouse_X * rotSpeed * Time.deltaTime;
//            my += mouse_Y * rotSpeed * Time.deltaTime;

//            my = Mathf.Clamp(my, -30f, 30f);

//            mx = Mathf.Clamp(mx, -90f, 90f);

//            // Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);

//            // transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

//            //Vector3 rot = transform.eulerAngles;
//            //rot.x = Mathf.Clamp(rot.x, -90f, 90f);
//            //transform.eulerAngles = rot;
//            Camera.main.transform.eulerAngles = new Vector3(-my, mx, 0);
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    public float rotSpeed = 200f;

    float mx = 0;
    float my = 0;
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -45f, 45f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
