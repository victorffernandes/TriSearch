using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

	public Text inputField;
	public ScoreController dbManager;

	void Start() {
		dbManager = GameObject.Find ("DatabaseManager").GetComponent<ScoreController> ();
	}
	
	public void Submit() {
		if (inputField.text.Split ('|') [0] == "godmode") {
			dbManager.godmode = 1;
			dbManager.name = inputField.text.Split ('|') [1];
			StartCoroutine (dbManager.SaveScores ());
			dbManager.isUpdating = true;
		} else {
			//if (PlayerPrefs.GetInt ("actualPoints") != 0) {
				dbManager.name = inputField.text;
				dbManager.score = PlayerPrefs.GetInt ("actualPoints");
				dbManager.godmode = 0;
				StartCoroutine (dbManager.SaveScores ());
				dbManager.isUpdating = true;
			//}
		}
	}
}
