using UnityEngine;
using System.Collections;

public class RCFlee : MonoBehaviour {

	public float directionDistance = 2;
	public float speed = 1;
	private float direction;
	public float targetDistance = 1;
	public GameObject Target;
	public bool LockToCameraViewport;
	public float rotationRange = 2;

	private bool hasHitTarget;

	private static bool DEBUG = true;
	private static bool DEBUG_DRAW = true;

	private string state;

	// Use this for initialization
	void Start () {
		state = "flee";
		hasHitTarget = false;
		StartCoroutine( Flee () );
	}
	
	// Update is called once per frame
	void Update () {
		switch(state) {
		case "flee":
			
			break;
			
		
			
			
		case "start-rotate":
			state="rotate";
			StartCoroutine( avoidObstacle() );
			break;
			
		default: 
			
			break;
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		
		if (col.gameObject == Target) {
			hasHitTarget = true;
		}
	}

	IEnumerator Flee() {
		while (!hasHitTarget && state=="flee") {
			bool flee;
			flee = fleeCheck();
			if (DEBUG) Debug.Log ("fleeing: okay?");
			if (DEBUG) Debug.Log (flee);
			if (flee) {	
				transform.Translate (Vector2.up * speed * Time.smoothDeltaTime);
			} else {
				state="start-rotate";
			}
			yield return null;
		}
	}

	IEnumerator avoidObstacle() {
		while(state=="rotate") {
			// save randomAngle in a array
			if (DEBUG) Debug.Log ("--state: rotate--");
			float randomAngle = Random.Range (-rotationRange, rotationRange);
			Vector2 directionChange = new Vector2(randomAngle, 0);

			
			Vector2 originalDirection = Target.transform.position;
			Vector2 direction = originalDirection + directionChange;
			
			// choose a random direction and do an obstacle check in that direction
			bool obstacle;
			obstacle = hasObstacles(direction, "yellow");
			if (DEBUG) Debug.Log ("---obstacle?:---");
			if (DEBUG) Debug.Log (obstacle);
			if (!obstacle) {
				Debug.Log ("---no obstacle, moving:---");
				float zRotation = Mathf.Atan2( (direction.y - transform.position.y), (direction.x - transform.position.x) ) * Mathf.Rad2Deg - 90;
				transform.eulerAngles = new Vector3(0, 0, zRotation);
				// clear the rotations list
			
				// if there is no obstacle, then actually rotate in that direction
				state="start-move";
			} else {
				yield return null;
				
			}
		}
		
	}

	bool fleeCheck() {

		// rotate in the opposite direction of your target
		float zRotation = Mathf.Atan2( (transform.position.y - Target.transform.position.y), (transform.position.x - Target.transform.position.x) ) * Mathf.Rad2Deg - 90;
		transform.eulerAngles = new Vector3(0, 0, zRotation);
	
		// get the direction **away** from the target
		Vector2 direction = (transform.position - Target.transform.position).normalized;

		// check to see if anything is in the way
		bool hasObstacle = hasObstacles(direction, "blue");

		// if there are obstacles, don't flee (rotate), otherwise, flee (Translate)
		return !hasObstacle;
	}

	bool hasObstacles(Vector2 direction, string colorString) {
		RaycastHit2D[] hits;
		RaycastHit2D[] hitsLeft;
		RaycastHit2D[] hitsRight;
		
		
		Vector2 directionLeft;
		
		Vector2 directionRight;
		// move the origin of the Raycast so that it's outside of the collider
		
		Color color;
		// move the origin of the Raycast so that it's outside of the collider
		
		switch(colorString) {
		case "blue":
			color = Color.cyan;
			break;
			
		case "red":
			color = Color.red;
			break;
			
		case "yellow":
			color = Color.yellow;
			break;
			
		default:
			color = Color.black;
			break;
		}
		
		bool hasObstacle = false;
		if (DEBUG_DRAW) Debug.DrawRay (transform.position, direction*directionDistance, color);
		
		hits = Physics2D.RaycastAll (transform.position, direction, directionDistance, 1 << 8);
		
		Vector2 left = new Vector2(-0.3F, 0);
		Vector2 leftOrigin = new Vector2(transform.position.x, transform.position.y) + left;
		directionLeft = direction + left;
		hitsLeft =  Physics2D.RaycastAll (leftOrigin, directionLeft, directionDistance, 1 << 8);
		if (DEBUG_DRAW) Debug.DrawRay (leftOrigin, directionLeft*directionDistance, color);
		
		Vector2 right = new Vector2(0.3F, 0);
		Vector2 rightOrigin = new Vector2(transform.position.x, transform.position.y) + right;
		directionRight = direction + right;
		hitsRight =  Physics2D.RaycastAll (rightOrigin, directionRight, directionDistance, 1 << 8);
		if (DEBUG_DRAW) Debug.DrawRay (rightOrigin, directionRight*directionDistance, color);
		// is there a collision?
		
		foreach(RaycastHit2D hit in hits) {
			if (hit && hit.collider) {
				hasObstacle = true;
			}
		}
		
		foreach(RaycastHit2D hit in hitsLeft) {
			if (hit && hit.collider) {
				hasObstacle = true;
			}
		}
		
		foreach(RaycastHit2D hit in hitsRight) {
			if (hit && hit.collider) {
				hasObstacle = true;
			}
		}
		
		return hasObstacle;
	}
}
