using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackOrder : Order
{
	public EntityController target;

	public AttackOrder(EntityController attackTarget)
	{
		target = attackTarget;
	}
}
