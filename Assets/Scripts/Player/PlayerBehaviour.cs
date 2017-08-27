using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    Rigidbody2D playerRB;
    float direction, lift;
    float distToGround;

    // Speed adjustment vars
    public float horizontalSpd = 1.6f;
    public float verticalSpd = 2.8f;
    public float maxHorizontalSpd = 6.0f;
    public float gravityScale = 0.6f;
    public float airDrag = 0.0f;
    public float groundDrag = 3.0f;
	public Vector2 ropeRelease = new Vector2 (0, 1000);

    //Simulate Button spamming
    bool verticalCD;

    // Change to proper ground layer number if needed
    public LayerMask groundLayer;
    //Info vars
    public bool isFlying, onGround, facingRight;
	//Anim Tree
	public Animator playerAnim;

	// Use this for initialization
	void Start () {
        playerRB = GetComponent<Rigidbody2D>();
        playerRB.gravityScale = gravityScale;
        playerRB.drag = groundDrag;
        direction = 0.0f;
        lift = 0.0f;
        verticalCD = false;
        distToGround = GetComponent<CircleCollider2D>().bounds.extents.y;
        isFlying = onGround = false;
        groundLayer = LayerMask.GetMask("Ground");
		playerAnim = GetComponent<Animator>(); 
    }

    // Update is used for receiving input
    void Update () {
        direction = Input.GetAxisRaw("Horizontal");
		lift = Input.GetButtonDown("Vertical")? 1.0f:0.0f;
		/*if (!verticalCD && Input.GetAxisRaw("Vertical") > 0)
        {
            lift = Input.GetAxisRaw("Vertical");
        }
        else {
            lift = 0.0f;
        }    
        verticalCD = Input.GetAxisRaw("Vertical") != 0.0f;*/
		isFlying = lift != 0.0f && !onGround;
		onGround = isGrounded();
		Vector2 playerMove = new Vector2(direction * horizontalSpd, lift * verticalSpd);
		if (lift != 0 || onGround)
			playerRB.AddForce(playerMove, ForceMode2D.Impulse);
		if (Mathf.Abs(playerRB.velocity.x) > maxHorizontalSpd)
		{
			playerRB.velocity = new Vector2(Mathf.Sign(playerRB.velocity.x)*maxHorizontalSpd, playerRB.velocity.y);
		}
		isFlying = lift != 0.0f && !onGround;
		onGround = isGrounded();
		setPlayerAnimVars();
    }

    // FixedUpdate is used for movement and methods involving rigidbody
    void FixedUpdate() {
    	/*isFlying = lift != 0 && !onGround;
        onGround = isGrounded();
        Vector2 playerMove = new Vector2(direction * horizontalSpd, lift * verticalSpd);
        if (lift != 0 || onGround)
            playerRB.AddForce(playerMove, ForceMode2D.Impulse);
        if (Mathf.Abs(playerRB.velocity.x) > maxHorizontalSpd)
        {
            playerRB.velocity = new Vector2(Mathf.Sign(playerRB.velocity.x)*maxHorizontalSpd, playerRB.velocity.y);
        }
        isFlying = lift != 0 && !onGround;
        onGround = isGrounded();
		setPlayerAnimVars();*/

		//Facing Directions
		if (direction < 0 && facingRight) {	//Moving Left, facing right (flip)
			Flip ();
		} 
		else if (direction > 0 && !facingRight) { //Moving Right, facing left (flip)
			Flip ();	
		}
    }


    // Checks if on ground through Raycast. Also changes drag
    bool isGrounded()
    {
        bool res = Physics2D.Raycast(transform.position, -Vector2.up, distance:distToGround, layerMask:groundLayer.value);
        if (res) {
            playerRB.drag = groundDrag;
        }
        else
        {
            playerRB.drag = airDrag;
        }
        return res;
    }

	void Flip(){
		//Flip the character Model
		facingRight = !facingRight;

		//Set the new localScale to be inverted
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void setPlayerAnimVars(){
		playerAnim.SetBool ("grounded", onGround);
		if (isFlying) {
			playerAnim.ResetTrigger ("isFlying");
			playerAnim.SetTrigger ("isFlying");
		}
			
		playerAnim.SetFloat ("velocity", playerRB.velocity.magnitude);
	}
}
