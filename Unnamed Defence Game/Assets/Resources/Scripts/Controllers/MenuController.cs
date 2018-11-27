using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
	public bool complete;
	public GameObject[] elements;
	public Text worldSizeText;
	public Slider worldSizeSlider;
	public int worldsize = 8;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void changeElement(int elementId)
	{
		for (int ii = 0; ii < elements.Length; ii++)
		{
			elements [ii].SetActive (ii == elementId);
		}
	}

	public void newMap()
	{
		complete = true;
		GameController.setState (GameController.GameState.SpawnWorld);
		Destroy (this.gameObject);
	}

	public void updateWorldSize ()
	{
		worldsize = (int)worldSizeSlider.value;
		World.worldSize = worldsize;
		worldSizeText.text = worldsize.ToString ();
	}
}
