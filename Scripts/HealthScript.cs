using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

	public int MAX_HEALTH = 100;
	public float health = 1;
	public bool isDead = false;

	public ParticleSystem hurtSplatter;
	public AudioClip hurtSound;
	public AudioClip deathSound;

	public AudioSource sounds;
	private Animator animator;
	public Image healthBar;
	public Text healthText;

	// Use this for initialization
	void Start () {
		health = MAX_HEALTH;
		animator = GetComponent<Animator> ();
		sounds = GetComponent<AudioSource> ();

		//initialize health text
		updateHealthUI ();
	}
	


	public void takeDamage(float damage){
		if (!isDead) {
			health -= damage;
			if (damage > 0) {
				//Instantiate (hurtSplatter, transform.position, transform.rotation);
				hurtSplatter.Play();
				//play hurt sound
				sounds.PlayOneShot(hurtSound);
				animator.SetTrigger("getHit");
			}

			if (health <= 0f) {
				die ();
			}else if(health > MAX_HEALTH){
				health = MAX_HEALTH;
			}
			updateHealthUI ();
		}
	}

	void die(){
		isDead = true;
		health = 0;
		updateHealthUI ();
		animator.SetTrigger ("die");
		sounds.PlayOneShot (deathSound);
		//print ("I died");
		//gameObject.SendMessage ("die");  //this crashes game!!!!
	}

	void updateHealthUI(){
		if (healthBar && healthText) {
			healthBar.fillAmount = health / MAX_HEALTH;
			healthText.text = Mathf.Round(health) + "/" + MAX_HEALTH;
		}
	}


}
