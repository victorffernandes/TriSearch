using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject selected = null;
    public GameObject faixaObj;
    public GameObject tri;
    List<Color> colors = new List<Color>();
    Color[] avaibleColor;
    Dictionary<int, float> res = new Dictionary<int, float>();
    public int moves = 0;
	private int baseS;
    private int numOfRows;
    public  float timeLimit;

	public static bool checkIfCompleteRow(int row,Color c)
	{
		int selected = 0;
		int all = 0;
		foreach (Triangle t in FindObjectsOfType<Triangle>()) {
			if (t.faixa == row) {
				all++;
				if (t.gameObject.GetComponent<SpriteRenderer> ().color == c)
					selected++;
			}
		}
		return (selected == all);
	}


	public void InstantiateTriangles (float baseSize, int faixa ) {
		bool isUpsideDown = false;
		GameObject f = (GameObject)Instantiate (faixaObj, new Vector2(0,faixa *(1.24f)), Quaternion.identity);
		f.GetComponent<Row>().row = faixa;
		f.GetComponent<SpriteRenderer> ().color = avaibleColor [faixa];
		f.name = "Faixa :" + faixa;
		for (int i = 0; i < baseSize; i++) {

			GameObject g = (GameObject)Instantiate(tri, new Vector2(
				tri.transform.localScale.x * i * 2.5f + (faixa * .71f),
                tri.transform.localScale.y * faixa * 4.3f),
               	Quaternion.identity);
			int random = Random.Range(0,colors.Count);
			g.GetComponent<SpriteRenderer>().color = colors[random];
			colors.RemoveAt(random);
            g.GetComponent<Triangle>().faixa = faixa;
            g.name = "Triangle" + faixa + ":" + i;
			if (isUpsideDown) {
				g.GetComponent<Transform>().localScale = new Vector2(g.GetComponent<Transform>().localScale.x,g.GetComponent<Transform>().localScale.y*-1);  /*Vira ele aqui*/  
			}
			isUpsideDown = !isUpsideDown;
		}
		if (baseSize != 1) {
			InstantiateTriangles (baseSize - 2, faixa + 1);
		} else
			numOfRows = faixa;
	}

	public int getNumOfTri(int baseSize,int iteration)
	{
		for(int i = 0;i<baseSize;i++ ){colors.Add( avaibleColor[iteration]);}
		if(baseSize == 1){
			return baseSize;
		}
			else{
			iteration++;
			return baseSize + getNumOfTri(baseSize - 2,iteration);
		}
	}

	public void cameraFocus()
	{
		int t = ((numOfRows - 1) / 2) ;
		string name = "Triangle" + ((numOfRows - 1) / 2) + ":"+((int)baseS - (2*(t)))/2;
		Camera.main.transform.position = GameObject.Find (name).transform.position;
		Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, -0.95f - (0.05f/numOfRows), -10f);
	}

	void Update()
	{
		if (FindObjectsOfType<Row> ().Length == 0 && avaibleColor != null) {
            avaibleColor = null;
            Debug.LogWarning("Runned");
            FindObjectOfType<HUDController>().changeScene("Game");
			PlayerPrefs.SetInt ("phaseCount",PlayerPrefs.GetInt ("phaseCount") + 1);
            if(moves != 0)
			PlayerPrefs.SetInt("actualPoints", Mathf.RoundToInt((baseS*10)/moves) +  PlayerPrefs.GetInt ("actualPoints"));
            else
            PlayerPrefs.SetInt("actualPoints", Mathf.RoundToInt((baseS * 10)) + PlayerPrefs.GetInt("actualPoints"));
		}
	}
	/*
	 * triangulos feitos de cada tipo 3:5:7:9:11:13
	 * Base * 10  / jogadas;
	 * 
	 */
	IEnumerator timerTilEnd(float time)
	{
		yield return new WaitForSeconds (time);
        FindObjectOfType<HUDController>().changeScene("Record");
	}


	void Start () {
		//PlayerPrefs.SetInt ("phaseCount",0);
		//PlayerPrefs.SetInt ("actualPoints", 0);
		baseS = (PlayerPrefs.GetInt ("phaseCount") / 3) <= 13 ? (PlayerPrefs.GetInt ("phaseCount") / 3):13;
		baseS = (baseS < 3) ? 3 :baseS;
		baseS = (baseS % 2 == 0) ? baseS + 1 : baseS;
        //Debug.LogError((PlayerPrefs.GetInt("phaseCount") / 3) + ":" + PlayerPrefs.GetInt("phaseCount") + ":" + baseS);
		avaibleColor = new Color[7]{Color.blue,Color.green,Color.magenta,Color.gray,Color.cyan,Color.black,Color.white};
		getNumOfTri (baseS,0);
		InstantiateTriangles (baseS, 0);
		cameraFocus ();
		res.Add (13, 9.04f);
		res.Add (11, 7.73f);
		res.Add (9, 6.45f);
		res.Add (7, 5.19f);
		res.Add (5, 4.19f);
		res.Add (3, 2.82f);
		Camera.main.orthographicSize = res [baseS];
		timeLimit = baseS * 5;
		StartCoroutine (timerTilEnd (timeLimit));
	}
}