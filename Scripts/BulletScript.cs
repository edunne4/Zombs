using UnityEngine;

public class BulletScript : MonoBehaviour {

	[Header("Explosion Stuff")]
	public bool explodes = false;
	public GameObject explosion;
	public float damage = 0f;
	public float speed = 0f;
	public float knockbackForce = 30f;

	private Vector2 player;
	private float distance = 0f;
	private Rigidbody2D body;
	public ParticleSystem shatterEffect;

	//private CameraShakeScript cameraShake;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		body.AddRelativeForce (speed * Vector2.up, ForceMode2D.Impulse);

		player = GameObject.FindWithTag("PlayerTag").transform.position;
		//cameraShake = Camera.main.GetComponent<CameraShakeScript> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//despawn if bullet gets too far
		distance = Vector2.Distance(Camera.main.WorldToScreenPoint(transform.position),
			Camera.main.WorldToScreenPoint(player));
		if(distance > Screen.width){
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("EnemyTag") || other.CompareTag ("PlayerTag")) {
			other.GetComponent<HealthScript> ().takeDamage (damage);
			knockback (other.GetComponent<Rigidbody2D>(), GetComponent<Rigidbody2D>().velocity);
		} else if (other.CompareTag ("CrateTag")){
			other.GetComponent<HealthScript> ().takeDamage (damage);
		}else {
			shatter ();
		}
		if (explodes) {
			explode ();
		}
		Destroy (gameObject);
	}

	void shatter(){
		//print (shatterEffect.isPlaying);
		//shatterEffect.Play ();
		Instantiate(shatterEffect, transform.position, transform.rotation);
		//print (shatterEffect.main.startColor);
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().enabled = false;
		Destroy (gameObject, 1f);

	}

	void explode(){
		GameObject explo = Instantiate (explosion, transform.position, transform.rotation);
		explo.GetComponent<explosionScript> ().maxDamage = damage;
		//StartCoroutine(cameraShake.shake(0.15f, 0.4f)); 

	}

	void knockback (Rigidbody2D foe, Vector2 vel){
		Vector2 push = vel.normalized * knockbackForce;
		foe.AddForce (push, ForceMode2D.Impulse);
	}
}
