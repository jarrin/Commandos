/// <summary>
/// Move player.
/// Управление мышью персонажем
/// Вещается на персонажа
/// </summary>
using UnityEngine;
using System.Collections;
using Pathfinding;

public class MovePlayer : MonoBehaviour
{
	public float stopStart = 1.5f, speed = 5f, rotationSpeed = 100f, heightPlayer = 1f;
	private float mag, angleToTarget;
	private Ray ray;
	private RaycastHit hit;
	private Vector3 dir;
	private Vector3 target = new Vector3 ();
	private Vector3 lastTarget = new Vector3 ();
	public AnimationClip a_Idle;
	public float a_IdleSpeed = 1;
	public AnimationClip a_Walk;
	public float a_WalkSpeed = 2;
	private bool walk;
	private bool CanTouchUp = false;
	Seeker seeker;
	private void Start ()
	{
		seeker = GetComponent<Seeker>();
		animation [a_Idle.name].speed = a_IdleSpeed;
		animation [a_Walk.name].speed = a_WalkSpeed;
		animation.CrossFade (a_Idle.name);
	}
	 public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
    }
	private void Update ()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButtonUp (0)) {
			//if(Input.touches.Length==1)
			
			ray = UnityEngine.Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 10000.0f)) {
				target = hit.point;
				seeker.StartPath (transform.position,target, OnPathComplete);
			}
		}
#else
		if (Input.touches.Length == 1) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				CanTouchUp = true;
			}
			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				CanTouchUp = false;
			}
			if (Input.GetTouch (0).phase == TouchPhase.Ended && CanTouchUp) {
				CanTouchUp = false;
				ray = UnityEngine.Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 10000.0f)) {
					target = hit.point;
				}
			}
		}
#endif
		LookAtThis ();
		
		//MoveTo ();
	}

	private void CalculateAngle (Vector3 temp)
	{
		dir = new Vector3 (temp.x, transform.position.y, temp.z) - transform.position;
		angleToTarget = Vector3.Angle (dir, transform.forward);
	}

	private void LookAtThis ()
	{
		if (target != lastTarget) {
			CalculateAngle (target);
			if (angleToTarget > 3) {
				transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (dir), rotationSpeed * UnityEngine.Time.deltaTime);
			}
		}
	}

	private void MoveTo ()
	{
		if (target != lastTarget) {
			if ((transform.position - target).sqrMagnitude > heightPlayer + 0.1f) {
				if (!walk) {
					animation.CrossFade (a_Walk.name, 0.4f);
					walk = true;
					Debug.Log ("Walk");
				}
				mag = (transform.position - target).magnitude;
				transform.position = Vector3.MoveTowards (transform.position, target, mag > stopStart ? speed * UnityEngine.Time.deltaTime : Mathf.Lerp (speed * 0.5f, speed, mag / stopStart) * UnityEngine.Time.deltaTime);
				ray = new Ray (transform.position, -Vector3.up);
				if (Physics.Raycast (ray, out hit, 1000.0f)) {
					transform.position = new Vector3 (transform.position.x, hit.point.y + heightPlayer, transform.position.z);
				}
			} else {
				lastTarget = target;
				if (walk) {
					Debug.Log ("Stop");
					animation.CrossFade (a_Idle.name);
					walk = false;
				}
			}
		}
	}
} 