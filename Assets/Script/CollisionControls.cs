using UnityEngine;
using System.Collections;

public class CollisionControls : MonoBehaviour {


    private HeroControl control;

    private int BlueCullingMask;
    private int RedCullingMask;
    private int GreenCullingMask;
    private int OldCullingMask;

	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(14, 9, true);
        Physics.IgnoreLayerCollision(14, 10, true);
        Physics.IgnoreLayerCollision(14, 11, true);
       
        control = GetComponent<HeroControl>();
        OldCullingMask = RedCullingMask = BlueCullingMask = GreenCullingMask = control.SubCamera.GetComponent<Camera>().cullingMask;
        RedCullingMask &= ~(1 << 10);
        RedCullingMask &= ~(1 << 11);
        BlueCullingMask &= ~(1 << 9);
        BlueCullingMask &= ~(1 << 10);
        GreenCullingMask &= ~(1 << 9);
        GreenCullingMask &= ~(1 << 11);
	}
	
	// Update is called once per frame
	void Update () {

            // Red
            if (ColorBars.circularDummy == 0)
            {
                Physics.IgnoreLayerCollision(13, 10);
                Physics.IgnoreLayerCollision(13, 11);
                Physics.IgnoreLayerCollision(13, 9, false);
                Physics.IgnoreLayerCollision(14, 10);
                Physics.IgnoreLayerCollision(14, 11);
                Physics.IgnoreLayerCollision(14, 9, false);
                control.SubCamera.GetComponent<Camera>().cullingMask = RedCullingMask;
            }
            // Green
			else if (ColorBars.circularDummy == 1)
            {
                Physics.IgnoreLayerCollision(13, 9);
                Physics.IgnoreLayerCollision(13, 11);
                Physics.IgnoreLayerCollision(13, 10, false);
                Physics.IgnoreLayerCollision(14, 9);
                Physics.IgnoreLayerCollision(14, 11);
                Physics.IgnoreLayerCollision(14, 10, false);
                control.SubCamera.GetComponent<Camera>().cullingMask = GreenCullingMask;
            }
            // Blue
			else if (ColorBars.circularDummy == 2)
            {
                Physics.IgnoreLayerCollision(13, 9);
                Physics.IgnoreLayerCollision(13, 10);
                Physics.IgnoreLayerCollision(13, 11, false);
                Physics.IgnoreLayerCollision(14, 9);
                Physics.IgnoreLayerCollision(14, 10);
                Physics.IgnoreLayerCollision(14, 11, false);
                control.SubCamera.GetComponent<Camera>().cullingMask = BlueCullingMask;
            }

	}
}
