using UnityEngine;
using System.Collections;

public class KillMeOverTime : MonoBehaviour {
	
	float startTime;
	// Use this for initialization
	void Start () {
		startTime = (float) Time.time ;
	}
	
	// Update is called once per frame
	void Update () {
		if ((float) Time.time - startTime >= 0.65)
			Destroy(this.gameObject);
	}
}