using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	//public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	public Vector2 speed = new Vector2(25, 25);
	
	// 1 - Store the movement
	private Vector2 movement;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");
//		float translation = Input.GetAxis("Vertical") * speed;
//		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
//		translation *= Time.deltaTime;
//		rotation *= Time.deltaTime;
//		transform.Translate(0, translation, 0);
//		transform.Rotate(0, 0, rotation);

		// 3 - Movement per direction
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
	}
	
	void FixedUpdate() {
		rigidbody2D.velocity = movement;
	}
}