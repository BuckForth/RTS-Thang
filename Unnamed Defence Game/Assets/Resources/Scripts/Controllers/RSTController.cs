using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RSTController : MonoBehaviour 
{
	public List<EntityController> selected = new List<EntityController> ();

	private Vector2 selectionStart = new Vector2();
	private Vector2 selectionEnd = new Vector2();
	private Camera cam;
	// Use this for initialization
	void Start ()
	{
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.Mouse0))
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 10.0f;
				mousePos = cam.ScreenToWorldPoint (mousePos);


				Ray ray = new Ray (mousePos, Vector3.down);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit))
				{
					selectionStart = new Vector2(hit.point.x,hit.point.z);
					Debug.Log ("Selection Start At: " + selectionStart);

				}
			}
		}
		else if (Input.GetKeyUp (KeyCode.Mouse0))
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10.0f;
			mousePos = cam.ScreenToWorldPoint (mousePos);


			Ray ray = new Ray (mousePos, Vector3.down);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (ray, out hit))
			{
				selectionEnd = new Vector2(hit.point.x,hit.point.z);
				Debug.Log ("Selection End At: " + selectionEnd);
			}

			Vector3[] boxCorns = new Vector3[4];
			boxCorns [0] = new Vector3 (selectionStart.x, 40, selectionStart.y);
			boxCorns [1] = new Vector3 (selectionEnd.x, 40, selectionStart.y);
			boxCorns [3] = new Vector3 (selectionStart.x, 40, selectionEnd.y);
			boxCorns [2] = new Vector3 (selectionEnd.x, 40, selectionEnd.y);
			Debug.DrawLine (boxCorns [0], boxCorns [1],Color.red);
			Debug.DrawLine (boxCorns [2], boxCorns [1],Color.blue);
			Debug.DrawLine (boxCorns [2], boxCorns [3],Color.blue);
			Debug.DrawLine (boxCorns [0], boxCorns [3],Color.red);
			Debug.Break ();
		}
		else
		{
			if (Input.GetKeyDown (KeyCode.Mouse1))
			{
				DeselectAll ();
			}
		}
	}

	private void DeselectAll()
	{
		selected = new List<EntityController> ();
	}
}
