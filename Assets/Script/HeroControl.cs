using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour {

	// Use this for initialization
	private bool Run = false;
	private bool LeftStrafe = false;
	private bool RightStrafe = false;
	private bool JumpUp = false;
	private Animator anim;
	private AnimatorStateInfo state;
	private TriggerArea ta;
	private bool InAction = false;
	public GameObject Test;
	private bool Matched = false;
	public GameObject Grounder;
	public GameObject SubCamera;
	public GameObject MagicGlass;
	public Material CameraMaterial;
	private float ConstantValue = 220.69695f;
	private bool MagicIsShittered = false;
	private GameObject CurrentMagicGlass;
	public bool Magic = false;
	private bool MagicAnimationStart = false;
	private bool MagicRotate = false;
	public Transform StartPoint;


	private bool isFirstWalk = true;
	private bool isGlassOpen = false;

	public AudioSource footstep;
	public AudioSource glassOpen;

	void Start () {
		anim = GetComponent<Animator>();
		state = anim.GetCurrentAnimatorStateInfo(0);
		Cursor.visible = false;
		CurrentMagicGlass = Instantiate(MagicGlass,new Vector3(100,100,100),Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//SubCamera.transform.rotation = Camera.main.transform.rotation;

//		RaycastHit hit;
//		Debug.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * 100,Color.red);
//		if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, Mathf.Infinity))
//		{
//			
//			
//			//print (Vector3.Angle(hit.transform.forward,this.transform.forward));
//			
//		
//			
//		}

		if(this.transform.position.y < -7)
		{
			this.transform.position = StartPoint.position;
		}

		if(InAction)
		{	
			if(state.IsName("Locomotion"+"."+ta.AnimationName))
			{
				Grounder.GetComponent<Collider>().enabled = false;
				this.GetComponent<Rigidbody>().useGravity = false;
				if(ta.HasMatchTargetion && !Matched)
				{
//					this.GetComponent<MouseLook>().enabled = false;
					Matched = true;
					Vector3 Pos = ProjectPoint(ta.MatchingObject.position,ta.MatchPointStart.position,ta.MatchPointEnd.position);
					Test.transform.position = Pos;
					anim.MatchTarget(Pos, this.transform.rotation, ta.avatarTarget , new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), ta.MatchStartTime, ta.MatchTargetTime);
				}
				anim.SetBool(ta.AnimationName,false);
			}

		}
		else
		{
			if(Input.GetMouseButtonUp(0))
			{
				glassOpen.Stop();
				shatter.stopIt = false;
				isGlassOpen = false;
			}

			if(Input.GetKey(KeyCode.Mouse0) && !shatter.stopIt)
			{

				if(!isGlassOpen)
				{
					isGlassOpen = true;
					glassOpen.loop = false;
					glassOpen.Play();
				}


				if(ColorBars.redAmount > 0.1f && ColorBars.circularDummy == 0 || ColorBars.greenAmount > 0.1f && ColorBars.circularDummy == 1 || ColorBars.blueAmount > 0.1f && ColorBars.circularDummy == 2 )
				{
					CameraControllercs.desiredDistance -= Time.deltaTime * 2;
					if(CurrentMagicGlass != null)
					{
						Camera.main.GetComponent<MouseLook>().enabled = true;
//						this.GetComponent<MouseLook>().enabled = false;
						Camera.main.GetComponent<CameraControllercs>().yMinLimit = -100;
						Camera.main.GetComponent<CameraControllercs>().yMaxLimit = 27;
						Camera.main.GetComponent<CameraControllercs>().rotationDampening = 0;
						CurrentMagicGlass.transform.parent = Camera.main.transform;
						SubCamera.transform.LookAt(CurrentMagicGlass.transform);
						anim.SetBool("Magic",true);
						Magic = true;
						Vector3 Pos = this.transform.position + this.transform.forward * 2;
						//CurrentMagicGlass.transform.position = new Vector3(Pos.x,Pos.y + 2,Pos.z);
						//CurrentMagicGlass.transform.rotation = Camera.main.transform.rotation;
						if(!MagicRotate)
						{
							MagicRotate = true;
							CurrentMagicGlass.transform.Rotate(90,0,0);
						}

						if(ColorBars.circularDummy == 0)
						{
							if(ColorBars.redAmount >= 0)
								ColorBars.redAmount -= Time.deltaTime / 10;
						}
						else if(ColorBars.circularDummy == 1)
						{
							if(ColorBars.greenAmount >= 0)
								ColorBars.greenAmount -= Time.deltaTime / 10;
						}
						else if(ColorBars.blueAmount >= 0)
						{
							ColorBars.blueAmount -= Time.deltaTime / 10;
						}
						                     
	//					float dist = ProjectPointDistance(SubCamera.transform.position,CurrentMagicGlass.transform.position,-CurrentMagicGlass.transform.forward * 50);
	//					SubCamera.camera.fieldOfView = ConstantValue / dist;
					}
					else
					{
						MagicIsShittered = true;
						Vector3 Pos = this.transform.position + this.transform.forward * 2;
						CurrentMagicGlass = Instantiate(MagicGlass,new Vector3(Pos.x,Pos.y + 2,Pos.z),Quaternion.identity) as GameObject;
						CurrentMagicGlass.transform.LookAt(Camera.main.transform);
					}
				}
				else
				{
					anim.SetBool("Magic",false);
					Magic = false;


					if(CurrentMagicGlass != null)
						Destroy(CurrentMagicGlass);
				}
			}
			else
			{	
				Magic = false;
				MagicRotate = false;
//				Camera.main.GetComponent<MouseLook>().enabled = false;
				this.GetComponent<MouseLook>().enabled = true;
				Camera.main.GetComponent<CameraControllercs>().yMinLimit = 13;
				Camera.main.GetComponent<CameraControllercs>().yMaxLimit = 18;
				Camera.main.GetComponent<CameraControllercs>().rotationDampening = 3;
				if(ColorBars.redAmount <= 1)
					ColorBars.redAmount += Time.deltaTime / 30;
				if(ColorBars.greenAmount <= 1)
					ColorBars.greenAmount += Time.deltaTime / 30;
				if(ColorBars.blueAmount <= 1)
					ColorBars.blueAmount += Time.deltaTime / 30;
				CameraControllercs.desiredDistance += Time.deltaTime * 2;
				anim.SetBool("Magic",false);
				anim.SetFloat("MagicWalk",-1);
				if(CurrentMagicGlass != null)
					Destroy(CurrentMagicGlass);
				if(MagicIsShittered)
				{
					MagicIsShittered = false;
					CurrentMagicGlass = Instantiate(MagicGlass,new Vector3(100,100,100),Quaternion.identity) as GameObject;
				}
				else if(CurrentMagicGlass != null && CurrentMagicGlass.transform.position != new Vector3(100,100,100))
					CurrentMagicGlass.transform.position = new Vector3(100,100,100);
			}
		}

		if(Input.GetKey(KeyCode.W))
		{
			if(anim.GetBool("Magic"))
			{
				Grounder.GetComponent<AudioSource>().Play();
				Run = false;
				float magicWalk = anim.GetFloat("MagicWalk");
				magicWalk += 0.01f;

				if(magicWalk < 1)
					anim.SetFloat("MagicWalk",magicWalk);
			
				footstep.pitch = 1.0f;
			}
			else
			{
				if (isFirstWalk){
					footstep.loop = true;
					footstep.Play();
					isFirstWalk = false;
				}
				Run = true;
			}
		}
		else
		{
			float magicWalk = anim.GetFloat("MagicWalk");
			magicWalk -= 0.01f;
			
			if(magicWalk > -1)
				anim.SetFloat("MagicWalk",magicWalk);
			Run = false;
			isFirstWalk = true;
			footstep.Stop();
		}

		if(Input.GetKey (KeyCode.D))
		{
			RightStrafe = true;
			LeftStrafe = false;
		}
		else
			RightStrafe = false;

		if(Input.GetKey (KeyCode.A))
		{

			LeftStrafe = true;
			RightStrafe = false;
		}
		else
			LeftStrafe = false;



	}

	void EndAnimation()
	{
		InAction = false;
		Matched = false;
		Grounder.GetComponent<Collider>().enabled = true;
		this.GetComponent<Rigidbody>().useGravity = true;
		this.GetComponent<MouseLook>().enabled = true;
	}

	void FixedUpdate()
	{
		state = anim.GetCurrentAnimatorStateInfo(0);
		anim.SetBool("Run",Run);
		anim.SetBool("RightStrafe",RightStrafe);
		anim.SetBool("LeftStrafe",LeftStrafe);
		if(ta != null && InAction)
		{
			anim.SetBool(ta.AnimationName,true);
		}
	}

	void OnTriggerStay(Collider otherCollider)
	{
		// Layer 8 Obstacle
		// Layer 12 Selector
		if(otherCollider.tag == "ActionTrigger" && !InAction)
		{

			RaycastHit hit;
			Debug.DrawRay(new Vector3(this.transform.position.x,this.transform.position.y + 1,this.transform.position.z), this.transform.forward , Color.red);
			if(Physics.Raycast(new Vector3(this.transform.position.x,this.transform.position.y + 1,this.transform.position.z ), this.transform.forward, out hit, 1 , 1 << 12))
			{
				if(Input.GetKeyDown(KeyCode.Mouse0))
				{	
					print (hit.transform.gameObject.name);
					ta = otherCollider.gameObject.GetComponent<TriggerArea>();
					InAction = true;
				}
			}
		}
	}

	public Vector3 ProjectPoint(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 rhs = point - lineStart;
		Vector3 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector3 lhs = vector2;
		if (magnitude > 1E-06f)
		{
			lhs = (Vector3)(lhs / magnitude);
		}
		float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
		return (lineStart + ((Vector3)(lhs * num2)));

	}

	public float ProjectPointDistance(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 rhs = point - lineStart;
		Vector3 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector3 lhs = vector2;
		if (magnitude > 1E-06f)
		{
			lhs = (Vector3)(lhs / magnitude);
		}
		float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
		return Vector3.Distance(lineStart,(lineStart + ((Vector3)(lhs * num2))));
	}

	void OnCollisionEnter(Collision otherCollision)
	{
		if(otherCollision.gameObject.tag == "Cube")
			otherCollision.gameObject.layer = 0;
	}
}
