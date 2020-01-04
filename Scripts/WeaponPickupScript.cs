using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupScript : MonoBehaviour {

	//public AudioClip pickUpSound;
	//public GameObject weapon;
	//public GameObject currentWeapon;
	public Transform image;
	public Weapon weaponInfo;

	void Start (){
		GetComponent<CircleCollider2D> ().enabled = false;
		image.GetComponent<SpriteRenderer> ().sprite = weaponInfo.image;

		StartCoroutine(makeAvailableCo());

		Destroy (gameObject, 300f);
	}

	IEnumerator makeAvailableCo(){
		yield return new WaitForSeconds (1f);
		GetComponent<CircleCollider2D> ().enabled = true;
	}



}
