using UnityEngine;
using System.Collections;

public class GooglyEye : MonoBehaviour {
	public static Transform LookAt;
	public Transform DefaultLookAt;
	public float range = 5;

	float currentAngle = 0f;
	float targetAngle = -1200f;
	Vector3 cross;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos;
		if (LookAt) {
			targetPos = LookAt.position;
		} else if (DefaultLookAt) {
			targetPos = DefaultLookAt.position;
		} else {
			targetPos = Vector3.zero;
		}

		targetAngle = Vector2.Angle (Vector2.right, targetPos - transform.parent.position);
		cross = Vector3.Cross (Vector2.right, targetPos);
		if (cross.z < 0) {
			targetAngle = 360 - targetAngle;
		}
		if (Mathf.Abs (currentAngle - targetAngle) > 180f) {
			targetAngle -=360f;
		}
		transform.localPosition = new Vector3(range,0f,0f);
		transform.localRotation = Quaternion.identity;
		transform.RotateAround (transform.parent.position, Vector3.forward, currentAngle);

		currentAngle = Mathf.Lerp (currentAngle, targetAngle, Time.deltaTime*5);

	}
}
