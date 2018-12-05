using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour {

	private float minDistance = .5f;
	private float maxDistance = 3f;
	private int maxEnemies = 12;
	private float minTime = 5f;
	private float maxTime = 20f;
	public GameObject[] enemyArray;
	private float spawnTime = 10f;
	private bool hardMode = false;


	public void Start(){
		spawnTime = 2f;
		Invoke("SpawnEnemies",spawnTime);
		spawnTime = 4f;

	}
	public void RestartSpawnEnemy(){
		spawnTime = 4f;
		if(!IsInvoking("SpawnEnemies")){
			Invoke("SpawnEnemies",spawnTime);
		}
	}

	public void SpawnEnemies(){

		if(srcBase.curPlayerState!= EnemyState.Morto){
			spawnTime = Random.Range(minTime,maxTime);
			// Escolhe um numero de objetos a serem instanciados dentro dos limites minimo e maximo.
			int minimum = 1;
			int maximum = 2;
			if (spawnTime > 13) {
				hardMode = true;
			} else {
				hardMode = false;
			}
			if (hardMode) {
				maximum += (int)Mathf.Floor ((Time.time - srcBase.startTime) * 0.05f);
			}
						
			int objectCount = Random.Range(minimum, maximum);
			if (objectCount > maxEnemies)
				objectCount = maxEnemies;
			if(srcBase.ENEMYHOLDER == null){
				srcBase.ENEMYHOLDER = new GameObject ("Enemies").transform;
			}
			// Instancia os objetos ate a quantidade escolhida for atingida.
			for (int i = 0; i < objectCount; i++){
				// Escolhe uma posicao aleatoria.
				Vector3 randomPosition = RandomPosition();
				// Escolhe um inimigo aleatorio para ser instanciado.
				GameObject enemyChoice = enemyArray[Random.Range(0, enemyArray.Length)];
				// Instancia o inimigo escolhido na posicao retornada pelo RandomPosition.
				enemyChoice = Instantiate (enemyChoice,new Vector3(randomPosition.x,randomPosition.y+enemyChoice.gameObject.transform.localPosition.y,0), Quaternion.identity) as GameObject;
				enemyChoice.transform.SetParent (srcBase.ENEMYHOLDER);
			}
			Invoke("SpawnEnemies",spawnTime);
		}
	}

	Vector3 RandomPosition(){
		Vector3 randomPosition;

		Vector3 pos = Random.insideUnitSphere * Random.Range(minDistance,maxDistance);
		randomPosition = new Vector3(gameObject.transform.localPosition.x+pos.x,gameObject.transform.localPosition.y,0f);
		return randomPosition;
	}
}