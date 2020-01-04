using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour {

	public int maxDrops;
	public int displacement = 5;
	public float weaponChance = 0.5f;

	private HealthScript health;
	private Animator animator;

	// Use this for initialization
	void Start () {
		health = GetComponent<HealthScript> ();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		animator.SetBool ("lowHealth", health.health < health.MAX_HEALTH / 2);
	}

	void crateBreak(){
		//drop items
		int numDrops = Random.Range(0, maxDrops + 1);
		GameObject[] drops = DropListScript.instance.drops;
		for (int i = 0; i < numDrops; i++) {
			int itemID = Random.Range (0, drops.Length);
			GameObject drop = Instantiate (drops [itemID], transform.position, transform.rotation);

			float x = Random.Range (-displacement, displacement);
			float y = Random.Range (-displacement, displacement);
			Vector2 displace = new Vector2 (x, y);
			drop.GetComponent<Rigidbody2D>().AddForce(displace, ForceMode2D.Impulse);
		}

		//drop weapons
		if(Random.value < weaponChance){  //if it should spawn a weapon
			List<Weapon> weapons = DropListScript.instance.weapons;
			int bestWeapon = Mathf.Clamp (WaveManagerScript.instance.wave + 3, 3, weapons.Count);

			int weaponID = Random.Range (0, bestWeapon);



			GameObject drop = Instantiate (DropListScript.instance.weaponDropPrefab, transform.position, transform.rotation);
			drop.GetComponent<WeaponPickupScript> ().weaponInfo = weapons [weaponID];

			float x = Random.Range (-displacement, displacement);
			float y = Random.Range (-displacement, displacement);
			Vector2 displace = new Vector2 (x, y);
			drop.GetComponent<Rigidbody2D> ().AddForce (displace, ForceMode2D.Impulse);

			DropListScript.instance.weapons.RemoveAt (weaponID);

			
		}


		disable ();
	}

	void disable(){
		GetComponent<BoxCollider2D> ().enabled = false;
		StartCoroutine (enable ());
	}

	IEnumerator enable(){
		yield return new WaitForSeconds (300f);

		health.health = health.MAX_HEALTH;
		health.isDead = false;

		GetComponent<BoxCollider2D> ().enabled = true;
		animator.SetTrigger ("reappear");
	}
}
