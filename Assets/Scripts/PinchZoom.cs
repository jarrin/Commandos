/*/
 * Script by Devin Curry
 * www.Devination.com
 * www.youtube.com/user/curryboy001
 * Please like and subscribe if you found my tutorials helpful :D
/*/
using UnityEngine;
using System.Collections;
 
public class PinchZoom : TouchLogic
{
	public float zoomSpeed = 5.0f;
	public float MIN_ZOOM = 10.0f;
	public float MAX_ZOOM = 50.0f;
	
	//buckets for caching our touch positions
	private Vector2 currTouch1 = Vector2.zero,
 lastTouch1 = Vector2.zero,
 currTouch2 = Vector2.zero,
 lastTouch2 = Vector2.zero;
 
	//used for holding our distances and calculating our zoomFactor
	private float currDist = 0.0f,
 lastDist = 0.0f,
 zoomFactor = 0.0f;
 
	void OnTouchMovedAnywhere ()
	{
		Zoom ();
	}

	void OnTouchStayedAnywhere ()
	{
		Zoom ();
	}

	void OnTouchBeganAnywhere ()
	{
		
	}

	void OnTouchEndedAnywhere ()
	{
		
	}

	//find distance between the 2 touches 1 frame before & current frame
	//if the delta distance increased, zoom in, if delta distance decreased, zoom out
	void Zoom ()
	{
		//Caches touch positions for each finger
		switch (TouchLogic.currTouch) {
		case 0://first touch
			currTouch1 = Input.GetTouch (0).position;
			lastTouch1 = currTouch1 - Input.GetTouch (0).deltaPosition;
			break;
		case 1://second touch
			currTouch2 = Input.GetTouch (1).position;
			lastTouch2 = currTouch2 - Input.GetTouch (1).deltaPosition;
			break;
		}
  
		//finds the distance between your moved touches
		//we dont want to find the distance between 1 finger and nothing
		if (Input.touchCount == 2) {
			if (TouchLogic.currTouch >= 1) {
				currDist = Vector2.Distance (currTouch1, currTouch2);
				lastDist = Vector2.Distance (lastTouch1, lastTouch2);
			} else {
				currDist = 0.0f;
				lastDist = 0.0f;
			}
			//Calculate the zoom magnitude
			zoomFactor = Mathf.Clamp (lastDist - currDist, -30.0f, 30.0f);
			Debug.Log ("zoomFactor: " + currTouch);
			//apply zoom to our camera
			Camera.mainCamera.transform.Translate (Vector3.back * zoomFactor * zoomSpeed * Time.deltaTime);
			Vector3 _temp = Camera.mainCamera.transform.position;
			float _y = Mathf.Clamp (_temp.y, MIN_ZOOM, MAX_ZOOM);
			_temp.y = _y;
			Camera.mainCamera.transform.position = _temp;
		}
	}
}