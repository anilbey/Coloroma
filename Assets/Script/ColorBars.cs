using UnityEngine;
using System.Collections;

public class ColorBars : MonoBehaviour {
	
	
	public GameObject fadeSmoke;
	
	public static int circularDummy = 0;
	
	public Texture2D black;
	
	// background image that is 256 x 32
	
	
	public Texture2D point = null;
	
	private Texture2D pointRed   = null;
	private Texture2D pointBlue  = null;
	private Texture2D pointGreen = null;
	
	private Texture2D bgImageRed  = null; 
	private Texture2D bgImageBlue = null;
	private Texture2D bgImageGreen = null;
	
	public Texture2D AbgImageRed  = null; 
	public Texture2D AbgImageBlue = null;
	public Texture2D AbgImageGreen = null;
	
	public Texture2D red;
	public Texture2D blue;
	public Texture2D green;
	// foreground image that is 256 x 32
	public Texture2D redfgImage = null;
	public Texture2D greenfgImage = null; 
	public Texture2D bluefgImage = null; 
	
	// a float between 0.0 and 1.0
	public static float redAmount = 1.0f;
	public static float greenAmount = 1.0f; 
	public static float blueAmount = 1.0f; 
	
	private float switchTime;
	
	
	
	void OnGUI () {
		// Create one Group to contain both images
		// Adjust the first 2 coordinates to place it somewhere else on-screen
		GUI.BeginGroup (new Rect (5,5,280,100));
		
		
		
		// Draw the background image
		GUI.Box (new Rect (0,0,256,25), bgImageRed);
		GUI.Box (new Rect (260,0,20,20), pointRed);
		GUI.Box (new Rect (0,30,256,25), bgImageBlue);
		GUI.Box (new Rect (260,30,20,20), pointGreen);
		GUI.Box (new Rect (0,60,256,25), bgImageGreen);
		GUI.Box (new Rect (260,60,20,20), pointBlue);
		
		// Create a second Group which will be clipped
		// We want to clip the image and not scale it, which is why we need the second Group
		GUI.BeginGroup (new Rect (0,0,redAmount * 256, 25));
		
		// Draw the foreground image
		GUI.Box (new Rect (0,0,256,25), redfgImage);
		GUI.EndGroup ();
		GUI.BeginGroup (new Rect (0,0,greenAmount * 256, 55));
		
		// Draw the foreground image
		GUI.Box (new Rect (0,30,256,25), greenfgImage);
		GUI.EndGroup ();
		GUI.BeginGroup (new Rect (0,0,blueAmount * 256, 85));
		
		// Draw the foreground image
		GUI.Box (new Rect (0,60,256,25), bluefgImage);
		GUI.EndGroup ();
		
		
		// End both Groups
		
		
		GUI.EndGroup ();
	}
	
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && (float) Time.realtimeSinceStartup - switchTime > 1.5 ) // back
		{
			circularDummy += 2;
			circularDummy %= 3;

			switchTime = (float) Time.realtimeSinceStartup;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && (float) Time.realtimeSinceStartup - switchTime > 1.5) // forward
		{
			++circularDummy;
			circularDummy %= 3;
			switchTime = (float) Time.realtimeSinceStartup;

		}
		else if(Input.GetKey(KeyCode.Alpha1) && (float) Time.realtimeSinceStartup - switchTime > 1.5)
		{	circularDummy = 0;

			switchTime = (float) Time.realtimeSinceStartup;
		}
		else if(Input.GetKey(KeyCode.Alpha2) && (float) Time.realtimeSinceStartup - switchTime > 1.5){
			circularDummy = 1;

			switchTime = (float) Time.realtimeSinceStartup;
		}
		else if(Input.GetKey(KeyCode.Alpha3) && (float) Time.realtimeSinceStartup - switchTime > 1.5){
			circularDummy = 2;

			switchTime = (float) Time.realtimeSinceStartup;
		}
		assignColors();
		
	}
	
	void assignColors()
	{
		if (circularDummy == 0)
		{
			redfgImage = AbgImageRed;
			greenfgImage = green; 
			bluefgImage = blue;
			pointRed = point;
			pointGreen = null;
			pointBlue = null;
		}
		else if (circularDummy == 1)                                          
		{
			redfgImage = red;                 
			greenfgImage = AbgImageGreen;
			bluefgImage = blue;
			pointGreen = point;
			pointRed = null;
			pointBlue = null;
			
		}
		else if (circularDummy == 2)
		{
			redfgImage = red;
			greenfgImage = green;
			bluefgImage = AbgImageBlue;
			pointBlue = point;
			pointRed = null;
			pointGreen = null;
		}
		
	}

}