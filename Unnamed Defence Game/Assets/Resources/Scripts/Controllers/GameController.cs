using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class GameController : MonoBehaviour 
{
	private static GameController gc;
	private static GameState state = GameState.Start;
	public Material worldMaterial;
	public static Queue<Chunk> updateQueue = new Queue<Chunk>();
	private float navUpdateTimer = 5f;
	private float navCount = 0f;
	private bool updateNav = true;
	// Use this for initialization
	void Start () 
	{
		GetComponent<NavMeshSurface> ().layerMask = LayerMask.GetMask ("World");
		GetComponent<NavMeshSurface> ().collectObjects = CollectObjects.Children;
		World.blockMat = worldMaterial;
		if (gc != null)
		{
			Destroy (this);
		}
		else
		{
			gc = this;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (state == GameState.Start)
		{
			GameObject splashControlOBJ = GameObject.Find ("Splash");
			if (splashControlOBJ == null)
			{
				splashControlOBJ = Instantiate(Resources.Load("Prefabs/Misc/Splash")) as GameObject;
				splashControlOBJ.name = "Splash";
			}

			if (splashControlOBJ.GetComponent<SplashController> ().complete)
			{
				state = GameState.Initialization;
				Destroy (splashControlOBJ);
				Update ();
			}
		}
		else if(state == GameState.Initialization)
		{
			Physics.gravity *= 5f;
			state = GameState.Menu;
		}
		else if(state == GameState.Menu)
		{
			GameObject menu = GameObject.Find ("Menu");
			if (menu == null)
			{
				menu = Instantiate(Resources.Load("Prefabs/Misc/Menu")) as GameObject;
				menu.name = "Menu";
			}
				
		}
		else if(state == GameState.SpawnWorld)
		{
			GameObject gen = GameObject.Find ("NewGameController");
			if (gen == null)
			{
				gen = Instantiate(Resources.Load("Prefabs/Misc/NewGameController")) as GameObject;
				gen.name = "NewGameController";
			}
			if (gen.GetComponent<NewGameController> ().complete)
			{
				state = GameState.GamePlay;
				Destroy (gen);
				Update ();
			}
		}
		else if(state == GameState.LoadWorld)
		{

		}
		else if(state == GameState.GamePlay)
		{
			if (updateQueue.Count > 0)
			{
				updateQueue.Dequeue ().UpdateMesh ();
			}
			navCount -= Time.deltaTime;
			if (navCount < 0)
			{
				navCount += navUpdateTimer;
				GetComponent<NavMeshSurface> ().BuildNavMesh ();
			}
				
		}
		else if(state == GameState.Credits)
		{
			
		}
	}

	public static void setState(GameController.GameState newState)
	{
		state = newState;
	}

	public static void updateNavMesh()
	{
		NavMeshSurface surface = GameObject.FindObjectOfType<Chunk>().GetComponent<NavMeshSurface>();
		surface.BuildNavMesh ();
	}

	public enum GameState
	{
		Start,
		Initialization,
		Menu,
		SpawnWorld,
		LoadWorld,
		GamePlay,
		Credits
	};
}
