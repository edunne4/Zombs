using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManagerScript : MonoBehaviour {

	public static WaveManagerScript instance;

	void Awake(){
		if (instance != null) {
			Debug.Log ("More than one wave manager");
			return;
		}
		instance = this;
	}

	private bool waveOn = false;
	public int timeBetweenWaves = 30;
	public int wave = 0;
	public int score = 0;
	private GameObject[] spawners;

	public Text waveText;
	public Text enemyCounter;
	public Text chatText;
	private Animator chatAnimator;

	private float timer = 0f;
	private WeaponShop weaponShop;

	// Use this for initialization
	void Start () {
		spawners = GameObject.FindGameObjectsWithTag ("SpawnerTag");
		//spawnWave ();
		timer = timeBetweenWaves;
		if (GameObject.FindGameObjectWithTag ("WeaponShopTag") != null) {
			weaponShop = GameObject.FindGameObjectWithTag ("WeaponShopTag").GetComponent<WeaponShop> ();
		}

		chatAnimator = chatText.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//print (EnemyScript.enemyCount);
		if (waveOn) {
			if (EnemyScript.enemyCount <= 0) {
				waveOn = false;

			}
		} else {
			timer -= Time.deltaTime;
			enemyCounter.text = "Time Left: " + Mathf.Round (timer);
			if(timer <= 0f){
				spawnWave();
			}else if (timer <= 5f) {
				enemyCounter.color = Color.red;
			}
		}
	}

	void spawnWave(){
		waveOn = true;
		timer = timeBetweenWaves;
		enemyCounter.color = Color.white;
		wave++;
		doWaveText ();
		//print ("Wave: " + wave);
		if (spawners.Length > 0) {
			float numZombs = Random.Range(Mathf.Pow (wave, 3/2), Mathf.Pow (wave, 2));
			//print ("numZombs: " + numZombs);
			int zombsPerSpawner =  Mathf.CeilToInt(numZombs / spawners.Length);
			//print ("zombsPerSpawner: " + zombsPerSpawner);
			foreach (GameObject spawner in spawners) {
				StartCoroutine (spawner.GetComponent<SpawnerScript> ().spawnWave (zombsPerSpawner));
			}

		}

		updateShops ();

	}

	void doWaveText (){
		waveText.text = "Wave: " + wave;
		waveText.GetComponent<Animator> ().SetTrigger ("New Wave");
	}

	public void updateEnemyCounter(){
		enemyCounter.text = "Enemies Left: " + EnemyScript.enemyCount;
	}

	void updateShops(){
		if (weaponShop != null) {
			weaponShop.updateSale ();
		}

	}

	public void updateChat(string newText){
		chatText.text = newText;
		chatAnimator.SetTrigger ("rollText");
	}

}
