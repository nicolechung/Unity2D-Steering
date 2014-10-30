﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// trying out wandering with raycasting and update
public class RCWander : MonoBehaviour {
	
	
	public float directionDistance = 2;
	public float speed = 1;
	private float direction;
	public bool LockToCameraViewport;
	
	
	private float viewportWidth;
	private float viewportHeight;
	private Bounds cameraBounds;
	private string state;

	
	private static bool DEBUG = true;
	private static bool DEBUG_DRAW = true;
	private float rotationRange = 2;
	private static int LAYER_MASK = 8; // make sure your player isn't on this list!


	// todo: store am array of directions
	Vector2 origin;
	
	
	// Use this for initialization
	void Start () {
		// start out seeking if the Target is within range
		state = "wander";
		StartCoroutine( Wander () );
		
	}

	
	IEnumerator Wander() {
		// check if there is something in the way of the target
		
		while (state=="wander") {
			bool obstacles;
			Vector2 direction = transform.up;
			if (DEBUG) Debug.Log (direction);
			obstacles = hasObstacles(direction, "blue");
			if (DEBUG) Debug.Log ("wander: has obstacle");
			if (DEBUG) Debug.Log (obstacles);
			if (!obstacles) {	
				transform.Translate(Vector2.up * speed * Time.smoothDeltaTime);
			} else {
				if (DEBUG) Debug.Log ("wander: start rotating");
				state="start-rotate";
				yield return null;
			}
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
		if (DEBUG_DRAW) Debug.DrawRay (transform.position, direction*directionDistance, color);
		
		hits = Physics2D.RaycastAll (transform.position, direction, directionDistance, 1 << LAYER_MASK);
		
		Vector2 left = new Vector2(-0.3F, 0);
		Vector2 leftOrigin = new Vector2(transform.position.x, transform.position.y) + left;
		directionLeft = direction + left;
		hitsLeft =  Physics2D.RaycastAll (leftOrigin, directionLeft, directionDistance, 1 << LAYER_MASK);
		if (DEBUG_DRAW) Debug.DrawRay (leftOrigin, directionLeft*directionDistance, color);
		
		Vector2 right = new Vector2(0.3F, 0);
		Vector2 rightOrigin = new Vector2(transform.position.x, transform.position.y) + right;
		directionRight = direction + right;
		hitsRight =  Physics2D.RaycastAll (rightOrigin, directionRight, directionDistance, 1 << LAYER_MASK);
		if (DEBUG_DRAW) Debug.DrawRay (rightOrigin, directionRight*directionDistance, color);
		// is there a collision?
		
		foreach(RaycastHit2D hit in hits) {
			Debug.Log(hit);
			Debug.Log (hit.collider);
			if (hit && hit.collider) {
				hasObstacle = true;
			}
		}
		
//		foreach(RaycastHit2D hit in hitsLeft) {
//			if (hit && hit.collider) {
//				hasObstacle = true;
//			}
//		}
//		
//		foreach(RaycastHit2D hit in hitsRight) {
//			if (hit && hit.collider) {
//				hasObstacle = true;
//			}
//		}
		
		return hasObstacle;
	}
	
	

	
	// need optional last direction
	
	IEnumerator rotatePlayer() {
		while(state=="rotate") {
			Debug.Log ("wander: rotating player");
			Debug.DrawRay(transform.position, transform.right, Color.green);
			RaycastHit2D []right = Physics2D.RaycastAll(transform.position, transform.right, directionDistance);
			RaycastHit2D []left = Physics2D.RaycastAll(transform.position, -transform.right, directionDistance);

			bool rotate = false;
			foreach(RaycastHit2D hit in right) {
				if (hit && hit.collider) {
					rotate = true;
				}
			}

			foreach(RaycastHit2D hit in left) {
				if (hit && hit.collider) {
					rotate = true;
				}
			}

			if (rotate) {
				transform.rotation = Random.rotation;
				/* set rotation to only be on the z-axis for 2D */
				transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
				state="wander";
				StartCoroutine( Wander() );
				yield return null;
			} 


			yield return null;
		}
		
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		switch(state) {
		

			
		case "start-rotate":
			state="rotate";
			StartCoroutine( rotatePlayer() );
			break;
			
		default: 
			
			break;
		}
		
	}
	

}