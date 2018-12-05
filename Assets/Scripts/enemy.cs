using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemy : MonoBehaviour {
	[SerializeField]
	private float speed=0f;
	private float maxSpeed = 10f;
	private float minChanceHardMode = 5f;
	private float maxChanceHardMode = 20f;
	public Animator anim;
	public EnemyState curstate;
	public GameObject sparks;
	public GameObject pointsUP;
	public bool friend = false;
	private Rigidbody2D enemyBody;
	private Collider2D boxColl;
	private SpriteRenderer sprite;
	private bool hardMode = false;
	private bool poofPlayed = false;
	private bool scoreAdded = false;

	void Start () {
		sprite = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
		enemyBody = gameObject.GetComponent<Rigidbody2D>() as Rigidbody2D;
		anim = gameObject.GetComponent<Animator>() as Animator;
		if (Random.Range (minChanceHardMode, maxChanceHardMode) > 13) {
			hardMode = true;
		} else {
			hardMode = false;
		}
		if (hardMode) {
			speed = Random.Range (0.1f + (Time.time - srcBase.startTime) * 0.005f, 1f + (Time.time - srcBase.startTime) * 0.01f);
			if (speed > maxSpeed)
				speed = maxSpeed;
		} else {
			speed = Random.Range (0.1f , 2f);
		}

		curstate = EnemyState.Perseguindo;
		boxColl = gameObject.GetComponent<Collider2D>() as Collider2D;
	}

	void FixedUpdate () {
		// Se for inimigo ele se movimenta assim.
		if(!friend &&!srcBase.dead &&curstate != EnemyState.Morto && srcBase.Player!=null){
			transform.localPosition = Vector3.MoveTowards(new Vector3(transform.localPosition.x,transform.localPosition.y,0f),new Vector3(srcBase.Player.transform.localPosition.x,srcBase.Player.transform.localPosition.y,0f),speed*Time.deltaTime);
		}
	/*	// Se for Amigo ele se movimenta assim.
		if(friend && curstate != EnemyState.Morto && srcBase.Player!=null){
			transform.localPosition = Vector3.MoveTowards(new Vector3(transform.localPosition.x,-4.15f,0f),new Vector3(srcBase.Player.transform.localPosition.x,-4.15f,0f),speed*Time.deltaTime);
		}
*/
	}
	void Update(){
		//caiu no buraco
		if (this.gameObject.transform.position.y <= -5f) {
			curstate = EnemyState.Morto;
			anim.SetBool("morto",true);
			Destroy(this.gameObject,5f);
		}

		/*
		if(friend && srcBase.curPlayerState != EnemyState.Morto && Vector3.Distance(this.gameObject.transform.position,srcBase.Player.transform.position)<1f){
			sprite.enabled = false;
			GameObject uiPoints = Instantiate(pointsUP,this.gameObject.transform.localPosition,Quaternion.identity) as GameObject;
			uiPoints.transform.SetParent(this.gameObject.transform);
			Destroy(this.gameObject,3f);
			srcBase.AddScorePoints();
		}
		*/
		// Caiu no Chão, Inicia a transição para Animal Amigo.
		/*if(friend && this.gameObject.transform.position.y<-3.7f){
			StartCoroutine("EnemyFriendTransition");

		}*/
		// troca o sprite de direcao
		sprite.flipX  = gameObject.transform.position.x > srcBase.Player.gameObject.transform.position.x? true: false; 
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "bullet" && curstate!= EnemyState.Morto && !friend){
			// instancia a particula e coloca como filha do inimigo.
			GameObject particula = Instantiate(sparks,this.gameObject.transform.localPosition,Quaternion.identity) as GameObject;
			particula.transform.SetParent(this.gameObject.transform);

			// Faz a Morte do inimigo.
			anim.SetBool("morto",true);
			curstate = EnemyState.Morto;
			enemyBody.angularDrag = 0.5f;
			enemyBody.gravityScale = 0.2f;
			CountScore ();
			//	StartCoroutine("EnemyFriendTransition");
		}
	}/*
	void OnTriggerEnter2D(Collider2D other){
		// coletou Score de animal transformado.
		if(other.gameObject.tag == "Player" && friend){
			CountScore ();
		}
	}*/

	private void CountScore(){
		sprite.enabled=true;
		Destroy(this.gameObject,4f);
		// Adiciona os Pontos
		if(srcBase.curPlayerState!= EnemyState.Morto){			
			scoreAdded = true;
			Interface.instance.PlayAudio(6, true); 
			GameObject uiPoints = Instantiate(pointsUP,new Vector3(this.gameObject.transform.localPosition.x,this.gameObject.transform.localPosition.y+Random.Range(0.5f,2.5f)),Quaternion.identity) as GameObject;
			uiPoints.transform.SetParent(this.gameObject.transform);
			// Adiciona o valor do ponto na caixa de texto que sobe.
			uiPoints.GetComponentInChildren<Text>().text = srcBase.AddScorePoints(3);
		}
	}

	/*
	IEnumerator EnemyFriendTransition(){

		// Inimigo Vira animal Amigo.
		anim.SetBool("friend",true);
		this.gameObject.tag = "Friend";
		//friend = true;
		if (!poofPlayed) {
			Interface.instance.PlayAudio (7, true); 
			poofPlayed = true;
			//Debug.Log ("this.boxColl.bounds: " + this.boxColl.bounds + "srcBase.Player.transform.position");
		}

		this.gameObject.transform.localRotation = Quaternion.identity; 
		enemyBody.freezeRotation = true;
		enemyBody.constraints = RigidbodyConstraints2D.FreezePositionY;
		gameObject.transform.position = new Vector3(gameObject.transform.position.x,-4.15f);
		yield return new WaitForSeconds(2f);
		boxColl.isTrigger = true;
		if (!scoreAdded && this.boxColl.bounds.Intersects (srcBase.Player.GetComponent<BoxCollider2D>().bounds)) {
			CountScore ();
		}
		speed = 5f;
		curstate = EnemyState.Perseguindo;
	}*/
}