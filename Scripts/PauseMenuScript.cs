using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {


	public void quitToMain ()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Start Menu");
	}

	public void pauseGame()
	{
		Time.timeScale = 0f;
	}

	public void resumeGame()
	{
		Time.timeScale = 1f;
	}
}
