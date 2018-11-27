using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : Entity
{
	protected Bounds activeHarvestRegion = new Bounds (Vector3.zero, Vector3.zero);
	protected VectorInt3 harvestTarget = new VectorInt3();
}
