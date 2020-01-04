using UnityEngine;

public class Weapon : ScriptableObject {

	public new string name = "";
	public float baseDamage = 0f;
	public Sprite image;
	public AudioClip pickUpSound;
	public bool isGun = false;
	public int value = 0;

}
