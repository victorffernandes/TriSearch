using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GameObject p;
	private ColorHSV rainbowButton = new ColorHSV (0, 1, 1);
	private bool gameModes = false;

	public void SelectColor(string color)
	{
		GameManager.DefaultColors = false;
		switch (color) {

		case "red":
			GameManager.TonesColor = Color.red;
			break;
		case "blue":
			GameManager.TonesColor = Color.blue;
			break;
		case "green":
			GameManager.TonesColor = Color.green;
			break;
		case "colors":
			GameManager.DefaultColors = true;
			break;
		}
	}

    public void initScore()
    {
        PlayerPrefs.SetInt("actualPoints", 0);
        PlayerPrefs.SetInt("phaseCount", 0);
    }

    void Start()
    {
 if (Application.loadedLevelName == "Game" ) {
						GameObject.FindGameObjectWithTag ("UIScore").GetComponent<Text> ().text = PlayerPrefs.GetInt ("actualPoints").ToString ();
						StartCoroutine (atualizeHUD (0.5f));
		} else if (Application.loadedLevelName == "Menu")
		initScore ();
    }

	void Update() {	
		rainbowButton.h += (rainbowButton.h < 360) ? 1 * Time.deltaTime : 0;
		if(gameModes) 
			GameObject.Find ("Colors").GetComponent<Image> ().color = ColorUtils.HSVtoRGB (rainbowButton);
	}

    IEnumerator atualizeHUD(float time)
    {
        yield return new WaitForSeconds(time);
		Debug.LogWarning("It's running!");
        GameObject.FindGameObjectWithTag("UITextMoves").GetComponent<Text>().text = FindObjectOfType<GameManager>().moves.ToString();
        GameObject.FindGameObjectWithTag("UITime").GetComponent<Text>().text = (Mathf.RoundToInt(FindObjectOfType<GameManager>().timeLimit - Time.timeSinceLevelLoad)).ToString();
		GameObject.FindGameObjectWithTag("UIScore").GetComponent<Text>().text = PlayerPrefs.GetInt("actualPoints").ToString();
        StartCoroutine(atualizeHUD(0.5f));
    }

    public void changeScene(string h)
    {
        StartCoroutine(changeC(h));
    }

	public void restart(string h)
	{
		StartCoroutine(changeC(h));
		initScore ();
	}

    public IEnumerator changeC(string g)
    {
		p = GameObject.FindGameObjectWithTag ("fade");
		p.GetComponent<Animator>().Play("fadeOut");
        yield return new WaitForSeconds(0.5f);
        Application.LoadLevel(g);
    }

	public void changeGameMode(bool check) {
		gameModes = check;
	}
}
