using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


public class GunScript : MonoBehaviour {

	
	public Transform firePoint;

	public Gun gunInfo;
	public Transform image;
	public FlareScript muzzleFlare;
	public Transform supportHand;

	public bool attacking = false;

	//private CameraShakeScript cameraShake;
	public InventoryScript currentInventory;

	//private int ammo = 0;

//	private bool reloading = false;

	private AudioSource sounds;
	private Animator animator;
	private float timer = 0f;
	// Use this for initialization
	void Start () {
		sounds = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();
		//cameraShake = Camera.main.GetComponent<CameraShakeScript> ();
		//gunImage.GetComponent<SpriteRenderer> ().sprite = gun.gunImage;
		//ammo = clipSize;

		updateAmmoUI ();
	}

	public void setUpGun (){
		transform.localPosition = gunInfo.pos;
		firePoint.localPosition = gunInfo.firePoint;
		supportHand.localPosition = gunInfo.handPos;
		image.GetComponent<SpriteRenderer> ().sprite = gunInfo.image;
		muzzleFlare.setColor (gunInfo.flareColor);


		updateAmmoUI ();
	}


	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 250f) {   //in case timer gets too high
			timer = 1f / gunInfo.fireRate + 1f;
		}


		if (attacking && timer > 1f / gunInfo.fireRate && !gunInfo.reloading) {
			if (gunInfo.clipAmmo > 0) {
				shoot ();
			} else{
				StartCoroutine ("reloadCo");
			}
		}

	}

	void shoot(){
		//create bullet and flare
		int maxOff = 100 - gunInfo.accuracy;
		Mathf.Clamp (maxOff, 0, 100);
		for (int i = -gunInfo.numBullets/2; i <= gunInfo.numBullets/2; i++) {
			int misfireAngle = Random.Range (-maxOff, maxOff);
			float bulletDir = 270f + i * gunInfo.spread + misfireAngle;
			GameObject bullet = Instantiate (gunInfo.bulletPrefab, firePoint.position, firePoint.rotation *
				Quaternion.Euler (0f, 0f, bulletDir));
			//send bullet with damage and speed
			BulletScript bulletInfo = bullet.GetComponent<BulletScript> ();
			bulletInfo.speed = gunInfo.bulletSpeed;
			bulletInfo.damage = gunInfo.baseDamage;
			bulletInfo.explodes = gunInfo.explosive;

		}
		muzzleFlare.GetComponent<Animator> ().SetTrigger ("flare");
		//muzzleFlare.Play ();
		//Instantiate (gunInfo.flarePrefab, firePoint.position, firePoint.rotation);
		//play sound and animation
		sounds.PlayOneShot (gunInfo.shootSound);
		animator.SetTrigger ("shoot");



		//shake camera
		CameraShaker.Instance.ShakeOnce (gunInfo.power, gunInfo.power, 0.2f, 0.2f);

		gunInfo.clipAmmo--;
		updateAmmoUI ();
		//print (ammo);
		//reset cooldown timer
		timer = 0;
	}

	public IEnumerator reloadCo(){
		if (gunInfo.clipAmmo  < gunInfo.clipSize && !gunInfo.reloading) { //if its not full or already reloading
			if (currentInventory.ammos [gunInfo.gunType] > 0) {   //if theres any ammo left
				gunInfo.reloading = true;
				sounds.PlayOneShot (gunInfo.reloadSound1, 0.5f);
				animator.SetBool ("reloading", gunInfo.reloading);
				yield return new WaitForSeconds (gunInfo.reloadTime);

				if (gunInfo.clipSize > currentInventory.ammos [gunInfo.gunType]) {
					gunInfo.clipAmmo  = currentInventory.ammos [gunInfo.gunType];
					currentInventory.ammos [gunInfo.gunType] = 0;
				} else {
					currentInventory.ammos [gunInfo.gunType] -= (gunInfo.clipSize - gunInfo.clipAmmo );
					gunInfo.clipAmmo  = gunInfo.clipSize;
				}
				sounds.PlayOneShot (gunInfo.reloadSound2, 0.5f);

				updateAmmoUI ();

				gunInfo.reloading = false;
				animator.SetBool ("reloading", gunInfo.reloading);
			} else {   
				//tell player no more ammo!
				string ammoType = "";
				switch (gunInfo.gunType) {
				case 1:
					ammoType = "Light";
					break;
				case 2:
					ammoType = "Heavy";
					break;
				case 3:
					ammoType = "Shotgun";
					break;
				case 4:
					ammoType = "Explosive";
					break;
				}

				WaveManagerScript.instance.updateChat("Out of " + ammoType + " ammo!");
			}
		}
	}

	void OnEnable(){
		//print ("current inventory: " + currentInventory);
		if (currentInventory) {
			currentInventory.ammoBG.SetActive (true);
			currentInventory.reloadButton.SetActive (true);
			updateAmmoUI ();
//			if (gunInfo.reloading) {
//				StartCoroutine ("reloadCo");
//
//			}
		}
	}

	void OnDisable(){
		gunInfo.reloading = false;
		if (currentInventory) {
			currentInventory.ammoBG.SetActive (false);
			currentInventory.reloadButton.SetActive (false);
		}
	}

	public void updateAmmoUI(){
		float a = gunInfo.clipAmmo ;
		float c = gunInfo.clipSize;

		currentInventory.ammoBar.fillAmount = a / c;        //reset ammo UI
		currentInventory.ammoText.text = gunInfo.clipAmmo  + "/" + currentInventory.ammos[gunInfo.gunType];

	}
	

	//names:
	//blue: plasma
	//red: flux
	//purple: void
	//red: thermal
	//green: phase
	//cyan: pulse
	//orange: fusion
	//silver: ion

}
