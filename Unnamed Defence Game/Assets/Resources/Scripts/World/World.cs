using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class World  
{
	public static Material blockMat;
	public static int worldSize = 8;

	private static bool worldNoiseInit = false;
	private static WorldNoise worldNoise;

	private static List<Chunk> chunks = new List<Chunk>();


	public static Block BlockAt(int xx, int yy, int zz)
	{
		Block rVal = null;

		int localX = xx % 16;
		int localZ = zz % 16;
		int localY = yy % 64;

		int chunkX = (int)((float)xx / 16f);
		int chunkY = (int)((float)zz / 16f);

		if (chunkX >= 0 && chunkX < worldSize && chunkY >= 0 && chunkY < worldSize)
		{
			int ii = 0; bool done = false;
			while (ii < chunks.Count && !done)
			{
				if(chunks[ii].chunkX == chunkX && chunks[ii].chunkY == chunkY)
				{
					rVal = chunks[ii].data[localX,localY,localZ];
					done = true;
				}
				ii++;
			}
		}

		return rVal;
	}

	public static Block genWorld(Block orig, int xx, int yy, int zz)
	{
		if (!worldNoiseInit){initializeWorldNoise ();}
		Block rVal = orig;
		if (rVal == null)
		{
			float height = worldNoise.getNoise(xx,zz);
			if (yy > height && (xx == 5 || zz == 5))
			{
				rVal = Library.blocks("StoneWall");
			}
			if (yy < height)
			{
				rVal = Library.blocks("Grass");
			}
			if (yy < height - 1)
			{
				rVal =  Library.blocks("Dirt");
			}
			if (yy < height - 3)
			{
				rVal =  Library.blocks("Stone");
			}

			//rVal = genCaves (rVal,xx,yy,zz);
			//rVal = genOre (rVal, Library.blocks ("Ore"), 0.75f, xx, yy, zz);
			rVal = genStartZone (rVal, Entities.entities [0], xx, yy, zz, 80, 80, 32);

			if (yy == 0)
			{
				rVal =  Library.blocks("Bedrock");
			}

		}
		return rVal;
	}

	public static Block genCaves(Block orig, int xx, int yy, int zz)
	{
		if (orig == Library.blocks("Stone"))
		{
			float caveVal = Config.caveNoise (xx, yy, zz);
			float distFromParam1 = Mathf.Abs (20f - yy);
			float gradientAmount = Mathf.Max (0.0f,(10f - distFromParam1) / 10f);
			gradientAmount = Mathf.Min (1.0f,(10f - distFromParam1) / 10f);
			if (caveVal * gradientAmount > Config.caveStrength)
			{
				return null;
			}
		}
		return orig;
	}

	public static Block genOre(Block orig, Block ore, float oreCutoff ,int xx, int yy, int zz)
	{
		if (orig == Library.blocks("Stone"))
		{
			System.Random oreRNG = new System.Random (ore.blockID ());
			Vector2 OreOffset = new Vector2 ((float)oreRNG.Next (64000), (float)oreRNG.Next (64000));
			float oreVal = Config.oreNoise (xx + (int)OreOffset.x, yy, zz + (int)OreOffset.y);
			if (oreVal > oreCutoff)
			{
				return ore;
			}
		}
		return orig;
	}

	public static Block genStartZone(Block orig, Entity startingUnit, int xx, int yy, int zz, int startxx, int startzz, int startSize)
	{
		Block rVal = orig;
		bool inXRange = (startxx + startSize + yy > xx && startxx - startSize - yy < xx);
		bool inYRange = (startzz + startSize + yy > zz && startzz - startSize - yy < zz);
		if (inXRange && inYRange)
		{
			rVal = null;
			if (yy == 1 && xx == startxx && zz == startzz)
			{
				//SpawnCommander
			}
		}
		return rVal;
	}

	public static void BuildChunk(int xx, int yy)
	{
		GameObject chunkObj = new GameObject ("Chunk(" + xx.ToString () + "," + yy.ToString () + ")");
		Chunk chunk = chunkObj.AddComponent<Chunk> ();
		chunk.chunkX = xx;
		chunk.chunkY = yy;
		chunkObj.GetComponent<MeshRenderer> ().material = blockMat;
		chunk.transform.position = new Vector3 ((float)(xx * 16f),0f,(float)(yy * 16f));
		chunks.Add (chunk);
		chunkObj.layer = LayerMask.NameToLayer ("World");
		Thread newThread = new Thread(BuildChunkThread);
		newThread.IsBackground = true;
		newThread.Priority =  System.Threading.ThreadPriority.Lowest;
		newThread.Start (new ChunkThreadHelper (chunk, xx, yy));
		chunk.transform.parent = GameObject.FindObjectOfType<GameController>().transform;
	}

	private static void BuildChunkThread(object data)
	{
		Chunk chunk = ((ChunkThreadHelper)data).chunk;
		int xx = ((ChunkThreadHelper)data).xx;
		int yy = ((ChunkThreadHelper)data).yy;
		for (int x = -1; x < 17; x ++)
		{
			for (int y = 0; y < 64; y++)
			{
				for (int z = -1; z < 17; z ++)
				{
					chunk.data [x+1, y, z+1] = genWorld (chunk.data [x+1, y, z+1], x + (xx * 16), y, z  + (yy * 16));
				}
			}
		}
		chunk.ready = true;
	}

	private static void initializeWorldNoise()
	{
		worldNoise = new WorldNoise ();
		worldNoise.octives = new NoiseOperand[3];

		NoiseOperand mainBranch = new NoiseOperand ();
		mainBranch.mask = NoiseMask.none;
		mainBranch.maskParam1 = 0f;
		mainBranch.maskParam2 = 0f;
		mainBranch.minHeight = 10f;//original was 25
		mainBranch.noiseMagnitude = 20f;
		mainBranch.noisePower = 1f;
		mainBranch.smoothness = 100;
		worldNoise.octives [0] = mainBranch;

		NoiseOperand branch2 = new NoiseOperand ();
		branch2.mask = NoiseMask.none;
		branch2.maskParam1 = 0f;
		branch2.maskParam2 = 0f;
		branch2.minHeight = 0f;
		branch2.noiseMagnitude = 10f;
		branch2.noisePower = 1f;
		branch2.smoothness = 50;
		worldNoise.octives [1] = branch2;

		NoiseOperand branch3 = new NoiseOperand ();
		branch3.mask = NoiseMask.none;
		branch3.maskParam1 = 0f;
		branch3.maskParam2 = 0f;
		branch3.minHeight = 0f;
		branch3.noiseMagnitude = 2f;
		branch3.noisePower = 1.5f;
		branch3.smoothness = 20;
		worldNoise.octives [2] = branch3;

		worldNoiseInit = true;
	}

	private class ChunkThreadHelper
	{
		public int xx;
		public int yy;
		public Chunk chunk;

		public ChunkThreadHelper(Chunk nChunk, int nxx, int nyy)
		{
			xx = nxx;
			yy = nyy;
			chunk = nChunk;
		}
	}
}
