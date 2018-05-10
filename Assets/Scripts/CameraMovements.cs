using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour {
	public Transform Tracked;
	public Vector2 Freedom = new Vector2(1f,0.75f);
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
		float pixelsInBlock = (float)(Screen.width)/(float)(gameObject.GetComponent<Assets.Pixelation.Scripts.Pixelation>().BlockCount);
		float worldInBlock = pixelsInBlock/worldToPixels;
		transform.position = new Vector3(Tracked.position.x, Tracked.position.y, transform.position.z) + (Vector3)Freedom;
		transform.position = new Vector3(
			Mathf.Floor(transform.position.x/worldInBlock)*worldInBlock,
			Mathf.Floor(transform.position.y/worldInBlock)*worldInBlock,
			Mathf.Floor(transform.position.z/worldInBlock)*worldInBlock
		);
	}
}
