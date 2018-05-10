using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizer : MonoBehaviour {
	public Transform AnkContainer;
	public float Coeffiscient = 1;
	// Use this for initialization
	void Start () {
		Coeffiscient = GameObject.Find("Game").GetComponent<Permanent>().zoom;
		gameObject.GetComponent<Camera>().orthographicSize *= Coeffiscient;
		AnkContainer.localPosition *= Coeffiscient;
		AnkContainer.position = AnkContainer.position - new Vector3(0,0,AnkContainer.position.z);
		gameObject.GetComponent<Assets.Pixelation.Scripts.Pixelation>().BlockCount *= Coeffiscient;
	}
	
}
