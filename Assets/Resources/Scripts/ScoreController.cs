using UnityEngine;
using System.Net;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	/*
	 * Original = http://trisearch.16mb.com/server/
	 * Test = http://trisearch.avellar.c9.io/server/
	 */

	public Text namesList;
	public Text scoreList;
	public int godmode;
	public string name;
	public int score;
	public string db_url = "http://trisearch.16mb.com/server/";
	public bool isUpdating;
	public bool isUpdatingScore;
	private string[] scoresText;
	public GameObject error;

	void Awake() {
		isUpdating = true;
		isUpdatingScore = true;
		db_url = "http://trisearch.16mb.com/server/";

		error.GetComponent<Animator>().Play("ConnectionStatus_Loading");
	}

	void Update() {
		if (isUpdating) {
			StartCoroutine (LoadScores());
			StopCoroutine(SaveScores());
		} else {
			StopCoroutine(LoadScores());
		}

		if (isUpdatingScore && !isUpdating) {
			if(scoresText.Length > 1) {
				namesList.text = "<color=#B8860B>" + scoresText[0] + "</color> \n";
            	scoreList.text = "<color=#B8860B>" + scoresText[1] + "</color> \n";

				for (int i = 2; i < scoresText.Length; i += 2) {
					namesList.text += scoresText [i] + "\n";
				}
				for (int i = 3; i < scoresText.Length; i += 2) {
					scoreList.text += scoresText [i] + "\n";
				}
				isUpdatingScore = false;
				error.GetComponent<Animator>().Play("ConnectionStatus_Valid");
			} else {
				error.GetComponent<Animator>().Play("ConnectionStatus_Error");
			}
		}
	}

	bool CheckConnection(string URL)
	{
		try
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
			request.Timeout = 5000;
			request.Credentials = CredentialCache.DefaultNetworkCredentials;
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			
			if (response.StatusCode == HttpStatusCode.OK) return true;
			else return false;
		}
		catch
		{
			return false;
		}
	}

	public IEnumerator SaveScores() {
		error.GetComponent<Animator>().Play("ConnectionStatus_Loading");
		if (CheckConnection(db_url + "saveScore.php")) {
			WWWForm form = new WWWForm ();
			form.AddField ("name", name);
			form.AddField ("score", score);
			form.AddField ("godmode", godmode);
			WWW webRequest = new WWW (db_url + "saveScore.php", form);
			yield return webRequest;
		} else {
			yield return SaveScores();
			error.GetComponent<Animator> ().Play ("ConnectionStatus_Error");
		}
	}

	IEnumerator LoadScores() {
		if (CheckConnection(db_url + "loadScore.php")) {
			error.GetComponent<Animator> ().Play ("ConnectionStatus_Loading");
			WWW webRequest = new WWW (db_url + "loadScore.php");
			yield return webRequest;
			scoresText = webRequest.text.Split ('|');
			isUpdating = false;
			isUpdatingScore = true;
		} else {
			yield return null;
			error.GetComponent<Animator> ().Play ("ConnectionStatus_Error");
		}
	}

	public void Refresh()
	{
		StartCoroutine (LoadScores());
	}
	
}