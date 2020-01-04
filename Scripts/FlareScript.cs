using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareScript : MonoBehaviour {

	public Light flash;
	public GameObject flareColor;



	public void setColor(Color newColor){
		flareColor.GetComponent<SpriteRenderer>().color = newColor;
		flash.color = newColor;
	}

}
