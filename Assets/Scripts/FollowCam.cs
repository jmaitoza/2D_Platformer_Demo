using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target; // the player
    public float smoothTime = 0.2f;
    
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        // preserve the camera's z position while changing x and y to match the player's
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        
        // smooth transition between camera's current position and the player's position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime); 
    }
}
