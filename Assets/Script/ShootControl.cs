using UnityEngine;
using System.Collections;

public class ShootControl : MonoBehaviour {

	// Use this for initialization
	public GameObject trail;
	public Transform TrailStart;
	public GameObject Gun;
	public float RailCharge = 0;
	private GameObject CurrentTrail;
	private float sHeight, sWidth;
	private Vector3 CurrentPos =  Vector3.zero;
	private Vector3 BeforePos = Vector3.zero;
	private Vector3 Velocity = Vector3.zero;
	private float AnimationTime = 0;
	private Animator anim;
	private bool IsAnotherPlayer = false, isTouchedToGround = false;
	private AnimatorStateInfo state;
	private string[] animations = new string[]{"Hit","Fall","Run","RunBack","Jump","Shoot","StrafeLeft","StrafeRight","Idle"};

	void Start () {
		CurrentPos = this.transform.position;
		BeforePos = CurrentPos;
		if (this.gameObject.name != "kartush")
		{
			IsAnotherPlayer = true;
			anim = this.GetComponent<Animator>();
			state = anim.GetNextAnimatorStateInfo(0);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(!IsAnotherPlayer)
		{
			if (Input.GetMouseButton (0)) 
			{
				RailCharge += Time.deltaTime * 10;
				if(RailCharge > 15)
					RailCharge = 15;
			}
			else if(RailCharge > 0)
			{
				sWidth = Screen.width / 2;
				sHeight = Screen.height / 2;
				CurrentTrail = Instantiate(trail,TrailStart.position,Quaternion.identity) as GameObject;
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(sWidth, sHeight, 0));
				RaycastHit hit;
				Vector3 BulletDirection = (ray.direction * 10000) - TrailStart.position;;
				if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.gameObject.tag == "OtherPlayer")
					{
						this.GetComponent<ClientDeneme>().PlayerHit(hit.transform.gameObject,Mathf.Clamp(RailCharge,0,100));
					}
					BulletDirection = hit.point - TrailStart.position;
				}
				this.GetComponent<ClientDeneme>().Fire(TrailStart.position,BulletDirection,RailCharge);
				CurrentTrail.GetComponent<Rigidbody>().AddForce(BulletDirection * 1000, ForceMode.Acceleration);
				RailCharge = 0;
			}
		}
		else
		{
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position,-this.transform.up,out hit,100))
			{
//				print (hit.distance);
				if(hit.distance > 0.1f)
					isTouchedToGround = false;
				else
					isTouchedToGround = true;
			}
			foreach (var item in animations) {
				if(state.IsName("MultiAnim" + "." + item))
				{
					anim.SetBool(item,false);
				}
			}
		
			CurrentPos = this.transform.position;
			Vector3 diff = CurrentPos - BeforePos;
		
			if(diff.magnitude > 0.05)
			{
				Velocity = this.transform.InverseTransformDirection(new Vector3(diff.x / Time.deltaTime, diff.y / Time.deltaTime, diff.z / Time.deltaTime));
			}
			else 
			{
				AnimationTime += Time.deltaTime;
				if(AnimationTime > 0.5f)
				{
					AnimationTime = 0;
					Velocity = Vector3.zero;
				}
			}
			BeforePos = CurrentPos;
		}
	}

	void FixedUpdate()
	{
		if(IsAnotherPlayer)
		{
			state = anim.GetNextAnimatorStateInfo(0);
			anim.SetBool("OnGround",isTouchedToGround);
//			anim.SetBool("Hit",Hit);
	//		print (Velocity);
			if(!isTouchedToGround)
			{
//				if(Velocity.y < -0.1f)
//					anim.SetBool("Fall",true);
			}
			else
			{
				if(Velocity.y > 0.1f)
					anim.SetBool("Jump",true);
				else if(Velocity.z > 0)
				{
	//				print ("Run");
					anim.SetBool("Run",true);
				}
				if(Velocity.z < 0)
				{
	//				print ("RunBack");
					anim.SetBool("RunBack",true);
				}
//				
//				if(Velocity.x < -0.1f && !state.IsName("MultiAnim.StrafeLeft"))
//					anim.SetBool("StrafeLeft",true);
//				if(Velocity.x > 0.1f && !state.IsName("MultiAnim.StrafeRight"))
//					anim.SetBool("StrafeRight",true);

			}
		}
	}

	public void ShootPlayer(Vector3 startPoint, Vector3 direction, float damage)
	{
		anim.SetBool("Shoot",true);
		Debug.DrawRay (startPoint, direction, Color.red,10);
		CurrentTrail = Instantiate(trail,startPoint,Quaternion.identity) as GameObject;
		CurrentTrail.GetComponent<Rigidbody>().AddForce(direction * 1000, ForceMode.Acceleration);
	}

//	void OnCollisionStay (Collision col)
//	{
//		if (IsAnotherPlayer && col.collider.gameObject.tag == "Ground")
//			isTouchedToGround = true;
//	}
}
