using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBotScript : MonoBehaviour {

	public float fireRate = 1f;  //make bigger to shooter faster
	private float timer = 0f;
	public float bulletSpeed = 10f;
	private float baseDamage = 2f;

	private List<Transform> closeEnemies = new List<Transform>();
	private Transform currentTarget = null;

	public Transform gun;
	private float gunAngle = 0f;
	public Transform firePoint;
	public GameObject projectilePrefab;
	public GameObject flarePrefab;

	private AudioSource sounds;
	public AudioClip shootSound;

	// Use this for initialization
	void Start () {
		sounds = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;


		Vector3 botScale = transform.localScale;

		if (currentTarget == null) {
			gun.rotation = Quaternion.Euler (0f, 0f, 90f);

		} else {   //if there is a target, aim at him and set the scale again to look at him


			Vector2 aimVec = currentTarget.transform.position - gun.position;
			gunAngle = Mathf.Atan2 (aimVec.y, aimVec.x) * Mathf.Rad2Deg;

			if (aimVec.x < 0f) {    //aiming to the left
				botScale.x = -1f;
						
			} else {  //else if aiming to the right face right
				botScale.x = 1f;
			}
			transform.localScale = botScale; //set the scale

			//gun.rotation = Quaternion.Euler (0f, 0f, gunAngle);

			//set the weapon scale
			Vector3 gunScale = gun.localScale;
			gunScale.x = botScale.x;
			gunScale.y = botScale.x;
			gun.localScale = gunScale;

			RaycastHit2D sight = Physics2D.Raycast (gun.position, aimVec, Mathf.Infinity, 1 << 8);
			if (sight.collider != null) {
				print (sight.collider.gameObject);
				//print (currentTarget);
				if (sight.collider.CompareTag("EnemyTag")) {
					print ("see target");
					gun.rotation = Quaternion.Euler (0f, 0f, gunAngle);
					//shoot
					if (timer > 1f / fireRate) {
						//print ("shoot");
						shoot ();
					}
				}
			}


		}


		//search for target if there are enemies in the circle
		if(closeEnemies.Count > 0){
			findClosestTarget ();
		}
	}


	void shoot (){
		GameObject bullet = Instantiate (projectilePrefab, firePoint.position, firePoint.rotation *
			Quaternion.Euler(0, 0, 270));

		print (bullet);
		Instantiate (flarePrefab, firePoint.position, firePoint.rotation);
		//play sound and animation
		sounds.PlayOneShot (shootSound);

		bullet.GetComponent<BulletScript> ().speed = bulletSpeed;
		bullet.GetComponent<BulletScript> ().damage = baseDamage;

		//reset cooldown timer
		timer = 0;
	}


	void OnTriggerEnter2D(Collider2D coll){
		if (coll.CompareTag ("EnemyTag")) {
			closeEnemies.Add(coll.transform);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.CompareTag ("EnemyTag")) {
			closeEnemies.Remove(coll.transform);
			if (coll.transform == currentTarget) {
				currentTarget = null;
			}
		}
	}

	void findClosestTarget(){
		float closestDist = 12f;
		Transform closestEnemy = null;
		foreach (Transform enemy in closeEnemies) {
			float currentDist = Vector2.Distance (transform.position, enemy.position);

			if (currentDist < closestDist) {
				closestDist = currentDist;
				closestEnemy = enemy;
			}
		}
		currentTarget = closestEnemy;
	}
}
