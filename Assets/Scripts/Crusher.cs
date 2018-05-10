using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour {
	GameObject spikePrefab;
	GameObject colPrefab;
	public int scoreMult = 500;
	public int startLine;
	public int endLine;
	public int startRow;
	public int endRow;
	public float Scale = 0.3f;
	
	public float maxTravelSpeed;
	public float travelSpeed;
	private float travelProgress = 0f;
	public int travelLenght;
	// Use this for initialization
	void Start () {
		travelSpeed = 0;
		spikePrefab = (GameObject)Resources.Load("Elements/Spikes");
		colPrefab = (GameObject)Resources.Load("Sprites/columnG");
		for (int k = startLine; k <= endLine; k++)
		{
			for(int l = startRow; l <= endRow; l++)
			{
				if(l == endRow)
				{
					PlaceElement(spikePrefab,l,k);
				}
				else
				{
					PlaceElement(colPrefab,l,k);
				}
			}
		}
		
	}
	public void Enable()
	{
		travelSpeed = maxTravelSpeed;
	}
	public void PlaceElement(GameObject obj, int col, int line)
	{
		GameObject Elem = GameObject.Instantiate(obj);
		
		Elem.transform.parent = transform;
		Elem.transform.localPosition = Scale*(new Vector2(col,line));
	}
	// Update is called once per frame
	void Update () {
		if(travelProgress <= travelLenght*Scale)
		{
			float prog = Scale*travelSpeed*Time.deltaTime;
			travelProgress += prog;
			GameObject.Find("Game").GetComponent<Permanent>().Score += Mathf.RoundToInt(scoreMult*prog);
			transform.Translate(prog,0f,0f);
		}

	}
}
