using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour {


	public int type = 0;   //health = 0,   light ammo = 1,   heavy ammo = 2,  shotgun ammo = 3,  explosive ammo = 4
	public int amount = 0;
	public AudioClip pickUpSound;

	void Start(){
		Destroy (gameObject, 300f);
	}

}
