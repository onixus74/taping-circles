using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Tongue : MonoBehaviour
{
	public GameObject catcher;
	public Transform mouth;
	public int VertexCount = 10;
	public Vector3 target;
	bool isHuntingIn = false;
	bool isHuntingOut = false;
	// Use this for initialization
	LineRenderer lineRenderer;

	void Start ()
	{
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetVertexCount (VertexCount);
		catcher.transform.position = mouth.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0) && !isHuntingIn && !isHuntingOut) {
			target  = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			target.z = -1;
			isHuntingIn = true;
		}
		if (isHuntingIn) {
			catcher.transform.position = Vector3.Lerp (catcher.transform.position, target, Time.deltaTime * 10);
			if (Vector3.Distance (catcher.transform.position, target) < 0.1f) {
				isHuntingIn = false;
				isHuntingOut = true;
				
			}
		} else if (isHuntingOut) {
			catcher.transform.position = Vector3.Lerp (catcher.transform.position, mouth.position, Time.deltaTime * 10);
			if (Vector3.Distance (catcher.transform.position, mouth.position) < 0.1f) {
				isHuntingOut = false;
				for(int i=0;i<catcher.transform.childCount;i++){
					Destroy(catcher.transform.GetChild (i).gameObject);
				}
			}
		}
		if (isHuntingIn || isHuntingOut) {
			lineRenderer.enabled = true;
			catcher.SetActive(true);
			float randomRange = (Vector3.Distance (mouth.position, catcher.transform.position) / VertexCount) / 15f;
			
			for (int i = 0; i<VertexCount; i++) {
				float t = ((float)(i) / (float)VertexCount) * ((float)(VertexCount + 1) / (float)VertexCount);
				Vector3 pos = Vector3.Lerp (mouth.position, catcher.transform.position, t) +
					new Vector3 (Random.Range (-randomRange, randomRange),
					             Random.Range (-randomRange, randomRange),
					             -1);
				lineRenderer.SetPosition (i, pos);
			}
		} else {
			lineRenderer.enabled = false;
			catcher.SetActive(false);

		}
	}
}
