using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropListScript : MonoBehaviour {

	#region Singleton
	public static DropListScript instance;
	void Awake(){
		if (instance != null) {
			Debug.Log ("more than one drops list");
			return;
		}
		instance = this;
	}
	#endregion



	public  GameObject[] drops;
	public List <Weapon> weapons = new List<Weapon>();
	public GameObject weaponDropPrefab;
	


}
