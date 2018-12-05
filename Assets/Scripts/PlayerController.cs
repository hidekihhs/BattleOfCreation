using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public static PlayerController instance = null;
	public float speed = 4.0f;
	public Rigidbody2D bodyPlayer;
	private Renderer _render;
	private int jumpSpeed;
	private int maxSpeedBullet = 500;
	public float gravity = 20.0F;
	private float maxSpeed = 4f;
	private float distToGround;
	private Vector3 moveDirection = Vector3.zero;
	public Rigidbody2D bullet;
	private bool isGrounded;
	public Facing facing;
	public Facing latestFacing;
	private bool turn = true;
	private int x, y;

	public Animator newAnim;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	public enum Facing {Right, UpRight, Up, UpLeft, Left, None};

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
		srcBase.Player = this.gameObject;
		srcBase.startTime = Time.time;
		srcBase.curPlayerState = EnemyState.Perseguindo;
		bodyPlayer = GetComponent<Rigidbody2D> ();
		_render = gameObject.GetComponent<Renderer>();
		newAnim = gameObject.GetComponent<Animator> ();
		isGrounded = true;
		jumpSpeed = 1500;
	}
	void FixedUpdate(){
		
		if (!srcBase.dead) {
			Vector3 directionFacing = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0.0f);
			facing = FindFacing (directionFacing);
			determineXY (facing);
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);

			moveDirection *= speed;
		}
	}
		
	void Jump() {

		bodyPlayer.AddForce (Vector3.up * jumpSpeed);
	}
	void Update(){
		Debug.Log (facing);
		if (!srcBase.dead) {

			if (Input.GetKey (KeyCode.LeftShift) && isGrounded) {
				Jump ();
				newAnim.SetTrigger ("Jump");
			}

			if (Input.GetKey (KeyCode.S)) {
				newAnim.SetBool ("Down", true);
			} else {
				newAnim.SetBool ("Down", false);
			}
			if (Input.GetKey (KeyCode.W)) {
				newAnim.SetBool ("lookUp", true);
			} else {
				newAnim.SetBool ("lookUp", false);
			}
			if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) {
				newAnim.SetBool ("upLR", true);
			} else {
				newAnim.SetBool ("upLR", false);
			}

			if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)  ) {
				newAnim.SetBool ("lado", true);
			} else {
				newAnim.SetBool ("lado", false);
			}
	
			if (bodyPlayer.velocity.magnitude > maxSpeed)
				bodyPlayer.velocity = bodyPlayer.velocity.normalized * maxSpeed;
			else
				bodyPlayer.AddForce (moveDirection * speed);
			
			newAnim.SetFloat ("Speed", bodyPlayer.velocity.magnitude);

			if (Input.GetKeyDown (KeyCode.Space))
				Fire ();

			if (this.gameObject.transform.position.y <= -10f || this.gameObject.transform.position.x <= -12f || this.gameObject.transform.position.x >= 11.9f) {
				if (!srcBase.dead) {
					newAnim.SetBool ("Dead", true);
					srcBase.curPlayerState = EnemyState.Morto;
					srcBase.GameOver ();
					this.gameObject.SetActive (false);
				}
			}
		}
	}

	private void determineXY(Facing facing){
		switch (facing) {
		case Facing.Left:
			//sprite indo pra esquerda
			x = -1;
			y = 0;
			if (turn) {
				transform.Rotate (0, 180, 0);
				turn = false;
				latestFacing = Facing.Left;
			}
			newAnim.SetBool ("UpAngle",false);
			newAnim.SetBool ("Up",false);

			break;

		case Facing.UpLeft:
			//sprite olhando pra cima pra esquerda
			x = -1;
			y = 1;
			latestFacing = Facing.UpLeft;
			newAnim.SetBool ("UpAngle",true);
			break;

		case Facing.Right:
			x = 1;
			y = 0;
			latestFacing = Facing.Right;
			if (!turn) {
				transform.Rotate (0, 180, 0);
				turn = true;
			}
			newAnim.SetBool ("UpAngle",false);
			newAnim.SetBool ("Up",false);
			break;

		case Facing.UpRight:
			//sprite olhando pra cima pra direita
			x = 1;
			y = 1;
			latestFacing = Facing.UpRight;
			newAnim.SetBool ("UpAngle",true);
			break;

		case Facing.Up:
			//sprite olhando pra cima
			x = 0;
			y = 1;
			newAnim.SetBool ("Up",true);
			newAnim.SetBool ("UpAngle",false);
			break;
		default:
			break;
		}
	}

	void Fire()
	{
		//AudioManager.instance._audio.Stop ();
		//AudioManager.instance._audio.clip = AudioManager.instance.clips [2];
		//AudioManager.instance._audio.Play ();
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		if (x == 0 && y == 0)
			determineXY (latestFacing);			

		bullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(x,y,0) * maxSpeedBullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);        
	}

	Facing FindFacing(Vector3 direction) {
		if (direction == Vector3.zero)
			return Facing.None;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		//Debug.Log ("Angle: "+angle);
		if (angle < 0.0f)
			angle = 360.0f + angle;

		angle += 22.5f;

		int i = (int)(angle / 45.0f);
		i = i % 8;

		if (i > 4)
			return Facing.None;

		return (Facing)i;
	}
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Enemy") {
			if (!srcBase.dead) {
				newAnim.SetBool ("Dead", true);
				bodyPlayer.constraints = RigidbodyConstraints2D.None;
				srcBase.curPlayerState = EnemyState.Morto;
				srcBase.GameOver ();
			} 
		}

	}
	void OnCollisionStay2D(Collision2D coll){
		//ground id = 8
		if (coll.gameObject.layer == 8) {
			isGrounded = true;
		}
	}
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.layer == 8) {
			isGrounded = false;
		}
	}
}