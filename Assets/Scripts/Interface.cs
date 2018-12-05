using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
	public static Interface instance = null;	
	public GameObject UiGameover;
	public GameObject UiPause;
	public GameObject UiTutorial;
	public GameObject UiNewScore;
	public Slider barraTinta;
	public Slider volumeSlider;
	public Text record;
	public Text HighScore;
	public Text HudScore;
	public GameObject[] enemySpawnPoints;
	public SpawnEnemy[] spawnScript;

	public void Awake () {
		if(instance == null){
			instance = this;
		}
		else if(instance != this){
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		volumeSlider.value = srcBase.volume;
		// Carrega o Save.
		srcBase.Load();
		// Verifica se inicia o Tutorial ou vai para a Gameplay.
		if(srcBase.showTutorial){
			srcBase.curGameState = GameState.Tutorial;
			StartCoroutine("ShowTuto");
		}else{
			srcBase.curGameState = GameState.GamePlay;
		}

		// Pega os pontos de respawn para depois poder resetar.
		if(enemySpawnPoints.Length>0){
			spawnScript = new SpawnEnemy[enemySpawnPoints.Length];
			for(int i=0;i<enemySpawnPoints.Length;i++){
				spawnScript[i] = enemySpawnPoints[i].GetComponent<SpawnEnemy>() as SpawnEnemy;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		switch(srcBase.curGameState){
		case GameState.Tutorial:

			break;
		case GameState.GamePlay:
			// Calcula score de tempo
			//	srcBase.CalcTimeScore();

			if(AudioManager.instance!=null&&(!AudioManager.instance._audio.isPlaying||AudioManager.instance._audio.clip == AudioManager.instance.clips[1])){
				PlayAudio(0,false);	
			}
			if(Input.GetKeyUp(KeyCode.Escape)){
				srcBase.curGameState = GameState.Pause;
				ShowPauseScreen();
			}
			HudScore.text = "Score: "+srcBase.score;
			break;
		case GameState.Pause:
			if(Input.GetKeyUp(KeyCode.Escape)){
				HidePauseScreen();
			}
			break;
		case GameState.GameOver:
			srcBase.dead = true;
			if(AudioManager.instance!=null && AudioManager.instance._audio.clip == AudioManager.instance.clips[0]){
				PlayAudio(1,false);
			}
			StartCoroutine("ShowGameOverScreen");
			if(Input.GetMouseButtonUp(0)){
				UiNewScore.SetActive(false);
			}
			break;
		}
	}
	IEnumerator ShowGameOverScreen(){
		yield return new WaitForSeconds(1f);
		Time.timeScale = 1f;
		UiGameover.SetActive(true);
		record.text = srcBase.score.ToString();
		HighScore.text = srcBase.highScore.ToString();
	}
	IEnumerator ShowTuto(){
		yield return new WaitForSeconds(.5f);
		Time.timeScale = 0f;
		UiTutorial.SetActive(true);
	}
	public void HideTuto(){
		UiTutorial.SetActive(false);
		srcBase.curGameState = GameState.GamePlay;
		Time.timeScale = 1f;
	}
	public void HideGameOverScreen(){
		srcBase.curGameState = GameState.GamePlay;
		UiGameover.SetActive(false);
		if(srcBase.ENEMYHOLDER!=null){
			Destroy(srcBase.ENEMYHOLDER.gameObject);
		}
		// Resetamos o Player
		srcBase.Player.gameObject.transform.localPosition = new Vector3(0f,-3.7f,0f);
		srcBase.Player.gameObject.transform.localRotation = Quaternion.identity;
		srcBase.curPlayerState = EnemyState.Perseguindo;
		PlayerController.instance.bodyPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
		PlayerController.instance.newAnim.SetBool("Dead",false);
		srcBase.Player.gameObject.SetActive(true);

		if(PlayerController.instance.latestFacing == PlayerController.Facing.Left)
			PlayerController.instance.transform.Rotate (0, 180, 0);

		// Resetamos o sourceBase
		srcBase.startTime = Time.time;
		srcBase.dead = false;
		srcBase.score=0;

		// Reiniciamos os inimigos
		if(spawnScript.Length>0){
			for(int i=0;i<enemySpawnPoints.Length;i++){
				spawnScript[i].RestartSpawnEnemy();
			}
		}
	}

	public void quit(){
		Application.Quit ();
	}
	public void ShowPauseScreen(){
		Time.timeScale = 0f;
		UiPause.SetActive(true);
	}
	public void HidePauseScreen(){
		Time.timeScale = 1f;
		UiPause.SetActive(false);
		srcBase.curGameState = GameState.GamePlay;
	}
	public void PlayAudio(int id, bool effect){
		if(AudioManager.instance!=null){
			if (effect) {
				AudioManager.instance._audioBrushEffect.Stop ();
				AudioManager.instance._audioBrushEffect.clip = AudioManager.instance.clips [id];
				AudioManager.instance._audioBrushEffect.Play ();
			} else {
				AudioManager.instance._audio.Stop ();
				AudioManager.instance._audio.clip = AudioManager.instance.clips [id];
				AudioManager.instance._audio.Play ();
			}
		}
	}
	public void StopAudio(bool effect){
		if(AudioManager.instance!=null){
			if(effect){
				AudioManager.instance._audioBrushEffect.Stop ();
			}else{
				AudioManager.instance._audio.Stop ();
			}
		}
	}
}


