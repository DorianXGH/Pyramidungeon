using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SegmentType {START, STANDARD, END, BOSS}

public enum BossType {HORUS,AMON,ANUBIS,SETH}

public class Segment {
	public SegmentType Type;
	public List<GameObject> Blocks;
	public float Difficulty;
	public int MaxHeight;
	public int SegmentWidth;
	public int Ceiling = 15;
	public int aboveCeiling = 10;
	public int Position;
	public static float Scale = 0.3f;
	public static GameObject go;
	public static bool ToInit = true;
	public static Dictionary<string, GameObject> Elements = new Dictionary<string, GameObject>();
	public Segment(SegmentType T, float Diff, int SegWidth, int Pos, int MaxH)
	{
		Type = T;
		Difficulty = Diff;
		SegmentWidth = SegWidth;
		Position = Pos;
		MaxHeight = MaxH;
	}

	public static void Init()
	{
		Elements.Add("Normal", (GameObject)Resources.Load("Elements/column"));
		Elements.Add("NormalG", (GameObject)Resources.Load("Sprites/columnG")); // physicless and lightless
		Elements.Add("Plus", (GameObject)Resources.Load("Elements/trap3"));
		Elements.Add("Minus", (GameObject)Resources.Load("Elements/trap2"));
		Elements.Add("Random", (GameObject)Resources.Load("Elements/trap1"));
		Elements.Add("CrusherTrigger", (GameObject)Resources.Load("Elements/CrushTrigger"));
		Elements.Add("Lava", (GameObject)Resources.Load("Elements/lava"));
		Elements.Add("LavaG", (GameObject)Resources.Load("Sprites/lavaG")); // physicless and lightless
		Elements.Add("Ank", (GameObject)Resources.Load("Elements/bonusAnk"));
		
		ToInit = false;
	}
	public static void InitO(GameObject o) // to init the parent GO
	{
		go = o;
	}
	public void PlaceElement(string Name, int col, int line)
	{
		GameObject Elem = GameObject.Instantiate(Elements[Name]);
		Elem.transform.position = new Vector2(Scale*(Position+col),Scale*line);
		Elem.transform.parent = go.transform;
	}
	public int GenerateStart()
	{
		PlaceElement("CrusherTrigger",SegmentWidth-1,0);
		for(int i = 0; i < SegmentWidth; i++)
		{
			string lavaOrNormal = "Normal";

			for(int k = 0; k>-10; k--)
			{
				PlaceElement(lavaOrNormal,i,k);
				if(k == 0) // only make the first layer lit and physic
				{
					lavaOrNormal += 'G';
				}
			}
			lavaOrNormal = "Normal"; // reset it, we kinda need physics for he ceiling too
			for(int k = Ceiling; k< Ceiling+aboveCeiling; k++)
			{
				PlaceElement(lavaOrNormal,i,k);
				if(k == Ceiling) // only make the first layer lit and physic
				{
					lavaOrNormal += 'G';
				}
			}
		}
		for(int i = 0; i > -20 ; i--)
		{
			string lavaOrNormal = "NormalG";

			for(int k = -9; k< Ceiling+aboveCeiling; k++)
			{
				if(k<=0 || k>=Ceiling)
				{
				PlaceElement(lavaOrNormal,i,k);
				}
			}
		}
		return 0;
	}
	public int GenerateEnd()
	{
		PlaceElement("CrusherTrigger",SegmentWidth-1,0);
		for(int i = 0; i < SegmentWidth; i++)
		{
			string lavaOrNormal = "Normal";

			for(int k = 0; k>-5; k--)
			{
				PlaceElement(lavaOrNormal,i,k);
				if(k == 0) // only make the first layer lit and physic
				{
					lavaOrNormal += 'G';
				}
			}
			lavaOrNormal = "Normal"; // reset it, we kinda need physics for he ceiling too
			for(int k = Ceiling; k< Ceiling+aboveCeiling; k++)
			{
				PlaceElement(lavaOrNormal,i,k);
				if(k == Ceiling) // only make the first layer lit and physic
				{
					lavaOrNormal += 'G';
				}
			}
		}
		for(int i = SegmentWidth; i < SegmentWidth + 20; i++)
		{
			string lavaOrNormal = "Normal";
			if(i > SegmentWidth) // only make the first layer lit and physic
			{
					lavaOrNormal += 'G';
			}
			for(int k = -9; k< Ceiling+aboveCeiling; k++)
			{
				PlaceElement(lavaOrNormal,i,k);
				
			}

		}
		return 0;
	}
	public int GenerateStandard()
	{
		int trapPos = Random.Range(0,SegmentWidth);
		int bonusPos = Random.Range(0,SegmentWidth);
		int Rheight = Random.Range(1,MaxHeight+1);
		int lava = Random.Range(0,101);
		
		List<int> probCuts = new List<int>();
		probCuts.Add(30 + (int)Mathf.Round(40*Difficulty)); // minus
		probCuts.Add(70 + (int)Mathf.Round(30*Difficulty)); // random
		probCuts.Add(100); // plus
		int height = Rheight * 3;
		Debug.Log(Difficulty);
		for(int i = 0; i < SegmentWidth; i++)
		{
			string lavaOrNormal = "Lava";
			if(lava <= 60-(60*Difficulty))
			{
				lavaOrNormal = "Normal";
			}
			for(int k = 0; k>-10; k--)
			{
				PlaceElement(lavaOrNormal,i,k);
				if(k == 0) // only make the first layer lit and physic
				{
					lavaOrNormal += 'G';
				}
			}
			lavaOrNormal = "Normal"; // let's do the ceiling in normal tiles
			for(int k = Ceiling; k< Ceiling+aboveCeiling; k++)
			{
				PlaceElement(lavaOrNormal,i,k);
				if(k == Ceiling) // only make the first layer lit and physic
				{
					lavaOrNormal += 'G';
				}
			}
			if(i == bonusPos)
			{
				if( Random.value < 0.015)
				{
					PlaceElement("Ank",i,height +4);
				}
			}

			if(i != trapPos)
			{
				PlaceElement("Normal",i,height);
			}
			else
			{
				int choice = Random.Range(0,101);
				if(choice <= probCuts[0])
				{
					PlaceElement("Minus",i,height);
				}
				else if(choice <= probCuts[1])
				{
					PlaceElement("Random",i,height);
				}
				else if(choice <= probCuts[2])
				{
					PlaceElement("Plus",i,height);
				}
			}
		}
		return Rheight;
	}
	public int GenerateSegment()
	{
		int Rheight = 0;
		switch(Type)
		{
			case SegmentType.STANDARD:
				Rheight = GenerateStandard();
				break;
			case SegmentType.START:
				Rheight = GenerateStart();
				break;
			case SegmentType.END:
				Rheight = GenerateEnd();
				break;
			default:
				break;
		}
		return Rheight;
	}

}
public class LevelGen : MonoBehaviour {

	// Use this for initialization
	List<Segment> Segs = new List<Segment>();
	public int NumberOfSegments = 50;
	public float DifficultyDivisor = 2f;
	void Start () {
		Random.InitState(System.DateTime.Now.Millisecond);
		if(Segment.ToInit)
		{
			Segment.Init();
		}
		Segment.InitO(gameObject);

		

		int prevH = 1;
		Segs.Add(new Segment(SegmentType.START,0,12, -6 , 0));
		Segs[0].GenerateSegment();
		float Difficulty = 0f;
		for(int k = 1; k <= NumberOfSegments; k++)
		{
			if(k%15 == 0)
			{
				Difficulty = (float)k/(DifficultyDivisor*NumberOfSegments);
			}
			Segs.Add(new Segment(SegmentType.STANDARD,Difficulty,6, k*6 , prevH));
			prevH = Segs[k].GenerateSegment() +1 ;
			if(prevH >3)
			{
				prevH=3;
			}
		}
		Segs.Add(new Segment(SegmentType.END,0,6, (NumberOfSegments+1)*6 , prevH));
		prevH = Segs[(NumberOfSegments+1)].GenerateSegment() +1 ;
		//StaticBatchingUtility.Combine(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
