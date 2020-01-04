using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class InventoryScript : MonoBehaviour {


	public GameObject player;

	public int[] ammos;
	public int money = 0;

	public const int lightAmmoID = 1;
	public const int heavyAmmoID = 2;
	public const int shotgunAmmoID = 3;
	public const int explosiveAmmoID = 4;

	public int[] maxAmmos;

	public GameObject ammoBG;
	public GameObject reloadButton;
	public GameObject interactButton;
	public Image ammoBar;
	public Text ammoText;
	public Text coinCount;


	public static InventoryScript instance;

	void Awake(){
		if (instance != null) {
			Debug.Log ("More than one Inventory");
			return;
		}
		instance = this;
	}



	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("PickUpTag")) {
			
			pickItUp (other.gameObject.GetComponent<PickupScript> ());
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.CompareTag ("WeaponPickUpTag")) {
			interactButton.SetActive (true);

			if (CrossPlatformInputManager.GetButtonUp ("Interact")) {
				player.GetComponent<PlayerControllerScript> ().weaponPickup (other.GetComponent<WeaponPickupScript> ().weaponInfo);
				other.enabled = false;
				other.GetComponent<Animator> ().SetTrigger ("PickedUp");
				Destroy (other.gameObject);
			}
		} else if (other.CompareTag ("WeaponShopTag")) {
			WeaponShop shop = other.GetComponent<WeaponShop> ();
			if (shop.weaponInfo != null) {
				other.GetComponent<Animator> ().SetBool ("playerNear", true);
				interactButton.SetActive (true);
				if (CrossPlatformInputManager.GetButtonUp ("Interact")) {
					if (shop.weaponInfo.value <= money) {
						addMoney (-shop.weaponInfo.value);
						shop.buyGun ();
					} else {
						WaveManagerScript.instance.updateChat ("Not Enough Money!");
					}
				}
			}
		} else if (other.CompareTag ("ShopTag")) {
			//ammo shop
			Shop shop = other.GetComponent<Shop>();
			other.GetComponent<Animator> ().SetBool ("playerNear", true);
			interactButton.SetActive (true);
			if (CrossPlatformInputManager.GetButtonUp ("Interact")) {
				if (shop.cost <= money) {
					addMoney (-shop.cost);
					shop.buyGood ();
				} else {
					WaveManagerScript.instance.updateChat ("Not Enough Money!");
				}
			}
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("WeaponPickUpTag")) {
			//interactButton.GetComponent<Button>().Reset();
			//CrossPlatformInputManager.GetButton ("Interact").SetState();
			interactButton.SetActive (false);

		} else if (other.CompareTag ("WeaponShopTag")) {
			other.GetComponent<Animator> ().SetBool ("playerNear", false);
			interactButton.SetActive (false);
		} else if (other.CompareTag ("ShopTag")) {
			//ammo shop
			other.GetComponent<Animator> ().SetBool ("playerNear", false);
			interactButton.SetActive (false);
		}
	}

	void pickItUp(PickupScript item){
		
		switch (item.type) {
		case 0:   //health
			fillHealth(item);
			break;
		default:     
			fillAmmo(item, item.type);
			break;

		}
	}


	void fillHealth(PickupScript item){
		if (player.GetComponent<HealthScript> ().health < player.GetComponent<HealthScript> ().MAX_HEALTH)
		{
			item.GetComponent<CircleCollider2D> ().enabled = false;
			item.GetComponent<Animator> ().SetTrigger ("PickedUp");
			player.GetComponent<HealthScript>().sounds.PlayOneShot(item.pickUpSound);

			player.GetComponent<HealthScript> ().takeDamage(-item.amount);
			Destroy (item.gameObject);
		}
	}

	void fillAmmo(PickupScript item, int ammoID){
		if(ammos[ammoID] < maxAmmos[ammoID])
		{
			item.GetComponent<CircleCollider2D> ().enabled = false;
			item.GetComponent<Animator> ().SetTrigger ("PickedUp");
			player.GetComponent<HealthScript>().sounds.PlayOneShot(item.pickUpSound);

			ammos[ammoID] += item.amount;
			if (ammos[ammoID] > maxAmmos[ammoID]) {
				ammos[ammoID] = maxAmmos[ammoID];
			}

			if (player.GetComponent<PlayerControllerScript> ().currentWeapon != null) {
				Weapon w = player.GetComponent<PlayerControllerScript> ().currentWeapon;
				if (w.isGun) {
					GameObject g = player.GetComponent<PlayerControllerScript> ().gunTemplate;
					g.GetComponent<GunScript> ().updateAmmoUI ();
				}
			}
			Destroy (item.gameObject);
		}
	}


	public void addMoney(int amount){
		money += amount;
		coinCount.text = money.ToString();
	}
}
