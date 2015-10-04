using UnityEngine;
using System.Collections;

public class ColliderController : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Triangle" && col.gameObject != transform.parent.gameObject)
        {
            col.gameObject.GetComponent<Triangle>().options.Add(transform.parent.gameObject);
        }
    }
}
