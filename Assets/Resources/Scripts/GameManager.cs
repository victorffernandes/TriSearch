using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public  static Color TonesColor;
	public static bool DefaultColors = false;
    public GameObject selected = null;//The selected triangle variable;
    public GameObject faixaObj;//The band prefab gameObj;
    public GameObject tri;//The triangle prefab  gameObj;
    List<Color> randomColors = new List<Color>();//An array that controls the triangles sequence thats doesnt need to be diferent;
    List<Color> securityColors = new List<Color>();//An array that controls the triangles sequence thats need to be diferent;
    List<Color> avaibleColor = new List<Color>();//All the avaible colors in the piramid;
    Dictionary<int, float> res = new Dictionary<int, float>();//A dictionary that have all the camera viewport size for different piramid sizes
    public int moves = 0;// Moves Count
    public static int baseS;// Number of triangles on the base of the piramid
	public static int numOfRows;// Number of bands and piramid's level.
    public float timeLimit;// Time to finish phase

    public List<Color> castListTo(List<GameObject> b)
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
                Debug.LogWarning("Worked!");
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
        GameObject f = (GameObject)Instantiate(faixaObj, new Vector2(0, faixa * (1.24f)), Quaternion.identity);
        f.GetComponent<Row>().row = faixa;
        f.GetComponent<SpriteRenderer>().color = avaibleColor[faixa];
        f.name = "Faixa :" + faixa;
        //--
        for (int i = 0; i < baseSize; i++)
        {
            GameObject g = (GameObject)Instantiate(tri, new Vector2(
                tri.transform.localScale.x * i * 2.5f + (faixa * .71f),
                tri.transform.localScale.y * faixa * 4.3f),
                Quaternion.identity);
            //int dif = getDifColorFromArray(colors,avaibleColor [faixa]);
            //g.GetComponent<SpriteRenderer>().color = colors[dif];
            //colors.RemoveAt(dif);
            g.GetComponent<Triangle>().faixa = faixa;
            g.name = "Triangle" + faixa + ":" + i;
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
        foreach (GameObject tri in rowTri)
        {
            if (tri != rowTri[j])
            {
                int i = Random.Range(0, randomColors.Count);
                tri.GetComponent<SpriteRenderer>().color = randomColors[i];
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
        int t = ((numOfRows - 1) / 2);
        string name = "Triangle" + ((numOfRows - 1) / 2) + ":" + ((int)baseS - (2 * (t))) / 2;
        Camera.main.transform.position = GameObject.Find(name).transform.position;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, -0.95f - (0.05f / numOfRows), -10f);
    }

    void Update()
    {
        if (FindObjectsOfType<Row>().Length == 0 && avaibleColor != null)
        {
            avaibleColor = null;
            Debug.LogWarning("Runned");
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
    IEnumerator timerTilEnd(float time)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<HUDController>().changeScene("Submit");
    }

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

        //PlayerPrefs.SetInt ("phaseCount",0);
        //PlayerPrefs.SetInt ("actualPoints", 0);
        baseS = (PlayerPrefs.GetInt("phaseCount") <= 3) ? 3 :
            (PlayerPrefs.GetInt("phaseCount") <= 6) ? 5 : (PlayerPrefs.GetInt("phaseCount") <= 9) ? 7 :
                (PlayerPrefs.GetInt("phaseCount") <= 12) ? 9 : (PlayerPrefs.GetInt("phaseCount") <= 15) ? 11 :
                (PlayerPrefs.GetInt("phaseCount") >= 18) ? 13 : 13;
		//baseS =13;
        //Debug.LogError((PlayerPrefs.GetInt("phaseCount") / 3) + ":" + PlayerPrefs.GetInt("phaseCount") + ":" + baseS);
        InitColor();
        InstantiateTriangles(baseS, 0);
        cameraFocus();
        res.Add(13, 9.04f);
        res.Add(11, 7.73f);
        res.Add(9, 6.55f);
        res.Add(7, 5.28f);
        res.Add(5, 4.07f);
        res.Add(3, 2.82f);
        Camera.main.orthographicSize = res[baseS];
        timeLimit = baseS * 5;
        StartCoroutine(timerTilEnd(timeLimit));

		for (int i = 0; i < numOfRows; i++) {
			if(checkIfCompleteRow(i,avaibleColor[i]))Application.LoadLevel("Game");
		}

    }
}