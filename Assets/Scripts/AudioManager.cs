using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance = null;
	public AudioSource _audio;
	public AudioSource _audioBrushEffect;
	public AudioClip[] clips;
	public Slider volumeSlider;

	public void Awake(){

		if (instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this) {
			Destroy(gameObject);
		}
	}
	// Use this for initialization
	void Update () {
		srcBase.volume = volumeSlider.value;
		if(volumeSlider==null&&Interface.instance.UiPause.gameObject.activeInHierarchy){
			volumeSlider = GameObject.FindWithTag("volumeSlider").GetComponent<Slider>() as Slider;
		}
		if (_audioBrushEffect == null) {
			_audioBrushEffect = GetComponent<AudioSource>();
		}
		if(_audio == null){
			_audio = Camera.main.GetComponent<AudioSource>();
		}else{
			_audio.volume = srcBase.volume-0.1f; // 1 a menos para deixar a musica sempre mais baixa que os efeitos.
			_audioBrushEffect.volume = srcBase.volume;
		}
	}
}