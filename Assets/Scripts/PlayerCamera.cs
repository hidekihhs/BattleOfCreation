using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	// Use this for initialization
	Transform player;
	public float horizontalOffset = 6f;

	void Start () {
		player =   GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		var x = player.position.x;
		transform.position = new Vector3 (x + horizontalOffset, transform.position.y, -10);
	}
}
