using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CamFollow : MonoBehaviour {


	public Transform target;
	public int lookAheadFactor = 2;
	public float dampTime = 0.02f;

	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVec = new Vector3 (CrossPlatformInputManager.GetAxis("Horizontal"),
			CrossPlatformInputManager.GetAxis("Vertical"), 0f);
		Vector3 point = new Vector3 (target.position.x, target.position.y, transform.position.z)
			+ moveVec * lookAheadFactor;
		//Vector3 delta = point - transform.position;

		transform.position = Vector3.SmoothDamp(transform.position, point, ref velocity, dampTime);

	}
}
