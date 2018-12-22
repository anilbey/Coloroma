using UnityEngine;
using System.Collections;

public class LayerModification : MonoBehaviour {
	
	public GameObject redBullet,greenBullet,blueBullet;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (ColorBars.circularDummy == 0)
		{
			this.GetComponent<ShootControl>().trail = redBullet;
			//			this.GetComponent<ShootControl>().RailCharge
			
			
			//			GameObject.Find("BulletRed").GetComponent<TrailRenderer>().endWidth = (float) (0.005 + this.GetComponent<ShootControl>().RailCharge * 0.013);
			
			
		}
		else if (ColorBars.circularDummy == 1)
		{
			this.GetComponent<ShootControl>().trail = greenBullet;
			
		}
		else if (ColorBars.circularDummy == 2)
		{
			this.GetComponent<ShootControl>().trail = blueBullet;
			
		}
		this.GetComponent<ShootControl>().trail.GetComponent<TrailRenderer>().endWidth = (float) (0.005 + this.GetComponent<ShootControl>().RailCharge * 0.023); //hashing the endWidth between 0.005 and 0.2
		
	}
}