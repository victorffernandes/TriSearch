using UnityEngine;
using System.Collections;

public class Row : MonoBehaviour {
	public int row ;
	Color col;
	
	void Start () {
		col = GetComponent<SpriteRenderer> ().color;
	}

	public static bool checkSimilarity(int j){
		Row[] rows = GameObject.FindObjectsOfType<Row> ();
		bool r = false;
		for (int i = 0; i < rows.Length; i++) {
			r = (rows[i].row == j);
			if(r)return r;
		}
		return r;
	}

	public bool checkIncoerence(){
		if (row != 0 && row != GameManager.numOfRows-1) {
			if(checkSimilarity(row + 1) && checkSimilarity(row - 1))
				return true;
		}
		return false;
	}

	public void destroyRow()
	{
		Destroy (gameObject);
		foreach (Triangle t in FindObjectsOfType<Triangle>()) {
			if(t.faixa == row) Destroy(t.gameObject);
		}
		if (checkIncoerence ()) {
			FindObjectOfType<HUDController> ().changeScene ("Submit");
			GameObject.FindGameObjectWithTag ("negativeSound").GetComponent<AudioSource> ().Play ();
		} else {
			GameObject.FindGameObjectWithTag ("positiveSound").GetComponent<AudioSource> ().Play ();
		}
	}

	void Update () {
		if (GameManager.checkIfCompleteRow (row, col)) {
			destroyRow();
			PlayerPrefs.SetInt("actualPoints", 2*((GameManager.baseS - (row *2))) + PlayerPrefs.GetInt("actualPoints"));
		}
	}
}
