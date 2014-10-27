using UnityEngine;
using System.Collections;

// trying out wandering with raycasting and update
public class AISeeker : MonoBehaviour {
	

	public float directionDistance = 2;
	public float speed = 1;
	private float direction;
	public float targetDistance = 1;
	public GameObject Target;
	public bool LockToCameraViewport;
	
	
	private float viewportWidth;
	private float viewportHeight;
	private Bounds cameraBounds;
	private SpriteRenderer seekerBounds;
	private bool steer;

	// Use this for initialization
	void Start () {
		// for camera set to orthographic
		seekerBounds = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnCollisionEnter2D (Collision2D col) {
		transform.rotation = Random.rotation;
		/* set rotation to only be on the z-axis for 2D */
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
	}
	

	void Move (Vector3 direction, float zRotation)
	{

		/* Below is a hack so that this.gameObject is not included as a collider */
		RaycastHit2D hit;
		//collider2D.enabled = false;
		Vector2 origin = new Vector2(transform.position.x+this.renderer.bounds.extents.x, transform.position.y+this.renderer.bounds.extents.y);
		if (!steer) {
			Debug.DrawRay (origin, direction*directionDistance, Color.green);
			hit = Physics2D.Raycast (origin, direction, directionDistance);
		} else {
			Debug.DrawRay (origin, transform.up*directionDistance, Color.green);
			hit = Physics2D.Raycast (origin, transform.up, directionDistance);
		}
		collider2D.enabled = true;
		Debug.Log ("--check hit.collider--");
		Debug.Log (hit.collider == null);
		if (hit.collider==null) {
			Debug.Log ("translating");

			if (!steer) {
				transform.eulerAngles = new Vector3(0, 0, zRotation);
				steer = true;
			}

			transform.Translate (Vector2.up * speed * Time.smoothDeltaTime);

		}
		else {
			steer = false;
			Debug.Log(hit.collider);
			Debug.Log ("rotating");
			Debug.DrawRay (transform.position, transform.right, Color.green);
			float random = Random.Range (-30, 30);
			transform.rotation = Quaternion.AngleAxis(random, Vector3.up);
			/* set rotation to only be on the z-axis for 2D */
			transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
			//collider2D.enabled = false;

			RaycastHit2D check = Physics2D.Raycast (origin, transform.up, directionDistance);
			Debug.DrawRay (origin, transform.up*directionDistance, Color.blue);

			if (check.collider == null) {
				transform.Translate (Vector2.up * speed * Time.smoothDeltaTime);
			}

		}
	}	
	
	// Update is called once per frame
	void Update () {
		// if too far wander
		float distance = Vector2.Distance(Target.transform.position,transform.position);
		if (distance > targetDistance) {
			Move(transform.up, 0);
		} else {	
			float zRotation = Mathf.Atan2( (Target.transform.position.y - transform.position.y), (Target.transform.position.x - transform.position.x) ) * Mathf.Rad2Deg - 90;
			Vector2 direction = (Target.transform.position - transform.position).normalized;

			Move (direction, zRotation);

		}
		
		
	}


}
