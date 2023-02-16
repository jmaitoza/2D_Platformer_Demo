using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpForce = 12.0f; // controls how high you can jump
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // need this other component attached to this GameObject
        anim = GetComponent<Animator>(); //attaches animations to player 
        box = GetComponent<BoxCollider2D>(); //uses the players collider as an area to check for if you hit the ground
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, body.velocity.y); // set only horizontal movement; preserve preexisting vertical movement.
        body.velocity = movement;
        
        //for ground detection
        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        //check below the colliders min Y values
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hit != null) // if collider was detected under the player
            grounded = true;
        
        //turn off gravity when stading on the ground
        body.gravityScale = (grounded && Mathf.Approximately(deltaX, 0)) ? 0 : 1; //<- to check both that the player is on the ground and not moving

        if (grounded && Input.GetKeyDown(KeyCode.Space)) //determines the jump key, in this case the 'SpaceBar'
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //specifically adds an impulse which is a sudden jolt, as opposed to a continuously applied force
        }

        //attach player to moving platform
        MovingPlatform platform = null;
        if (hit != null)
            platform = hit.GetComponent<MovingPlatform>(); //check whether platform under the player is moving platform
        if (platform != null) // either attach to the platform or clear transform.parent
            transform.parent = platform.transform;
        else
        {
            transform.parent = null;
        }

        //animation code
        anim.SetFloat("speed", Mathf.Abs(deltaX)); // speed is greater than zero even if velocity is negative

        Vector3 pScale = Vector3.one; //default scale 1 if not on moving platform
        if (platform != null)
            pScale = platform.transform.localScale;
        
        if (!Mathf.Approximately(deltaX, 0)) // floats arent always exact, so compare using Approximately()
        {
           transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1/pScale.y, 1);
        }
    }
}
