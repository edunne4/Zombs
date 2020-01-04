using UnityEngine;

public class ColliderScript : MonoBehaviour {

	public Melee meleeWeapon;

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("EnemyTag") || other.CompareTag ("PlayerTag")) {
			other.GetComponent<HealthScript> ().takeDamage (meleeWeapon.baseDamage);
			knockback (other.GetComponent<Rigidbody2D>());
		}else if(other.CompareTag ("CrateTag")){
			other.GetComponent<HealthScript> ().takeDamage (meleeWeapon.baseDamage);
		}
	}

	void knockback (Rigidbody2D foe){
		Vector2 push = (foe.transform.position - transform.position).normalized * meleeWeapon.knockback;
		foe.AddForce (push, ForceMode2D.Impulse);
	}
}
