using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJoyStickScript : MonoBehaviour {

	private Touch touch;

	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				if (touch.position.x < Screen.width / 3) {
					transform.position = touch.position;
				}
			}
		}
	}
}
