using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject background;
	public Transform player;

	private Bounds bounds;
	private float viewportWidth;
	private float viewportHeight;

	// Use this for initialization
	void Start () {
		bounds = background.renderer.bounds;

		// for camera set to orthographic
		viewportHeight = Camera.main.camera.orthographicSize;
		viewportWidth = viewportHeight * Screen.width/Screen.height;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 pos = new Vector3(player.position.x, player.position.y, -10);
		pos.x = Mathf.Clamp (pos.x, bounds.min.x + viewportWidth, bounds.max.x - viewportWidth);
		pos.y = Mathf.Clamp (pos.y, bounds.min.y + viewportHeight, bounds.max.y - viewportHeight);
		transform.position = pos;
	}
}
