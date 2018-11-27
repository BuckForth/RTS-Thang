using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuebarController : MonoBehaviour 
{
	private float value = 1f;
	public Image foreGround;
	public float scaleMax = 1f;

	private void updateBar()
	{
		foreGround.transform.localScale = new Vector3 (value * scaleMax, foreGround.transform.localScale.y, foreGround.transform.localScale.z);
	}
	public void setValue(float newValue)
	{
		value = newValue;
		updateBar ();
	}
	public float getValue()
	{
		return value;
	}
}
