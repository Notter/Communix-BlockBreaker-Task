using UnityEngine;

public class Brick : MonoBehaviour 
{
	public BlockBreakerLogic blockBreaker;

    [SerializeField] AudioClip hitSFX;
    [SerializeField] GameObject bloodDrip;

    //Called whenever a trigger has entered this objects BoxCollider2D. The value 'col' is the Collider2D object that has interacted with this one
    void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.CompareTag("Ball"))											//Is the tag of the colliding object 'Ball'
        {
            Instantiate(bloodDrip, transform.position - new Vector3(0, 0.5f), Quaternion.identity);
            AudioManager.Instance.PlayClip(hitSFX);
            GameManager.Instance.AddScore();											//Increases the score value in the GameManager class by one
			col.gameObject.GetComponent<Ball>().SetDirection(transform.position);       //Sending over the brick's position
            blockBreaker.RemoveBrick(gameObject);										//Removes this brick from the 'bricks' list in the GameManager

			Destroy(gameObject);														//Destroy's the brick
		}
	}
}
