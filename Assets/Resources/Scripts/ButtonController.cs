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
        if (PlayerPrefs.GetInt("actualPoints") != 0)
        {
            dbManager.name = inputField.text;
            dbManager.score = Random.Range(1, 1000);
            StartCoroutine(dbManager.SaveScores());
            dbManager.isUpdating = true;
        }
	}
}
