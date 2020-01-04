using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour {

	public Weapon weaponInfo;
	//public Image image;
	public SpriteRenderer itemPreview;
	public Text price;
	public GameObject weaponDropPrefab;
	public int startWeapons = 0;

	// Use this for initialization
	void Start () {

		updateSale ();
	}

	public void buyGun(){ //drop gun in front, then update sale
		WeaponPickupScript soldWeapon = Instantiate (weaponDropPrefab, transform.position,
			                               Quaternion.identity).GetComponent<WeaponPickupScript> ();
		soldWeapon.weaponInfo = weaponInfo;
		soldWeapon.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * 10f, ForceMode2D.Impulse);

		DropListScript.instance.weapons.Remove (weaponInfo);

		itemPreview.enabled = false;
		weaponInfo = null;
	}

	public void updateSale(){ //get new gun and put it up for sale
		List<Weapon> weapons = DropListScript.instance.weapons;
		int bestWeapon = Mathf.Clamp (WaveManagerScript.instance.wave + startWeapons, startWeapons, weapons.Count);
		//print (bestWeapon);

		int weaponID = Random.Range (0, bestWeapon);

		weaponInfo = DropListScript.instance.weapons[weaponID];
		itemPreview.sprite = weaponInfo.image;
		itemPreview.enabled = true;
		//image.sprite = weaponInfo.image;
		price.text = "$" + weaponInfo.value;
	}
	

}
