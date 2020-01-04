using System.Collections;
using UnityEngine;
using EZCameraShake;

public class explosionScript : MonoBehaviour {

	public float maxDamage = 1f;
	private float damage = 0f;

	private float radius = 1f;

	void Start(){
		radius = GetComponent<CircleCollider2D> ().radius;
		StartCoroutine (destroyCo());
		CameraShaker.Instance.ShakeOnce (10f, 10f, 0.2f, 0.2f);
	}

	void OnTriggerEnter2D(Collider2D other){
		HealthScript health = other.GetComponent<HealthScript>();
		if (health != null) {
			print (other.gameObject);
			Vector2 dir = (other.transform.position - transform.position);
			damage = (1f - dir.magnitude / radius) * maxDamage;
			if (damage > 0f) {
				health.takeDamage (damage);
			}
		}
	}

	IEnumerator destroyCo(){
		yield return new WaitForSeconds (.2f);
		GetComponent<CircleCollider2D> ().enabled = false;
	}
		
}
