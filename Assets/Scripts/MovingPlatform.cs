using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script takes two positions, 'start' and 'finish' and makes the platform bounce between them
public class MovingPlatform : MonoBehaviour
{
    public Vector3 finishPos = Vector3.zero; // position to move to
    public float speed = 0.5f;

    private Vector3 startingPos;
    private float trackPercent = 0; // how far along the "track" between start and finish
    private int direction = 1; // current movement direction

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position; // placement in the scene is the position to move from
    }

    // Update is called once per frame
    void Update()
    {
        trackPercent += direction * speed * Time.deltaTime;
        float x = (finishPos.x - startingPos.x) * trackPercent + startingPos.x;
        float y = (finishPos.y - startingPos.y) * trackPercent + startingPos.y;
        transform.position = new Vector3(x, y, startingPos.z);

        if ((direction == 1 && trackPercent > 0.9f) || (direction == -1 && trackPercent < 0.1f))
        {
            //change direction at both start and end
            direction = direction * -1;
        }

    }

    //draw a line in the scene view of where the platform is moving too
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);
    }
}
