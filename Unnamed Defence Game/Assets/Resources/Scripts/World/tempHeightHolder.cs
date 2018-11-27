using UnityEngine;
using System.Collections;

public class tempHeightHolder : MonoBehaviour
{
	public WorldNoise noise = new WorldNoise();
	public Material mat;
	// Use this for initialization
	void Start ()
	{
		World.blockMat = mat;
		for (int xx = 0; xx < 8; xx++)
		{
			for (int yy = 0; yy < 8; yy++)
			{
				World.BuildChunk (xx, yy);
			}
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

