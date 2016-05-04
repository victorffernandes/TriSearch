using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//1 XSCALE = 2.51 XPOS	
//1 YSCALE = 4.34 YPOS

public class GameManager : MonoBehaviour
{
	public static bool isPaused = false;
	public  static Color TonesColor;
	public static bool DefaultColors = false;
    public GameObject selected = null;//The selected triangle variable;
    public GameObject faixaObj;//The band prefab gameObj;
    public GameObject tri;//The triangle prefab  gameObj;
    List<Color> randomColors = new List<Color>();//An array that controls the triangles sequence thats doesnt need to be diferent;
    List<Color> securityColors = new List<Color>();//An array that controls the triangles sequence thats need to be diferent;
    public List<Color> avaibleColor = new List<Color>();//All the avaible colors in the piramid;
    public int moves = 0;// Moves Count
    public static int baseS;// Number of triangles on the base of the piramid
	public static int numOfRows;// Number of bands and piramid's level.
    public float timeLimit;// Time to finish phase
	public float w_Triangle;
	public float h_Triangle;
	const float X_SCALE = 2.51f;
	const float Y_SCALE = 4.34f;

    
	List<Color> castListTo(List<GameObject> b)
    {
        List<Color> result = new List<Color>();
        foreach (GameObject g in b)
        {
            result.Add(g.GetComponent<SpriteRenderer>().color);
        }
        return result;
    }


    public int getSecurityColor(List<Color> c, Color color)
    {
        int i = c.Count - 1;
        do
        {
            if (color != c[i])
            {
                //Debug.LogWarning("Worked!");
                return i;	
            }
            i--;
        } while (i > 0);
        return Random.Range(0, c.Count);
    }


    public static bool checkIfCompleteRow(int row, Color c)
    {
        int selected = 0;
        int all = 0;
        foreach (Triangle t in FindObjectsOfType<Triangle>())
        {
            if (t.faixa == row)
            {
                all++;
                if (t.gameObject.GetComponent<SpriteRenderer>().color == c)
                    selected++;
            }
        }
        return (selected == all);
    }

    /// <summary>
    /// Recursive void that instantiate de game piramid
    /// </summary>
    /// <param name="baseSize">Size of the base of the piramid</param>
    /// <param name="faixa">The number of the actual band</param>
    public void InstantiateTriangles(float baseSize, int faixa)
    {
        List<GameObject> rowTri = new List<GameObject>();
        bool isUpsideDown = false;
        //Instantiate de background colorful band
		GameObject f = (GameObject)Instantiate(faixaObj, new Vector2(0,(faixa*h_Triangle*Y_SCALE)), Quaternion.identity);
        f.GetComponent<Row>().row = faixa;
        f.GetComponent<SpriteRenderer>().color = avaibleColor[faixa];
        f.name = "Faixa :" + faixa;
		f.transform.localScale = new Vector3 (f.transform.localScale.x,Y_SCALE* h_Triangle, 0);
        for (int i = 0; i < baseSize; i++)
        {
			//Debug.LogWarning("Runned");
            GameObject g = (GameObject)Instantiate(tri, new Vector2(
				i * (w_Triangle * X_SCALE ) + (faixa*w_Triangle*X_SCALE),
				(faixa*h_Triangle*Y_SCALE)),
                Quaternion.identity);
			g.transform.localScale = new Vector3((tri.transform.localScale.x * w_Triangle),
			                                      (tri.transform.localScale.y *h_Triangle),0);
            g.GetComponent<Triangle>().faixa = faixa;
            g.name = "Triangle:" + faixa + ":" + i;
            if (isUpsideDown)
            {
                g.GetComponent<Transform>().localScale = new Vector2(g.GetComponent<Transform>().localScale.x, g.GetComponent<Transform>().localScale.y * -1);  /*Vira ele aqui*/
            }
            isUpsideDown = !isUpsideDown;
            rowTri.Add(g);
        }
        int j = Random.Range(0, rowTri.Count);
        int n = getSecurityColor(securityColors, avaibleColor[faixa]);
        rowTri[j].GetComponent<SpriteRenderer>().color = securityColors[n];
        securityColors.RemoveAt(n);
        //Set triangles color
        foreach (GameObject t in rowTri)
        {
            if (t != rowTri[j])
            {
                int i = Random.Range(0, randomColors.Count);
				t.GetComponent<SpriteRenderer>().color = randomColors[i];
                randomColors.RemoveAt(i);
            }
        }
        //---
        if (baseSize != 1)
        {
            InstantiateTriangles(baseSize - 2, faixa + 1);
        }
    }

    public int getNumOfTri(int baseSize, int iteration)
    {
        for (int i = 1; i < baseSize; i++)
        {
            randomColors.Add(avaibleColor[iteration]);
        }

        if (baseSize == 1)
        {
            numOfRows = iteration + 1;
            return baseSize;
        }
        else
        {
            iteration++;
            return baseSize + getNumOfTri(baseSize - 2, iteration);
        }
    }

    public void cameraFocus()
    {
		Vector3 zeroOne = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 0));
		Vector3 oneOne = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));

		while (Mathf.Abs(oneOne.x - zeroOne.x) <= (baseS+1) * X_SCALE) {
			Camera.main.orthographicSize += 0.02f;
			zeroOne = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 0));
			oneOne = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		}	
		Vector3 zeroZero = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		h_Triangle = Mathf.Abs ((oneOne.y - zeroZero.y)) / (Y_SCALE*(numOfRows+1));
    }

	public void cameraPositioning(){
		int t = ((numOfRows - 1) / 2);
		string name = "Triangle:" + ((numOfRows - 1) / 2) + ":" + ((int)baseS - (2 * (t))) / 2;
		Camera.main.transform.position = GameObject.Find(name).transform.position + new Vector3(0,0,-10);
		Vector3 oo = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 lt = GameObject.Find("Triangle:"+(numOfRows-1)+":0").transform.position;
		float dft = Mathf.Abs (oo.y) - Mathf.Abs (lt.y);
		if (baseS == 5 || baseS == 9 || baseS == 13)
		Camera.main.transform.Translate (new Vector3 (0, -dft / 2, 0));

	}

    void Update()
    {
        if (FindObjectsOfType<Row>().Length == 0 && avaibleColor != null)
        {
            avaibleColor = null;
            FindObjectOfType<HUDController>().changeScene("Game");
            PlayerPrefs.SetInt("phaseCount", PlayerPrefs.GetInt("phaseCount") + 1);
            if (moves != 0)
                PlayerPrefs.SetInt("actualPoints", Mathf.RoundToInt((baseS * 4) - moves) + PlayerPrefs.GetInt("actualPoints"));
            else
                PlayerPrefs.SetInt("actualPoints", Mathf.RoundToInt((baseS * 4)) + PlayerPrefs.GetInt("actualPoints"));
        }
    }
    /*
     * triangulos feitos de cada tipo 3:5:7:9:11:13
     * Base * 10  / jogadas;
     * 
     */
    void InitColor()
    {
		List<Color> colors = new List<Color>();
		if (DefaultColors) {
			colors.Add (new Color (245f / 255f, 153f / 255f, 24f / 255f, 1f));
			colors.Add (new Color (255f / 255f, 239f / 255f, 43f / 255f, 1f));
			colors.Add (new Color (88f / 255f, 193f / 255f, 212f / 255f, 1f));
			colors.Add (new Color (87f / 255f, 38f / 255f, 122f / 255f, 1f));
			colors.Add (new Color (231f / 255f, 41f / 255f, 123f / 255f, 1f));
			colors.Add (new Color (230f / 255f,55f / 255f, 31f / 255f, 1f));
			colors.Add (new Color (145f / 255f, 206f / 255f, 195f/255f, 1f));
		} else colors = ColorUtils.GetTones (TonesColor, 7);
		foreach (Color c in colors)
		avaibleColor.Add (c);

        securityColors = new List<Color>(avaibleColor);
        getNumOfTri(baseS, 0);
        securityColors.RemoveRange(numOfRows, securityColors.Count - numOfRows);
    }


    void Start()
    {
        baseS = (PlayerPrefs.GetInt("phaseCount") <= 3) ? 3 :
            (PlayerPrefs.GetInt("phaseCount") <= 6) ? 5 : (PlayerPrefs.GetInt("phaseCount") <= 9) ? 7 :
                (PlayerPrefs.GetInt("phaseCount") <= 12) ? 9 : (PlayerPrefs.GetInt("phaseCount") <= 15) ? 11 :
                (PlayerPrefs.GetInt("phaseCount") >= 18) ? 13 : 13;
		InitColor();
		cameraFocus ();
        InstantiateTriangles(baseS, 0);
		cameraPositioning ();
        timeLimit = baseS * 5;

		for (int i = 0; i < numOfRows; i++) {
			if(checkIfCompleteRow(i,avaibleColor[i]))Application.LoadLevel("Game");
		}

    }	
}