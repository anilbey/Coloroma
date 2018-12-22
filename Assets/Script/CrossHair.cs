using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {
	
	bool drawCrosshair = true;
	public static Color crosshairColor = Color.red;
	
	float width = 10;
	float height = 1;
	
	class spreading
	{
		public float spread = 10;
		public float maxSpread = 30;
		public float minSpread = 10;
		public float spreadPerSecond = 15;
		public float decreasePerSecond = 50;
	}
	
	private spreading spread;
	public Texture2D tex;
	public GUIStyle lineStyle;
	
	void Start()
	{
		spread = new spreading();
	}
	
	// Use this for initialization
	void Awake () {
		tex = new Texture2D(1,1);
		SetColor(tex,crosshairColor);
		lineStyle = new GUIStyle();
		lineStyle.normal.background = tex;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(ColorBars.circularDummy == 0) SetColor(tex,Color.red);
		else if (ColorBars.circularDummy == 1) SetColor (tex,Color.green);
		else if (ColorBars.circularDummy == 2) SetColor (tex,Color.blue);
		
		if(Input.GetButton("Fire1")){
			spread.spread += spread.spreadPerSecond * Time.deltaTime;       //Incremente the spread
			//			Fire();
		}else{
			spread.spread -= spread.decreasePerSecond * Time.deltaTime;      //Decrement the spread     
		}
		
		spread.spread = Mathf.Clamp(spread.spread, spread.minSpread, spread.maxSpread); 
	}
	
	void OnGUI(){
		Vector2 centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);
		if(drawCrosshair){
			GUI.Box(new Rect(centerPoint.x - width / 2, centerPoint.y - (height + spread.spread), width, height), "", lineStyle);
			GUI.Box(new Rect(centerPoint.x - width / 2, centerPoint.y + spread.spread, width, height), "", lineStyle);
			GUI.Box(new Rect(centerPoint.x + spread.spread, (centerPoint.y - width / 2), height , width), "", lineStyle);
			GUI.Box(new Rect(centerPoint.x - (height + spread.spread), (centerPoint.y - width / 2), height , width), "", lineStyle);
		}   
	}
	
	void SetColor(Texture2D myTexture, Color myColor){
		for (int y = 0; y < myTexture.height; ++y){
			for (int x = 0; x < myTexture.width; ++x){
				myTexture.SetPixel(x, y, myColor);
			}
		}
		
		myTexture.Apply();
	}
	
}