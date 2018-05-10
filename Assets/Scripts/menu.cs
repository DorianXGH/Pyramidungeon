using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour {
	public Transform startItem;
	public Transform optionsItem;
	public Transform quitItem;
	public Transform ankSprite;
	public int menuPos = 0;
	public int items = 3;

	// Use this for initialization
	void Start () {
		AnkUpdatePos();
	}
	void AnkUpdatePos()
	{
		Transform selectedItem; 
		if(menuPos == 0)
		{
			selectedItem = startItem;
		}
		else if (menuPos == 1)
		{
			selectedItem = optionsItem;
		}
		else if (menuPos == 2)
		{
			selectedItem = quitItem;
		}
		else
		{
			selectedItem = transform;
		}
		ankSprite.position = selectedItem.position + new Vector3(- ((RectTransform)selectedItem).rect.width/2f,0,0);

	}
	// Update is called once per frame
	int mod(int k, int n) {  return ((k %= n) < 0) ? k+n : k;  }
	void Update () {
		if(Input.GetKeyDown("up"))
		{
			menuPos --;
			menuPos = mod(menuPos,items);
			AnkUpdatePos();
		}
		if(Input.GetKeyDown("down"))
		{
			menuPos ++;
			menuPos = mod(menuPos,items);
			AnkUpdatePos();
		}
		if(Input.GetKeyDown("left"))
		{
			GameObject.Find("Game").GetComponent<Permanent>().zoom *= 2f;
		}
		if(Input.GetKeyDown("right"))
		{
			GameObject.Find("Game").GetComponent<Permanent>().zoom *= 0.5f;
		}
		if(Input.GetKeyDown("return"))
		{
			if(menuPos == 0)
			{
				GameObject.Find("Game").GetComponent<Permanent>().NewRun();
				Initiate.Fade("mainScene",Color.black,2f);
			}
			if(menuPos == 2)
			{
				Application.Quit();
			}
		}
	}
}
