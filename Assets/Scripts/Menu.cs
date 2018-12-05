using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	float volume = 0.2f;

	// Variaveis Da Splash Screen.
	public float secondsVisible = 2.0f;
	public CanvasGroup[] canGroup;
	public Canvas menuCanv;
	public Animator uiCreditos;

	void Start(){
		StartCoroutine(TransitionImage());	
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
	}

	IEnumerator TransitionImage(){
		// Executa essa tarefa em cada Splash Image.
		for(int i=0;i<canGroup.Length;i++){
			// Espera 1 Segundo.
			yield return new WaitForSeconds(1f);
			// Fade in.
			float t = 0;
			while (t <= 1.0f){
				t +=Time.deltaTime;
				canGroup[i].alpha = t;
				yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
			}
			t = 1f;
			// Espera o Tempo Visivel.
			yield return new WaitForSeconds(secondsVisible);
			// Fade Out.
			while (t >= 0.0f){
				t -=Time.deltaTime;
				canGroup[i].alpha = t;
				yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
			}
			t = 0f;
			// Destroi as Splash Images para nao atrapalhar o Menu.
			Destroy(canGroup[i].gameObject);
		}
		// Inicia o Menu
		menuCanv.gameObject.SetActive(true);
		// Espera 1 Segundo.
		yield return new WaitForSeconds(1f);
		// Fade in.
		float temp = 0;
		while (temp <= 1.0f){
			temp +=Time.deltaTime;
			menuCanv.GetComponent<CanvasGroup>().alpha = temp;
			yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
		}
		temp = 1f;
		AudioManager.instance._audio.Play();
	}

	IEnumerator LoadingAnim(int id){
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(id);
	}

	public void LoadScene(int id){
		StartCoroutine(LoadingAnim(id));
	}
	public void enableTutorial(){
		srcBase.showTutorial = !srcBase.showTutorial;
	}
}