using UnityEngine;
using System.Collections;

public class Row : MonoBehaviour {
	public int row ;
	Color col;
	// Use this for initialization
	void Start () {
		col = GetComponent<SpriteRenderer> ().color;
	}

	public void destroyRow()
	{
		Destroy (gameObject);
		foreach (Triangle t in FindObjectsOfType<Triangle>()) {
			if(t.faixa == row) Destroy(t.gameObject);
		}
	}


	// Update is called once per frame
	void Update () {
		if (GameManager.checkIfCompleteRow (row, col)) {
			destroyRow();
			PlayerPrefs.SetInt("actualPoints", 2*((GameManager.baseS - (row *2))) + PlayerPrefs.GetInt("actualPoints"));
		}
	}
}
