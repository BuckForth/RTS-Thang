using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity 
{
	protected string entityName = "";
	protected int healthCap = 10;
	protected float movement = 0f;
	protected HoldableObject placeholderforitemdroporvaluegain;

	public bool canMove = true;
	public bool canHarvest = false;
	public bool canAttack = false;


	public virtual GameObject SpawnEntity(Vector3 location)
	{
		GameObject rVal = new GameObject (entityName);
		rVal.transform.position = location;
		EntityController control = rVal.AddComponent<EntityController> ();
		control.health = healthCap;

		return rVal;
	}

	public virtual bool Act(EntityController controller)
	{
		bool rVal = true;
		if (controller.orders.Count > 0)
		{
			Order currOrder = controller.orders.Peek();
			if (currOrder.GetType() == typeof(MoveOrder))
			{
				bool orderComplete = Move (controller);
				rVal = orderComplete;
			}
		}

		return rVal;
	}

	public virtual void Hurt(EntityController conThis, float damage)
	{

	}
		
	public virtual bool Move (EntityController controller)//returns true when at target location
	{
		NavMeshAgent agent = controller.GetComponent<NavMeshAgent> ();
		if (agent.destination != ((MoveOrder)(controller.orders.Peek ())).targetLocation)
		{
			agent.SetDestination (((MoveOrder)(controller.orders.Peek ())).targetLocation);
		}
		agent.isStopped = false;
		bool rVal = ((controller.transform.position - ((MoveOrder)(controller.orders.Peek ())).targetLocation).magnitude < 0.5f);
		if (rVal)
		{
			//agent.isStopped = true;
		}
		return rVal;
		/*
		bool rVal = true;
		GameObject go = controller.gameObject;
		MoveOrder order = controller.orders.Peek () as MoveOrder;
		Vector2 goPos = new Vector2 (go.transform.position.x, go.transform.position.z);
		Vector2 orderPos = new Vector2 (order.targetLocation.x, order.targetLocation.z);
		if ((goPos - orderPos).magnitude > 1f)
		{
			rVal = false;
			//MoveLoop
			Rigidbody body = go.GetComponent<Rigidbody> ();
			Vector3 footBlock = new Vector3 (Mathf.Round (go.transform.position.x), Mathf.Round (go.transform.position.y) + 0.5f, Mathf.Round (go.transform.position.z));
			Vector3 horizontalVel = (order.targetLocation - footBlock);
			horizontalVel.y = 0f;
			horizontalVel = horizontalVel.normalized * movement;
			Vector3 movementDirection = horizontalVel.normalized;
			horizontalVel.y = body.velocity.y;
			body.velocity = horizontalVel;
			controller.transform.LookAt (go.transform.position + movementDirection, Vector3.up);
			CapsuleCollider cap = controller.GetComponentInChildren<CapsuleCollider> ();
			RaycastHit hit = new RaycastHit ();
			int mask = LayerMask.NameToLayer ("World");
			Vector3 p1 = controller.transform.position + cap.center + Vector3.up * cap.radius;
			Ray newray = new Ray (p1, movementDirection);

			if (Physics.Raycast (newray, out hit, 1.75f))
			{
				Vector3 impactPnt = hit.point;
				Jump (controller);
			}
		}
		else
		{
			return true;
		}
		return rVal;*/
	}

	public void Jump(EntityController controller)
	{

	}
	public Entity(float newMovement)
	{
		movement = newMovement;
	}
	public Entity(){	}

	protected Vector3[] recalculatePath(EntityController controller, Vector3 location, float leash)//Effectivly A star search
	{
		Stack<Vector3>rVal = new Stack<Vector3> ();
		Vector3 orig = controller.gameObject.transform.position;


		return rVal.ToArray();
	}

}
