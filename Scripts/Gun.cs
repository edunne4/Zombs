using UnityEngine;


[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun")]
public class Gun : Weapon {

	public int gunType = 1;
	//public float baseDamage = 1f;
	public int clipAmmo = 0;
	public float bulletSpeed = 10f;
	public float fireRate = 3f;  //make bigger to shooter faster
	public int clipSize = 1;
	public float reloadTime = 2f;
	public float power = 0.0f;  //how much the camera shakes when fired
	public int numBullets = 1;
	public float spread = 0f;
	public bool explosive = false;
	public int accuracy = 100;

	public GameObject bulletPrefab;
	//public ParticleSystem flarePrefab;
	public Color flareColor;
	public Vector2 firePoint = Vector2.zero;
	public Vector2 handPos = Vector2.zero;
	public Vector2 pos = Vector2.zero;
	public AudioClip shootSound;
	public AudioClip reloadSound1;
	public AudioClip reloadSound2;
	//public Sprite gunImage;

	public bool reloading = false;

}
