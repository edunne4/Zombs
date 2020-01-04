using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

	public int num = 1;

	public GameObject[] enemyTypes;

	public int spacialBuffer = 1;
	public float timeBuffer = 1f;



	public IEnumerator spawnWave(int numSpawns){
		 

		for (int i = 0; i < numSpawns; i++) {
			float x = Random.Range (transform.position.x - spacialBuffer, transform.position.x + spacialBuffer);
			float y = Random.Range (transform.position.y - spacialBuffer, transform.position.y + spacialBuffer);
			Vector2 spawnPos = new Vector2 (x, y);

			int enemyID = Random.Range (0, enemyTypes.Length);

			Instantiate(enemyTypes[enemyID], spawnPos, transform.rotation);

			yield return new WaitForSeconds (timeBuffer);
		}
	}


}
