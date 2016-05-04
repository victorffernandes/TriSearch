using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUDController : MonoBehaviour {

	public GameObject p;
	private ColorHSV rainbowButton = new ColorHSV (0, 1, 1);
	private bool gameModes = false;
	List<Color> cs;
	public float left = 0;

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
		cs = FindObjectOfType<GameManager> ().avaibleColor;
		GameObject.FindGameObjectWithTag("UITime").GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
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
		Debug.Log("isRuning");
		left += .5f;
		GameObject.FindGameObjectWithTag("UITime").GetComponent<Image>().fillAmount = (left/FindObjectOfType<GameManager>().timeLimit);
		GameObject.FindGameObjectWithTag("UIScore").GetComponent<Text>().text = PlayerPrefs.GetInt("actualPoints").ToString();
		GameObject.FindGameObjectWithTag ("UITime").GetComponent<Image> ().color = cs [Random.Range (0, cs.Count)];     
		if (GameManager.isPaused == false) {
			//Debug.Log (GameManager.isPaused);
			StartCoroutine (atualizeHUD (0.5f));
		}
		if (left >= FindObjectOfType<GameManager> ().timeLimit) {
			GameObject.FindGameObjectWithTag ("negativeSound").GetComponent<AudioSource> ().Play ();
			changeScene("Submit");
		}
    }
		
	public void showPause(GameObject g){
		if(!GameManager.isPaused)StartCoroutine (atualizeHUD (.5f));
		GameManager.isPaused = ! GameManager.isPaused;
		g.SetActive (GameManager.isPaused);
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
