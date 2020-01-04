using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFinishedParticleScript : MonoBehaviour {


	private ParticleSystem system;


	// Use this for initialization
	void Start () {
		system = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (system.isPlaying){
			return;
		}
		Destroy (gameObject);
	}
}
