using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleScript : MonoBehaviour {


	public int dayLength = 300;
	public float noonIntensity = 0.8f;
	public float nightTimeIntensity = 0.0f;
	private float A = 0f;
	private Light star;
	private float time = 0f;


	// Use this for initialization
	void Start () {
		star = GetComponent<Light> ();
		time = 0;
		star.intensity = nightTimeIntensity;

		A = (noonIntensity - nightTimeIntensity)/2f;

	}
	
	// Update is called once per frame
	void Update () {
		

		star.intensity = A * Mathf.Sin (2*Mathf.PI / dayLength * time) + A;

		time += Time.deltaTime;
		if (time > dayLength) {
			time = 0f;
		}


	}
}
