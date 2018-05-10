using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
	public int Change = 0;
	public int ID;
	public bool LightFade = true;
	public bool Enabled = true;
	public bool RandomC = false;
	public bool DontDisableAfterHit = false;
	public float LuckMinus = 0.3f;
	public float StartLightIntensity;
	private GameObject lightO;
	// Use this for initialization
	void Start () {
		if(transform.childCount>0)
		{
			lightO = transform.GetChild(0).gameObject;
		}
		
		Vector2 pos = transform.position;
		ID = int.Parse(((int)pos.x).ToString()+((int)pos.y).ToString());
	}
		
	IEnumerator Fade() {
		
		for (float f = 0f; f <= 1.001f; f += 0.1f) {
			float x = f;
			lightO.GetComponent<Light>().intensity = Mathf.Lerp(StartLightIntensity,0f,x);
			yield return null;
		}
	}

	public int Hit()
	{
		int ret = 0;
		if(Enabled)
		{
			ret = Change;
			if (RandomC && Random.value < LuckMinus)
			{
				ret = -Change;
			}
			if(LightFade)
			{
				StartLightIntensity = lightO.GetComponent<Light>().intensity;
				StartCoroutine("Fade");
			}
			if(!DontDisableAfterHit)
			{
				Enabled = false;
			}
		}
		return ret;
	}
}
