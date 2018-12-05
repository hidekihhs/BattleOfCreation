using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour {
	public int scoreColor;
	private SpriteRenderer sprite;
	void Start() {
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	void Update() {
		if(srcBase.curGameState == GameState.GameOver){
			//t = 0f;
			sprite.color = new Color(1f,1f,1f,0f);
		}else{
			sprite.color = new Color(1f,1f,1f,0f);
			if (srcBase.score >= scoreColor) {
				sprite.color = new Color (1f, 1f, 1f, 1f);
			}
		}
	}
}