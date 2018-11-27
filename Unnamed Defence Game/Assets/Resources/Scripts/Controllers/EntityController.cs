using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour 
{
	public int health = 0;
	public int team = 0;
	public bool walking = false;
	public Entity driverEntity;
	public Queue<Order> orders = new Queue<Order> ();

	// Use this for initialization
	void Start () 
	{
		Vector3 targetLocation = new Vector3 (16f,0f,16f);
		orders.Enqueue (new MoveOrder(targetLocation));
		orders.Enqueue (new MoveOrder(new Vector3 (100f,0f,50f)));
		orders.Enqueue (new MoveOrder(new Vector3 (80f,0f,100f)));
		driverEntity = new Entity (5f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (orders.Count > 0)
		{
			bool orderComplete = driverEntity.Act (this);
			if (orderComplete)
			{
				orders.Dequeue ();
			}
		}
		if (Input.GetKeyDown (KeyCode.J))
		{
			GetComponent<NavMeshAgent>().destination = (new Vector3 (Random.value * 100f,0f,Random.value * 100f));
			GetComponent<NavMeshAgent> ().isStopped = false;
		}
	}

	void selectEntity(RSTController rtsController)
	{

	}

	public void addOrder(Order newOrder)
	{

	}

	public void hurt(float damage)
	{
		health -= (int)damage;
		float carryChance = damage - ((float)((int)damage));
		if (Random.value < carryChance)
		{
			health -= 1;
		}
	}
}
