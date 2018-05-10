using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Permanent : MonoBehaviour {
	public int Score;
	public int Level;
	public int Anks;
	public float zoom = 1f;
	public bool Boss;
	GameObject ScoreObj;
	GameObject AnkIndicator;
	// Use this for initialization
	void Start () {
		if(GameObject.FindGameObjectsWithTag("Permanent").GetLength(0)==2)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	public void NewRun()
	{
		Score = 0;
		Level = 1;
		Anks = 1;
		Boss = false;
	}
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if(scene.name == "mainScene")
		{
        	ScoreObj = GameObject.Find("Score");
			AnkIndicator = GameObject.Find("multipleAnks");
		}
    }
	void Update () {
		if(Score<0)
		{
			Score = 0;
		}
		if(SceneManager.GetActiveScene().name == "mainScene")
		{
			ScoreObj.GetComponent<TMPro.TextMeshPro>().text = "Score : " + Score.ToString();
			if(Anks > 1)
			{
				AnkIndicator.GetComponent<TMPro.TextMeshPro>().text = "x" + Anks.ToString();
			}
			else
			{
				AnkIndicator.GetComponent<TMPro.TextMeshPro>().text = "";
			}
		}
	}
}
