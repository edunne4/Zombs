using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapon/Melee")]
public class Melee : Weapon{

	public float swingSpeed = 1f;  //make bigger to shooter faster
	public float knockback = 1f;
	public AudioClip swingSound;
	public bool hasLight = false;
	public Color glowColor;

}
