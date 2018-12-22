using UnityEngine;
using System.Collections;

public class AnilController : MonoBehaviour {

	public float forwardSpeed;
	public float backwardSpeed;
	public float sideSpeed;

	private bool isTouchedToGround;

	private bool isPressedKeyA;
	private bool isPressedKeyS;
	private bool isPressedKeyW;
	private bool isPressedKeyD;

	private float speedConstant = 10f;
	

	// Use this for initialization
	void Start () {
	
		isTouchedToGround = true;
		isPressedKeyA = isPressedKeyD = isPressedKeyS = isPressedKeyW = false;
	}

	// Update is called once per frame
	void Update () {
	
		if (!isTouchedToGround) 
		{
			this.GetComponent<Rigidbody>().drag = 0;
		}
		else
			this.GetComponent<Rigidbody>().drag = 5;

//		print (this.transform.InverseTransformDirection (rigidbody.velocity));

		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && isTouchedToGround)
		{
//			rigidbody.AddForce(0,0,0,ForceMode.
			 
			//this.gameObject.transform.Translate(-1 * Time.deltaTime * sideSpeed,0,0);
//			this.gameObject.rigidbody.AddRelativeForce(-1*sideSpeed,0,0);
//			if (isPressedKeyW) speedConstant = 0.5f; else speedConstant = 1f;
			GetComponent<Rigidbody>().AddForce (transform.right * -1 * speedConstant * sideSpeed);

			//isPressedKeyA = true;

		}
		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isTouchedToGround)
		{
			//this.gameObject.transform.Translate(0,0,forwardSpeed * Time.deltaTime);
//			this.gameObject.rigidbody.AddRelativeForce(0,0,forwardSpeed);
//			if (isPressedKeyA || isPressedKeyD) speedConstant = 0.5f; else speedConstant = 1f;
			GetComponent<Rigidbody>().AddForce (transform.forward * speedConstant * forwardSpeed);
			isPressedKeyW = true;
		}
		if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && isTouchedToGround)
		{
			//this.gameObject.transform.Translate(sideSpeed * Time.deltaTime,0,0);
//			if (isPressedKeyW) speedConstant = 0.5f; else speedConstant = 1f;
//			this.gameObject.rigidbody.AddRelativeForce(sideSpeed*speedConstant,0,0,ForceMode.Impulse);
			GetComponent<Rigidbody>().AddForce (transform.right * sideSpeed * speedConstant);
			isPressedKeyD = true;
		}
		if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isTouchedToGround)
		{
			sideSpeed = backwardSpeed;
//			this.gameObject.transform.Translate(0,0,-1*backwardSpeed * Time.deltaTime);
//			if (isPressedKeyA || isPressedKeyD) speedConstant = 0.5f; else speedConstant = 1f;
//			this.gameObject.rigidbody.AddRelativeForce(0,0,-1*speedConstant*backwardSpeed);
			isPressedKeyS = true;
			GetComponent<Rigidbody>().AddForce (transform.forward*-1  *forwardSpeed * speedConstant);
		}
		if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
		{
			sideSpeed = forwardSpeed;
			isPressedKeyS = false;
		}
		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
		{

			isPressedKeyW = false;
		}
		if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
		{

			isPressedKeyD = false;
		}
		if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
		{

			isPressedKeyA = false;
		}



		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isTouchedToGround)
			{
				if (isPressedKeyW && isPressedKeyA)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(-200f,300f,200f);
				}
				else if (isPressedKeyW && isPressedKeyD)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(200f,300f,200f);
				}

				else if (isPressedKeyS && isPressedKeyD)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(200f,300f,-100f);
				}
				else if (isPressedKeyS && isPressedKeyA)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(-200f,300f,-100f);
				}
				else if (isPressedKeyW)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0,300f,400f);
				}
				else if (isPressedKeyA)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(-300f,300f,0);
				}
				else if(isPressedKeyD)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(300f,300f,0);
				}
				else if (isPressedKeyS)
				{
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0,300f,-100f);
				}
				else
					this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0f,400f,0f);
				isTouchedToGround = false;
			}

		}
		    
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.collider.gameObject.tag == "Ground")
			isTouchedToGround = true;
	}

}
