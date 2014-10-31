﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// trying out wandering with raycasting and update
public class RCWander : MonoBehaviour {
	
	
	public float collisionDistance = 2;
	public float speed = 1;
	private float direction;
	public bool LockToCameraViewport;
	
	
	private float viewportWidth;
	private float viewportHeight;
	private Bounds cameraBounds;
	private string state;

	
	private static bool DEBUG = false; // to turn debug messages on an off
	private static bool DEBUG_DRAW = false; // to turn Debug lines on and off
	private static int LAYER_MASK = 8; // make sure your player isn't on this list!
	public int straightAhead = 10; // how much to go in a given direction before turning
	private int count;
	// todo: store am array of directions
	Vector2 origin;
	
	
	// Use this for initialization
	void Start () {
		// start out seeking if the Target is within range
		state = "wander";
		count = straightAhead;
		StartCoroutine( Wander () );
		
	}

	
	IEnumerator Wander() {
		// check if there is something in the way of the target
		
		while (state=="wander" && count > 0) {
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
			count--;
			if (count == 0) state = "start-rotate";
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
		if (DEBUG_DRAW) Debug.DrawRay (transform.position, direction*collisionDistance, color);
		
		hits = Physics2D.RaycastAll (transform.position, direction, collisionDistance, 1 << LAYER_MASK);
		
		Vector2 left = new Vector2(-0.3F, 0);
		Vector2 leftOrigin = new Vector2(transform.position.x, transform.position.y) + left;
		directionLeft = direction + left;
		hitsLeft =  Physics2D.RaycastAll (leftOrigin, directionLeft, collisionDistance, 1 << LAYER_MASK);
		if (DEBUG_DRAW) Debug.DrawRay (leftOrigin, directionLeft*collisionDistance, color);
		
		Vector2 right = new Vector2(0.3F, 0);
		Vector2 rightOrigin = new Vector2(transform.position.x, transform.position.y) + right;
		directionRight = direction + right;
		hitsRight =  Physics2D.RaycastAll (rightOrigin, directionRight, collisionDistance, 1 << LAYER_MASK);
		if (DEBUG_DRAW) Debug.DrawRay (rightOrigin, directionRight*collisionDistance, color);
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
	
	

	
	// need optional last direction
	
	IEnumerator rotatePlayer() {
		while(state=="rotate") {
			if (DEBUG) Debug.Log ("wander: rotating player");
			if (DEBUG_DRAW) Debug.DrawRay(transform.position, transform.right, Color.green);

			if (DEBUG) Debug.Log ("rotating");
			transform.rotation = Random.rotation;
			/* set rotation to only be on the z-axis for 2D */
			transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
			state = "wander";
			StartCoroutine( Wander () );
			
			yield return null;
		}
		
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		switch(state) {
		

			
		case "start-rotate":
			state="rotate";
			count = straightAhead;	
			StartCoroutine( rotatePlayer() );
			break;
			
		default: 
			
			break;
		}
		
	}
	

}