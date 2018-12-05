using UnityEngine;
using System.Collections;

public class ScenarioScroller : MonoBehaviour {

	// Use this for initialization
	public float scrollSpeed;
	public float tileSizeZ;

	void Start ()
	{
	}

	void Update (){
		//transform.localPosition = Vector3.MoveTowards(new Vector3(transform.localPosition.x,transform.localPosition.y,tileSizeZ), new Vector3 (transform.localPosition.x - scrollSpeed,transform.localPosition.y,tileSizeZ),scrollSpeed*Time.deltaTime);
	}
		
}
