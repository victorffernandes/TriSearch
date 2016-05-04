using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ButtonController : MonoBehaviour {

	public Text inputField;
	public bool canSend = true;
	public ScoreController dbManager;

	void Start() {
		dbManager = GameObject.Find ("DatabaseManager").GetComponent<ScoreController> ();
	}
	
	public void Submit() {
	if(canSend){
			Match match = Regex.Match(inputField.text, @"^[a-zA-Z0-9]*$", RegexOptions.IgnoreCase);
			if (match.Success) {
				dbManager.name = inputField.text;
				dbManager.score = PlayerPrefs.GetInt ("actualPoints");
				StartCoroutine (dbManager.SaveScores ());
				dbManager.isUpdating = true;
				canSend = false;
			}
		}
	}
}
