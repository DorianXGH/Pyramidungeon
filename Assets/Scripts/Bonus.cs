using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {
	public string BonusName = "";
	// Use this for initialization
	void Start () {
		
	}
	public string Hit()
	{
		Destroy(gameObject);
		return BonusName;
	}
}
