﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// trying out wandering with raycasting and update
public class RCSeeker : MonoBehaviour {
	
	
	public float directionDistance = 2;
	public float speed = 1;
	private float direction;
	public float targetDistance = 1;
	public GameObject Target;
	public bool LockToCameraViewport;
	
	
	private float viewportWidth;
	private float viewportHeight;
	private Bounds cameraBounds;
	private string state;
	
	private bool isHitTarget;
	
	private static bool DEBUG = true;
	private static bool DEBUG_DRAW = true;
	private List<float> rotations;
	private float rotationRange = 2;

	
	// todo: store am array of directions
	Vector2 origin;
	
	
	// Use this for initialization
	void Start () {
		// start out seeking if the Target is within range
		state = "seek";
		isHitTarget = false;
		rotations = new List<float>();
		StartCoroutine( Seek () );
		
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		
		if (col.gameObject.name == "Player") {
			isHitTarget = true;
		}
	}
	
	IEnumerator Seek() {
		// check if there is something in the way of the target
		
		while (!isHitTarget && state=="seek") {
			bool seek;
			seek = seekCheck();
			Debug.Log ("seeking: seek?");
			Debug.Log (seek);
			if (seek) {	
				transform.Translate (Vector2.up * speed * Time.smoothDeltaTime);
			} else {
				state="start-rotate";
			}
			yield return null;
		}
	}
	
	
	IEnumerator AvoidMove() {
		Debug.Log ("--moving around obstacle--");
		bool obstacle;
		bool seek;
		
		while(state=="avoid") {
			// check if there is something in the way of the target
			transform.Translate (Vector2.up * speed * Time.smoothDeltaTime);
			seek = seekCheck();
			obstacle = obstacleCheck();
			Debug.Log ("--is there an obstacle?--");
			Debug.Log (obstacle);
			Debug.Log ("avoiding state, keep seeking?");
			Debug.Log (seek);
			
			// inside here, if there is an obstacle, change state to "rotate"
			if (obstacle) {
				state="start-rotate";
				yield return null;
			}
			// else start seek
			if (seek && !obstacle) {
				Debug.Log ("--starting seek--");
				state = "seek";
				StartCoroutine( Seek () );
			} 
			transform.Translate (Vector2.up * speed * Time.smoothDeltaTime);
			
			yield return null;
		}
		
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
		Debug.DrawRay (transform.position, direction*directionDistance, color);
		
		hits = Physics2D.RaycastAll (transform.position, direction, directionDistance, 1 << 8);
		
		Vector2 left = new Vector2(-0.3F, 0);
		Vector2 leftOrigin = new Vector2(transform.position.x, transform.position.y) + left;
		directionLeft = direction + left;
		hitsLeft =  Physics2D.RaycastAll (leftOrigin, directionLeft, directionDistance, 1 << 8);
		Debug.DrawRay (leftOrigin, directionLeft*directionDistance, color);
		
		Vector2 right = new Vector2(0.3F, 0);
		Vector2 rightOrigin = new Vector2(transform.position.x, transform.position.y) + right;
		directionRight = direction + right;
		hitsRight =  Physics2D.RaycastAll (rightOrigin, directionRight, directionDistance, 1 << 8);
		Debug.DrawRay (rightOrigin, directionRight*directionDistance, color);
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
	
	
	bool seekCheck() {
		
		if (state=="seek") {
			float zRotation = Mathf.Atan2( (Target.transform.position.y - transform.position.y), (Target.transform.position.x - transform.position.x) ) * Mathf.Rad2Deg - 90;
			transform.eulerAngles = new Vector3(0, 0, zRotation);
		}
		
		Vector2 direction = (Target.transform.position - transform.position).normalized;
		
		bool hasObstacle = hasObstacles(direction, "blue");
		
		return !hasObstacle;
		
	}
	
	bool obstacleCheck() {
	
		
		Vector2 direction;
		direction = transform.up;
		bool hasObstacle = hasObstacles(direction, "yellow");

		
		return hasObstacle;
	}
	
	
	// need optional last direction
	
	IEnumerator avoidObstacle() {
		while(state=="rotate") {
			// save randomAngle in a array
			Debug.Log ("--state: rotate--");
			float randomAngle = Random.Range (-rotationRange, rotationRange);
			Vector2 directionChange = new Vector2(randomAngle, 0);
			if (rotations.Count > 0) {
				foreach(float elem in rotations) {
					Debug.Log (elem);
					if ( CheckRange(randomAngle, elem-0.1F, elem+0.1F) ) {
						Debug.Log ("--too close to last attempt--");
						yield return null;					
					}
				}	
			}
			
			Vector2 originalDirection = Target.transform.position;
			Vector2 direction = originalDirection + directionChange;
			
			// choose a random direction and do an obstacle check in that direction
			bool obstacle;
			obstacle = hasObstacles(direction, "yellow");
			Debug.Log ("---obstacle?:---");
			Debug.Log (obstacle);
			if (!obstacle) {
				Debug.Log ("---no obstacle, moving:---");
				float zRotation = Mathf.Atan2( (direction.y - transform.position.y), (direction.x - transform.position.x) ) * Mathf.Rad2Deg - 90;
				transform.eulerAngles = new Vector3(0, 0, zRotation);
				// clear the rotations list
				rotations.Clear();
				// if there is no obstacle, then actually rotate in that direction
				state="start-move";
			} else {
				rotations.Add(randomAngle);
				yield return null;
				
			}
		}

	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		switch(state) {
		case "seek":
			
			break;
			
		case "start-move":
			state="avoid";
			StartCoroutine( AvoidMove() );
			
			break;
		

		case "start-rotate":
			state="rotate";
			StartCoroutine( avoidObstacle() );
			break;
			
		default: 
			
			break;
		}
		
	}
	
	
	// helpers
	
	bool CheckRange(float num, float min, float max) {
		return num > min && num < max;
	}
	
}