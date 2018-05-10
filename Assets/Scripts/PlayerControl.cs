using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public float JumpForce = 10f;
	public int trapScoreMult = 500;
	float x = 0f;
	public int LP = 5;
	public int MLP = 5;
	public float dx = 0.01f;
	public float maintainForce = 0.02f;
	bool prevL = false;
	bool prevR  = false;
	Rigidbody2D body;
	public float MoveCoeff = 1.5f;
	// Use this for initialization
	public bool CanJump = false;
	float lifeMpos;
	float lifeMsize;
	float prog; // the life, in fraction
	float prevY; // the previous position of the life mask
	GameObject LifeMask;
	void Start () {
		Time.timeScale = 3;
		LifeMask = GameObject.Find("Life");
		body = gameObject.GetComponent<Rigidbody2D>();
		lifeMpos = LifeMask.transform.localPosition.y;
		lifeMsize = LifeMask.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Horizontal")<0)
		{
			if(prevL)
			{
				if(x<=1f)
				{
					x+=dx*Time.deltaTime;
				}
			}
			else
			{
				x=0f;
			}
			prevL = true;
			body.AddForce(Time.deltaTime*MoveCoeff*Vector2.left*(Mathf.Pow(x-2,2)+1));
		}
		else
		{
			prevL= false;
		}
		if(Input.GetAxis("Horizontal")>0)
		{
			if(prevR)
			{
				if(x<=1f)
				{
					x+=dx*Time.deltaTime;
				}
			}
			else
			{
				x=0f;
			}
			prevR = true;
			body.AddForce(Time.deltaTime*MoveCoeff*Vector2.right*(Mathf.Pow(x-2,2)+1));
		}
		else
		{
			prevR= false;
		}
		RaycastHit2D hit = Physics2D.Raycast((Vector2)body.transform.position - new Vector2(0,0.2f), Vector2.down);
		Trap a = hit.transform.gameObject.GetComponent<Trap>();
		bool isLava = (a!=null)?(a.DontDisableAfterHit == true):false; // jumping on lava ? If you allow that, why do you place lava at all ?
		if(hit.distance<Mathf.Epsilon && !isLava)
		{
			if((Input.GetAxis("Jump")>0) && CanJump)
			{
				body.AddForce(JumpForce*Vector2.up);
				CanJump = false;
			}
		} else if((Input.GetAxis("Jump")>0) && body.velocity.y>= 0.01f)
		{
			body.AddForce(maintainForce*Vector2.up);
		}
		/// snap to grid
		float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
		float pixelsInBlock = (float)(Screen.width)/(float)(Camera.main.GetComponent<Assets.Pixelation.Scripts.Pixelation>().BlockCount);
		float worldInBlock = pixelsInBlock/worldToPixels;
		GameObject spr = GameObject.Find("SpritePerso");
		spr.transform.position = new Vector3(
			Mathf.Floor(transform.position.x/worldInBlock)*worldInBlock,
			Mathf.Floor(transform.position.y/worldInBlock)*worldInBlock,
			Mathf.Floor(transform.position.z/worldInBlock)*worldInBlock
		);
	}
	void Die()
	{
		Initiate.Fade("dieScreen",Color.black,2f);
	}
	IEnumerator LifeToTarget() {
		
		for (float f = 0f; f <= 1.001f; f += 0.1f) {
			float x = f;
			float newY = lifeMpos - prog*lifeMsize;
			//Debug.Log("To "+prog.ToString()+"; lerp to : " + newY.ToString() + "; Factor : "+ x.ToString() );
			Vector2 newPos = LifeMask.transform.localPosition;
			newPos.y = Mathf.Lerp(prevY,newY,x);
			LifeMask.transform.localPosition = newPos;
			yield return null;
		}
	}
	void RefreshLife()
	{
		prog = 1f - (float)LP/(float)MLP;
		prevY = LifeMask.transform.localPosition.y;
		StartCoroutine("LifeToTarget");
	}
	void OnCollisionEnter2D (Collision2D col)
	{
		bool savedJumpAb = CanJump;
		CanJump = true;
		if(col.gameObject.tag == "Trigger")
		{
			GameObject.Find("Crusher").GetComponent<Crusher>().Enable();
		}
		if(col.gameObject.GetComponent<Trap>() != null)
		{
			int HitLP = col.gameObject.GetComponent<Trap>().Hit();
			LP += HitLP;
			GameObject.Find("Game").GetComponent<Permanent>().Score += trapScoreMult*HitLP;
			if (HitLP <= -5)
			{
				Die();
			}
			if (LP>MLP)
			{
				LP = MLP;
			}
			if (LP <= 0)
			{
				LP = 0;
				if(GameObject.Find("Game").GetComponent<Permanent>().Anks > 1)
				{
					LP = MLP;
					GameObject.Find("Game").GetComponent<Permanent>().Anks -=1;
				}
				else
				{
					Die();
				}
			}
			RefreshLife();
		}
		if(col.gameObject.GetComponent<Bonus>() != null)
		{
			string bonus = col.gameObject.GetComponent<Bonus>().Hit();
			Debug.Log(bonus);
			if (bonus == "ank")
			{
				GameObject.Find("Game").GetComponent<Permanent>().Anks += 1;
			}
			CanJump = savedJumpAb;
		}

	}
	void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Trigger")
		{
			GameObject.Find("Crusher").GetComponent<Crusher>().Enable();
		}
    }
}
