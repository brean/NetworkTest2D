using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum Direction {
    UP, DOWN, LEFT, RIGHT
};

public class PlayerController : NetworkBehaviour {
    [SyncVar(hook="OnDirectionChange")]
    public Direction direction;

    public Rigidbody2D rb;
    public float speed = 2.0f;

    /**
     * data updating from server, called when direction changes inside 
     * a command.
     */
    public void OnDirectionChange(Direction dir)
    {
        MoveDirection(dir);
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
    /**
     * change direction and move player
     */
    void MoveDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                transform.localEulerAngles = new Vector3(0, 270, 0);
                rb.velocity = new Vector2(speed, 0);
                break;
            case Direction.LEFT:
                transform.localEulerAngles = new Vector3(0, 90, 0);
                rb.velocity = new Vector2(-speed, 0);
                break;
            case Direction.UP:
                transform.localEulerAngles = new Vector3(180, 0, 0);
                rb.velocity = new Vector2(0, speed);
                break;
            case Direction.DOWN:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                rb.velocity = new Vector2(0, -speed);
                break;
        }
    }

    /**
     * code that will be executed on the server, when we change
     * the direction it will be updated on the client. 
     * If you set direction directly in the Update loop after keyboard input
     * it will only update and send the host player variable, the
     * client will be stuck.
     */
    [Command]
    public void CmdSetDirection(Direction dir)
    {
        this.direction = dir;
    }

	// Update is called once per frame
    // please note that this controller is fairly simple, so the game does
    // not feel right yet - the player controls do not react smoothly.
	void Update () {
        // every frame we slow down the player, no matter if he is local 
        // or server
        if (rb != null)
        {
            rb.velocity *= 0.5f;
        }
        if (!isLocalPlayer)
        {
            return;
        }
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        if (horizontal > 0) {
            CmdSetDirection(Direction.RIGHT);
        } else if (horizontal < 0) {
            CmdSetDirection(Direction.LEFT);
        } else if (vertical > 0) {
            CmdSetDirection(Direction.UP);
        } else if (vertical < 0) {
            CmdSetDirection(Direction.DOWN);
        }
    }
}
