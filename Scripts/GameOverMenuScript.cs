using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenuScript : MonoBehaviour {

	public GameObject HUD;
	public GameObject gameOverMenu;
	public Text waveText;
	public Text killCounter;
	public Text scoreText;

	public void restartLevel(){
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	public void gameOver(){
		HUD.SetActive (false);
		gameOverMenu.SetActive (true);
		waveText.text = "Died On Wave " + WaveManagerScript.instance.wave + "!";
		killCounter.text = "Killed " + EnemyScript.killed + " Monsters!";
		scoreText.text = "Score: " + WaveManagerScript.instance.score;
	}

}
