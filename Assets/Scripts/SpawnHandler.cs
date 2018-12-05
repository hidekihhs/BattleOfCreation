using UnityEngine;
using System.Collections;

public class SpawnHandler : MonoBehaviour {

	public GameObject[] platformsCollection;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "SpawnTrigger") {
			var stage = other.gameObject;
			Transform spawnLocation = stage.transform.parent.Find ("SpawnLocation");
			GameObject obj = Instantiate (platformsCollection [Random.Range (0, platformsCollection.Length)], spawnLocation.position, Quaternion.identity) 
				as GameObject;

			obj.name = "SpawnedPlatform";
		}
		if (other.gameObject.name == "SpawnLocation") {
			Destroy (other.gameObject.transform.parent.gameObject);
		}
	}

}
