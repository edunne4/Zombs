using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour {

	public int speed = 20;
	public float attackDamage = 4f;
	public float attackRate = 1f;
	private float attackTimer = 0f;
	private bool isAttacking = false;
	public int reward = 0;

	private Transform target;
	private Rigidbody2D rb;
	private Animator animator;
	public Canvas healthInfo;
	public GameObject poof;

	// Use this for initialization
	void Start () {
		EnemyScript.enemyCount++;    //increase total static number of enemies

		target = GameObject.FindGameObjectWithTag ("PlayerTag").transform;
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();


		Instantiate (poof, transform.position, transform.rotation);

		WaveManagerScript.instance.updateEnemyCounter ();    //update enemy counter in UI
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		animator.SetFloat ("speed", rb.velocity.magnitude);

		//check if still alive
		if (GetComponent<HealthScript> ().isDead) {
			GetComponent<CircleCollider2D> ().enabled = false;
			return;
		}

		//attacking
		if (attackTimer > 250f) {   //in case timer gets too high
			attackTimer = 1f / attackRate + 1f;
		}

		attackTimer += Time.deltaTime;
		if(isAttacking && (attackTimer > 1/attackRate)){
			attack ();
		}


		//pathfinding
		if (target) {
			Vector2 dir = target.position - transform.position;
			dir.Normalize ();
			rb.AddForce (dir * speed);
			Vector3 theScale = transform.localScale;
			if (target.position.x > transform.position.x) {
				theScale.x = -1f;
			} else {
				theScale.x = 1f;
			}
			transform.localScale = theScale;
			healthInfo.transform.localScale = theScale/100f;
		}
	}


	//attack
	void OnCollisionEnter2D(Collision2D hit){
		if(hit.gameObject.CompareTag("PlayerTag") || hit.gameObject.CompareTag("FriendlyTag")){
			//if attack timer long enough
			//print ("attack");
			isAttacking = true;
		}
	}

	void OnCollisionExit2D(Collision2D hit){
		if(hit.gameObject.CompareTag("PlayerTag") || hit.gameObject.CompareTag("FriendlyTag")){
			//if attack timer long enough
			//print ("attack");
			isAttacking = false;
		}
	}

	void attack(){
		attackTimer = 0f;
		animator.SetTrigger ("attack");

	}

	void sendDamage (){
		if (isAttacking) {
			if (target.CompareTag ("PlayerTag")) {
				target.GetComponent<HealthScript> ().takeDamage (attackDamage);
			}
		}
	}

	void OnDeath (){
		EnemyScript.enemyCount--;
		EnemyScript.killed++;
		InventoryScript.instance.addMoney (reward);
		WaveManagerScript.instance.updateEnemyCounter ();

		StartCoroutine (disappear ());
	}

	IEnumerator disappear(){
		yield return new WaitForSeconds (2f);

		Instantiate (poof, transform.position, transform.rotation);

		Destroy (gameObject);
	}

//	void OnDestroy(){
//		EnemyScript.enemyCount--;
//	}
}
