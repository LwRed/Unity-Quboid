using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
     public Transform target;

     public Vector3 target_Offset;
     public float smoothTime = 0.3f;
     private Vector3 velocity = Vector3.zero;
 private void Start()
 {
     if(target)
     {
     target_Offset = transform.position - target.position;
     }
 }
 void Update()
 {
     if(target)
        {
           //transform.position = Vector3.Lerp(transform.position, target.position+target_Offset, 0.1f);
           transform.position = Vector3.SmoothDamp(transform.position, target.position+target_Offset, ref velocity, smoothTime);
       }
 }
}