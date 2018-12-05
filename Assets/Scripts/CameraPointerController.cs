using UnityEngine;
using System.Collections;

public class CameraPointerController : MonoBehaviour {

	// Use this for initialization
	public float speed = 6.0f;
	private Rigidbody2D bodyPointer;

	void FixedUpdate(){
		bodyPointer = GetComponent<Rigidbody2D> ();
		bodyPointer.velocity = new Vector2 (speed, bodyPointer.velocity.y);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
