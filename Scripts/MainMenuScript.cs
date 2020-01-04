using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class MainMenuScript : MonoBehaviour {

	private string mapName = "";

	public void assignMap(string selectedMap){
		mapName = selectedMap;
	}

	public void loadMap()
	{
		//print (Application.CanStreamedLevelBeLoaded(mapName));
		if(Application.CanStreamedLevelBeLoaded(mapName)){
			SceneManager.LoadScene (mapName);
		}
	}

	public void playAdForMoney(){
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			loadMap();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}


}
