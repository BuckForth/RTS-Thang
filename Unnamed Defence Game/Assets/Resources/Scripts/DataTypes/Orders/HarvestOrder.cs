using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestOrder : Order
{
	public Bounds harvestTarget;

	public HarvestOrder(Bounds target)
	{
		harvestTarget = target;
	}
}
