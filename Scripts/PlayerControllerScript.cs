using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerScript : MonoBehaviour {
	
	public int playerSpeed = 10;
	private Rigidbody2D rb;
	private List<GameObject> closeEnemies = new List<GameObject>();

	public Weapon currentWeapon;
	public Weapon[] weapons;
	private int weaponIndex = 0;
	public GameObject currentTarget = null;


	[Header("References")]
	public InventoryScript inventory;

	private float gunAngle = 180f;

	//hand stuff
	private Transform weaponHolder;
	private GameObject rightHand;
	private GameObject leftHand;
	public GameObject gunTemplate;
	public GameObject meleeTemplate;
	public GameObject weaponDrop;
	public GameOverMenuScript canvasController;

	//animation/sound
	private Animator animator;

	// Use this for initialization
	void Start () {
		//assign some parts
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		rightHand = GameObject.Find ("Pip Sky/Arms/HandRight");
		leftHand = GameObject.Find ("Pip Sky/Arms/HandLeft");
		weaponHolder = transform.Find ("Arms/WeaponHolder");

		unequip ();           //unequip anything
		equip(weapons [weaponIndex]);  //equip current weapon index

	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<HealthScript> ().isDead) {
			return;
		}

		//print (EnemyScript.enemyCount);
		//movement
		Vector2 moveVec = new Vector2 (CrossPlatformInputManager.GetAxis("Horizontal"),
			CrossPlatformInputManager.GetAxis("Vertical"));
		rb.AddForce (moveVec * playerSpeed);


		//gun control
		Vector3 playerScale = transform.localScale;


		if (moveVec != Vector2.zero) {
			animator.SetBool ("isRunning", true);
			gunAngle = Mathf.Atan2 (moveVec.y, moveVec.x) * Mathf.Rad2Deg; //angle for gun to be at

			if (moveVec.x > 0f) {    //if moving right face right
				playerScale.x = -1f;
			} else {  //else if moving left face left
				playerScale.x = 1f;
			}
		} else {
			animator.SetBool ("isRunning", false);
		}
		transform.localScale = playerScale; //set the scale


		#region attacks
		//attack
		bool isAttacking = CrossPlatformInputManager.GetButton ("Attack");
		if (currentWeapon != null) { //if holding a weapon

			//if there's no target, point the gun in the direction of joystick
			if (currentTarget == null) {
				weaponHolder.rotation = Quaternion.Euler (0f, 0f, gunAngle);

			} else {   //if there is a target, aim at him and set the scale again to look at him
				

				Vector2 aimVec = currentTarget.transform.position - weaponHolder.position;
				gunAngle = Mathf.Atan2 (aimVec.y, aimVec.x) * Mathf.Rad2Deg; 

				if (aimVec.x > 0f) {    //aiming to the right
					playerScale.x = -1f;
				} else {  //else if aiming to the left face left
					playerScale.x = 1f;
				}
				transform.localScale = playerScale; //set the scale

				weaponHolder.rotation = Quaternion.Euler (0f, 0f, gunAngle);
			}


			//set the weapon scale
			Vector3 weaponScale = weaponHolder.localScale;
			weaponScale.x = -playerScale.x;
			weaponScale.y = -playerScale.x;
			weaponHolder.localScale = weaponScale;

			//tell weapon to attack
			if (currentWeapon.isGun) {
				gunTemplate.GetComponent<GunScript> ().attacking = isAttacking;
			} else {
				meleeTemplate.GetComponent<MeleeScript> ().attacking = isAttacking;
			}




		} else {//else punch
			if(CrossPlatformInputManager.GetButtonDown ("Attack")){
				animator.SetTrigger("punch");
			}
		} 
			
		#endregion

		//switchweapon button
		if (CrossPlatformInputManager.GetButtonUp ("Switch")) {
			unequip ();
			weaponIndex++;
			if (weaponIndex >= weapons.Length) {
				weaponIndex = 0;
			}
			equip (weapons [weaponIndex]);
		//reload button
		}else if (CrossPlatformInputManager.GetButtonUp ("Reload") && !isAttacking) {
			StartCoroutine (gunTemplate.GetComponent<GunScript> ().reloadCo());
		}

		//search for target if there are enemies in the circle
		if(closeEnemies.Count > 0){
			findClosestTarget ();
		}
	}


	#region weaponChange
	void unequip (){
		//arms.rotation = Quaternion.identity;
		if (currentWeapon != null) {
//			currentWeapon.SetActive (false);
			gunTemplate.SetActive(false);
			meleeTemplate.SetActive(false);
			currentWeapon = null;
		}
		rightHand.SetActive(true);
		leftHand.SetActive (true);

	}

	void equip(Weapon w){
		if (w) {
			rightHand.SetActive(false);
			leftHand.SetActive (false);
			if (w.isGun) {
				GunScript g = gunTemplate.GetComponent<GunScript> ();
				g.currentInventory = inventory;
				g.gunInfo = (Gun)w;
				g.setUpGun ();
				gunTemplate.SetActive (true);
			} else {
				MeleeScript m = meleeTemplate.GetComponent<MeleeScript> ();
				m.meleeInfo = (Melee)w;
				m.setUpMelee ();
				meleeTemplate.SetActive (true);
			}

			currentWeapon = w;


		}
	}

	public void weaponPickup(Weapon newWeapon){
		
			//pick it up
			GetComponent<HealthScript> ().sounds.PlayOneShot (newWeapon.pickUpSound); //play pick up sound on health script's audiosource
			//print ("picked up weapon");
			//switch weapons
			int slotToUse = checkWeaponSlots ();
			if (slotToUse == weaponIndex) {
				if (currentWeapon != null) {
					drop (currentWeapon);
					
				}
			}
			unequip ();
			weaponIndex = slotToUse;
			weapons [weaponIndex] = newWeapon;
			equip (newWeapon);
			//GameObject nw = newWeapon.currentWeapon;

//				nw.transform.parent = arms;        //set gun's parent as arms
//				nw.transform.localPosition = Vector2.zero;
//				weapons [slotToUse] = nw;   //put new weapon in list of weapons
//				equip (weapons [weaponIndex]);
			
			
	}

	int checkWeaponSlots(){
		int openSlot = weaponIndex;
		int i = 0;
		while ( i < weapons.Length) {
			if (weapons [i] == null) {
				openSlot = i;
				break;
			}
			i++;
		}
		return openSlot;
	}

	void drop(Weapon oldWeapon){
		GameObject dropped = Instantiate (weaponDrop, transform.position, Quaternion.identity);
		dropped.GetComponent<WeaponPickupScript> ().weaponInfo = oldWeapon;

	}
	#endregion


	#region enemyStuff
	//finding closest target
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("EnemyTag")) {
			closeEnemies.Add(other.gameObject);
		}
		//findClosestTarget ();
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("EnemyTag")) {
			closeEnemies.Remove(other.gameObject);
			if (other.gameObject == currentTarget) {
				currentTarget = null;
			}
		}
	}
		

	void findClosestTarget(){
		float closestDist = 12f;
		GameObject closestEnemy = null;
		foreach (GameObject enemy in closeEnemies) {
			float currentDist = Vector2.Distance (transform.position, enemy.transform.position);

			if (currentDist < closestDist) {
				closestDist = currentDist;
				closestEnemy = enemy;
			}
		}
		currentTarget = closestEnemy;
	}

	#endregion

	void OnDeath(){
		unequip ();
		GetComponent<CircleCollider2D> ().enabled = false;
		//do death stuff
		canvasController.gameOver();
	}


}
