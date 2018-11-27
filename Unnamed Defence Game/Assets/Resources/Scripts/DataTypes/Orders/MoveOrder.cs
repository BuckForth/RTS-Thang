using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveOrder : Order
{
	public Vector3 targetLocation;

	public MoveOrder(Vector3 endpoint)
	{
		targetLocation = endpoint;
	}
}
