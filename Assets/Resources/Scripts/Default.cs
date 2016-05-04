using UnityEngine;
using System.Collections;

public abstract class Default : MonoBehaviour {

	public virtual void OnTouch(object o) { }
	
	
	public virtual void Update () {
		if (Input.touchSupported && Input.touchCount > 0 && !GameManager.isPaused) {
			for (int i = 0; i < Input.touchCount; i++) {
				Vector2 point = new Vector2 (Input.GetTouch (i).position.x, Input.GetTouch (i).position.y);
				if (this.gameObject.GetComponent<PolygonCollider2D>().OverlapPoint (Camera.main.ScreenToWorldPoint (point)))
					OnTouch (Input.GetTouch (i));
			}
		} else {
			Vector2 point = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if (this.gameObject.GetComponent<PolygonCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(point)) && Input.GetMouseButton(0))
				OnTouch(Input.mousePosition);
		}
	}
}