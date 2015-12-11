using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Bug : MonoBehaviour {

	void Start(){
		GooglyEye.LookAt = transform;
	}

	bool isCatched = false;
	void OnTriggerEnter2D(Collider2D other) {
		if (!isCatched) {
			if (other.gameObject.tag == "Catcher") {
				Handheld.Vibrate();
				isCatched = true;
				GetComponent<Collider2D>().enabled = false;
				transform.parent = other.gameObject.transform;
				GetComponent<Animation>().Stop();
			}
		}
	}

	void Update(){
		if (isCatched && transform.parent) {
			transform.position = Vector3.Lerp(transform.position,transform.parent.position,Time.deltaTime*10);
		}
	}
}
