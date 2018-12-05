using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
	public CanvasGroup loadingCanv;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);	
	}
	void OnLevelWasLoaded(){
		StartCoroutine("fadeOut");
	}

	IEnumerator fadeOut(){
		// Fade Out.
		float t = 1;
		while (t >= 0.0f){
			t -=Time.deltaTime;
			loadingCanv.alpha = t;
			yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
		}
		t = 0f;
		gameObject.SetActive(false);
	}
}