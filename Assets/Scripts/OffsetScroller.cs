using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

	public float scrollSpeed;
	private Vector2 savedOffset;
	private Renderer _render;
	float x = -1f;
	public bool colorBackground;
	public float minimum;
	public float maximum;
	public float duration;
	private float startTime;

	void Start () {
		startTime = Time.time;
		_render =  GetComponent<Renderer>();
		savedOffset = _render.sharedMaterial.GetTextureOffset ("_MainTex");
	}

	void Update () {
		Sunset ();
		if (colorBackground) {
			float t = 0f;
			if(srcBase.curGameState == GameState.GameOver){
				t = 0f;
				startTime = Time.time;
				_render.sharedMaterial.color = new Color(1f,1f,1f,0f);
			}else{
				t = (Time.time - startTime) / duration;
				//Debug.Log ("t:" + gameObject.GetComponent<SpriteRenderer>().color);
				_render.sharedMaterial.color = new Color (1f, 1f, 1f, Mathf.SmoothStep (minimum, maximum, t));  
			}
		}
	}

	void Sunset(){
		x = Mathf.Repeat (Time.time * scrollSpeed, 2);
		x -= 1;

		Vector2 offset = new Vector2 (x,savedOffset.y + x*0.3f);
		_render.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}

	void Sunrise(){
		x = Mathf.Repeat (Time.time * scrollSpeed, 2);
		x -= 1;

		Vector2 offset = new Vector2 (-x,-(savedOffset.y + x*0.3f));
		_render.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}

	void OnDisable () {
		_render.sharedMaterial.SetTextureOffset ("_MainTex", savedOffset);
	}
}