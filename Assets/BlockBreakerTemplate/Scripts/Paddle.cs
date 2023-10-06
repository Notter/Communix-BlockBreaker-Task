using UnityEngine;

public class Paddle : MonoBehaviour 
{
	[SerializeField] float speed;		//The amount of units the paddle will move a second
	[SerializeField] float minX;		//The minimum x position that the paddle can move to
    [SerializeField] float maxX;		//The maximum x position that the paddle can move to
    [SerializeField] AudioClip hitSFX;

	Rigidbody2D rig;                    //The paddle's rigidbody 2D component

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
	{
        float direction = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(direction * speed * Time.deltaTime, 0);

        // Mouse control
        if (Input.GetMouseButton(0)) // Left mouse button pressed
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 paddlePosition = rig.position;

            // Set the paddle's X velocity to move toward the mouse
            Vector2 mouseVelocity = new Vector2(mousePosition.x - paddlePosition.x, 0);
            rig.velocity = mouseVelocity.normalized * speed * Time.deltaTime;
        }

        //Clamps the position so that it doesn't go below the 'minX' or past the 'maxX' values
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, 0);	
	}

	//Called whenever a trigger has entered this objects BoxCollider2D. The value 'col' is the Collider2D object that has interacted with this one
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.CompareTag("Ball"))                                       //Is the colliding object got the tag "Ball"?
		{
			AudioManager.Instance.PlayClip(hitSFX);
			col.gameObject.GetComponent<Ball>().SetDirection(transform.position);   //Bounce the ball of the paddle
		}
	}

	//Called when the paddle needs to be reset to the middle of the screen
	public void ResetPaddle () => transform.position = new Vector3(0, transform.position.y, 0);
	
}
