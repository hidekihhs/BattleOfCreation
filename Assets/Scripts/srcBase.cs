using UnityEngine;
using System.Collections;
public enum EnemyState{
	Perseguindo,
	Morto
};
public enum GameState {
	Tutorial,
	GamePlay,
	Pause,
	GameOver
};
public class srcBase {
	public static string MATERIAL_PATH = "Materiais/";
	public static GameObject Player;
	public static int score;
	public static int highScore;
	public static float startTime;
	public static EnemyState curPlayerState;
	public static GameState curGameState;
	public static Transform ENEMYHOLDER;
	public static int maxVertexCount = 68;
	public static int curVertexCount;
	public static float volume = 0;
	public static bool dead = false;
	public static bool showTutorial = true;

	public static void GameOver(){
		dead = true;
		if(score>=highScore){
			Interface.instance.UiNewScore.SetActive(true);
			highScore = score;
			// Salva o highscore.
			Save();
		}else{
			if(Input.GetMouseButtonUp(0)){
				Interface.instance.UiNewScore.SetActive(false);
			}
		}
		Time.timeScale = 0.3f;

		curGameState = GameState.GameOver;
	}
	public static string AddScorePoints(int enemyValue){
		int res = enemyValue+CalcTimeScore();
		score+=res;
		string points = res.ToString();
		return points;
	}
	public static int CalcTimeScore(){
		int timeScore = (int) (Time.time - startTime * 1);
		return timeScore;
	}
	public static void Save(){
		PlayerPrefs.SetInt ("highscore", score);
		PlayerPrefs.Save ();
	}
	public static void Load(){
		bool haskey = PlayerPrefs.HasKey("highscore");
		if (haskey) {
			// Carrega o highscore salvo.
			highScore = PlayerPrefs.GetInt ("highscore");
		} else {
			PlayerPrefs.SetInt ("highscore", 0);
		}
	}
}