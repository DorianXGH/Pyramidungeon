using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToMenu : MonoBehaviour {
	public float duration = 1f;
	private float elapsed = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if(elapsed>=duration)
		{
			Initiate.Fade("mainMenu",Color.black,2f);
		}
	}
}
