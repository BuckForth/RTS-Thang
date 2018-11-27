using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameController : MonoBehaviour 
{
	public bool complete = false;

	public int chunkCNT = 0;
	public int chunkDEX = 0;
	public ValuebarController LoadingBar;

	void Update () 
	{
		chunkCNT = World.worldSize * World.worldSize;
		if (chunkDEX < chunkCNT)
		{
			World.BuildChunk (chunkDEX % World.worldSize, chunkDEX / World.worldSize);
			chunkDEX++;
			LoadingBar.setValue ((float)(chunkDEX)/(float)(chunkCNT));
		}
		else
		{
			complete = true;
		}
	}
}
