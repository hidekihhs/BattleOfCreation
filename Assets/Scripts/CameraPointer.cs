using UnityEngine;
using System.Collections;

public class CameraPointer : MonoBehaviour {

	// Use this for initialization
	Transform cameraPointer;
	public float horizontalOffset = 6f;

	void Start () {
		cameraPointer =   GameObject.FindGameObjectWithTag ("CameraPointer").transform;
	}

	// Update is called once per frame
	void Update () {
		var x = cameraPointer.position.x;
		transform.position = new Vector3 (x + horizontalOffset, transform.position.y, -10);
	}
}
