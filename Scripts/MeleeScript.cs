using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour {

//	public float baseDamage = 1f;
//	public float swingSpeed = 1f;  //make bigger to shooter faster
//	public float knockback = 1f;

	public Melee meleeInfo;
	public Transform image;
	public GameObject user;
	public Light glow;

	public bool attacking = false;

	private AudioSource sounds;
	private Animator animator;
	private float timer = 0f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		setUpMelee ();
		sounds = user.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 250f) {   //in case timer gets too high
			timer = 1f / meleeInfo.swingSpeed + 1f;
		}

		if (attacking && timer > 1f / meleeInfo.swingSpeed) {
			swing ();
		}
	}



	void swing(){
		animator.SetTrigger ("swing");
		timer = 0;
	}

	public void setUpMelee(){
		image.GetComponent<SpriteRenderer> ().sprite = meleeInfo.image;
		image.GetComponent<ColliderScript> ().meleeWeapon = meleeInfo;
		glow.color = meleeInfo.glowColor;
		glow.enabled = meleeInfo.hasLight;
	}

	void makeNoise(){
		sounds.PlayOneShot (meleeInfo.swingSound);
	}






}
