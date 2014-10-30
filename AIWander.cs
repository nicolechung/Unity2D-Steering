using UnityEngine;
using System.Collections;

// trying out wandering with raycasting and update
public class AIWander : MonoBehaviour {

	public float collisionDistance = 1;
	public float directionDistance = 2;
	public float speed = 1;
	private float direction;
	public float rotateSpeed = 20;

	public bool LockToCameraViewport;
	
	private float viewportWidth;
	private float viewportHeight;
	private Bounds cameraBounds;
	// Use this for initialization
	void Start () {
		// for camera set to orthographic
	
	}


	// Update is called once per frame
	void Update () {

		Debug.DrawRay(transform.position, transform.up, Color.red);
		/* Below is a hack so that this.gameObject is not included as a collider */
		collider2D.enabled = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
		collider2D.enabled = true;

		if ( !hit.collider ) {
			transform.Translate(Vector2.up * speed * Time.smoothDeltaTime);

		} else {

			Debug.DrawRay(transform.position, transform.right, Color.green);
			collider2D.enabled = false;
			RaycastHit2D right = Physics2D.Raycast(transform.position, transform.right, directionDistance);
			RaycastHit2D left = Physics2D.Raycast(transform.position, -transform.right, directionDistance);
			collider2D.enabled = true;


			// if there is an object at the right side of an object then give a random direction
			if ( right.collider ) {
				transform.rotation = Random.rotation;
				/* set rotation to only be on the z-axis for 2D */
				transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
			}

			if ( left.collider ) {
				transform.rotation = Random.rotation;
				/* set rotation to only be on the z-axis for 2D */
				transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
			}

		

		

 
		}

	}
}
