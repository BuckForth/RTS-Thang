using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
//[RequireComponent(typeof(NavMeshSurface))]
public class Chunk : MonoBehaviour 
{
	public Block[,,] data = new Block[18,64,18];

	public int chunkX = 0;
	public int chunkY = 0;
	private List<Vector3> vertices = new List<Vector3>();
	private List<Vector2> UVs = new List<Vector2>();
	private List<int> triangles = new List<int>();
	private int faceCount = 0; 
	private int blockCount = 0;
	private float nudge = 0.001f;
	private Mesh mesh;

	private bool built = false;
	public bool ready = false;
	private bool updatingChunk = true;
	private Thread meshPrepThread;

	void Start()
	{
		//GetComponent<NavMeshSurface> ().layerMask = LayerMask.GetMask ("World");
		//GetComponent<NavMeshSurface> ().collectObjects = CollectObjects.All;
		//GetComponent<NavMeshSurface> ().center = new Vector3 (8.5f,0f,8.5f);
		//GetComponent<NavMeshSurface> ().size = new Vector3 (17f, 80f, 17f);
	}

	void Update()
	{
		if (!built && ready)
		{
			built = true;
			mesh = GetComponent<MeshFilter> ().mesh;
			UpdateChunk ();
		}
		else if (built && updatingChunk && meshPrepThread != null && !meshPrepThread.IsAlive)
		{
			updatingChunk = false;
			GameController.updateQueue.Enqueue (this);
		}
	}

	public void threadedGen()
	{
		updatingChunk = true;

		Vector3[] corners = new Vector3[8];
		Vector3[] vertList = new Vector3[12];
		bool[] hits = new bool[8];

		for (int i = 0; i < corners.Length; i++) 
		{
			corners[i] = new Vector3();
		}


		for (int xx = 1; xx < data.GetLength (0)-1; xx++) 
		{
			for (int yy = 0; yy < data.GetLength (1); yy++) 
			{
				for (int zz = 1; zz < data.GetLength (2)-1; zz++) 
				{
					int cubeindex = 0;

					corners[0] = new Vector3(xx, yy, zz);
					corners[1] = new Vector3(xx + 1, yy, zz);
					corners[2] = new Vector3(xx + 1, yy, zz + 1);
					corners[3] = new Vector3(xx, yy, zz + 1);
					corners[4] = new Vector3(xx, yy + 1, zz);
					corners[5] = new Vector3(xx + 1, yy + 1, zz);
					corners[6] = new Vector3(xx + 1, yy + 1, zz + 1);
					corners[7] = new Vector3(xx, yy + 1, zz + 1);

					bool[] touchies = new bool[] 
					{ 
						BlockAt(xx - 1,yy - 1,zz - 1),//0
						BlockAt(xx    ,yy - 1,zz - 1),//1
						BlockAt(xx + 1,yy - 1,zz - 1),//2
						BlockAt(xx - 1,yy - 1,zz    ),//3
						BlockAt(xx    ,yy - 1,zz    ),//4
						BlockAt(xx + 1,yy - 1,zz    ),//5
						BlockAt(xx - 1,yy - 1,zz + 1),//6
						BlockAt(xx    ,yy - 1,zz + 1),//7
						BlockAt(xx + 1,yy - 1,zz + 1),//8

						BlockAt(xx - 1,yy    ,zz - 1),//9
						BlockAt(xx    ,yy    ,zz - 1),//10
						BlockAt(xx + 1,yy    ,zz - 1),//11
						BlockAt(xx - 1,yy    ,zz    ),//12
						true,		  	      //13
						BlockAt(xx + 1,yy    ,zz    ),//14
						BlockAt(xx - 1,yy    ,zz + 1),//15
						BlockAt(xx    ,yy    ,zz + 1),//16
						BlockAt(xx + 1,yy    ,zz + 1),//17

						BlockAt(xx - 1,yy + 1,zz - 1),//18
						BlockAt(xx    ,yy + 1,zz - 1),//19
						BlockAt(xx + 1,yy + 1,zz - 1),//20
						BlockAt(xx - 1,yy + 1,zz    ),//21
						BlockAt(xx    ,yy + 1,zz    ),//22
						BlockAt(xx + 1,yy + 1,zz    ),//23
						BlockAt(xx - 1,yy + 1,zz + 1),//24
						BlockAt(xx    ,yy + 1,zz + 1),//25
						BlockAt(xx + 1,yy + 1,zz + 1)};//26

					hits[0] = (touchies[4] || touchies[12] || touchies[10] || touchies[3] || touchies[9]  || touchies[1] || touchies[0]);
					hits[1] = (touchies[4] || touchies[14] || touchies[10] || touchies[5] || touchies[11] || touchies[1] || touchies[2]);
					hits[2] = (touchies[4] || touchies[14] || touchies[16] || touchies[5] || touchies[17] || touchies[7] || touchies[8]);
					hits[3] = (touchies[4] || touchies[12] || touchies[16] || touchies[3] || touchies[15] || touchies[7] || touchies[6]);

					hits[4] = (touchies[22] || touchies[12] || touchies[10] || touchies[21] ||  touchies[9] || touchies[19] || touchies[18]);
					hits[5] = (touchies[22] || touchies[14] || touchies[10] || touchies[23] || touchies[11] || touchies[19] || touchies[20]);
					hits[6] = (touchies[22] || touchies[14] || touchies[16] || touchies[23] || touchies[17] || touchies[25] || touchies[26]);
					hits[7] = (touchies[22] || touchies[12] || touchies[16] || touchies[21] || touchies[15] || touchies[25] || touchies[24]);
					

					if (hits[0]) cubeindex += 1;
					if (hits[1]) cubeindex += 2;
					if (hits[2]) cubeindex += 4;
					if (hits[3]) cubeindex += 8;
					if (hits[4]) cubeindex += 16;
					if (hits[5]) cubeindex += 32;
					if (hits[6]) cubeindex += 64;
					if (hits[7]) cubeindex += 128;


					if (MCTable.EDGES [cubeindex] != 0 || MCTable.EDGES [cubeindex] != 255)
					{
						// Find the vertices where the surface intersects the cube
						if ((MCTable.EDGES [cubeindex] & 1) == 1)
							vertList [0] = vertexInterpolation (0.0f, corners [0], corners [1]);
						if ((MCTable.EDGES [cubeindex] & 2) == 2)
							vertList [1] = vertexInterpolation (0.0f, corners [1], corners [2]);
						if ((MCTable.EDGES [cubeindex] & 4) == 4)
							vertList [2] = vertexInterpolation (0.0f, corners [2], corners [3]);
						if ((MCTable.EDGES [cubeindex] & 8) == 8)
							vertList [3] = vertexInterpolation (0.0f, corners [3], corners [0]);
						if ((MCTable.EDGES [cubeindex] & 16) == 16)
							vertList [4] = vertexInterpolation (0.0f, corners [4], corners [5]);
						if ((MCTable.EDGES [cubeindex] & 32) == 32)
							vertList [5] = vertexInterpolation (0.0f, corners [5], corners [6]);
						if ((MCTable.EDGES [cubeindex] & 64) == 64)
							vertList [6] = vertexInterpolation (0.0f, corners [6], corners [7]);
						if ((MCTable.EDGES [cubeindex] & 128) == 128)
							vertList [7] = vertexInterpolation (0.0f, corners [7], corners [4]);
						if ((MCTable.EDGES [cubeindex] & 256) == 256)
							vertList [8] = vertexInterpolation (0.0f, corners [0], corners [4]);
						if ((MCTable.EDGES [cubeindex] & 512) == 512)
							vertList [9] = vertexInterpolation (0.0f, corners [1], corners [5]);
						if ((MCTable.EDGES [cubeindex] & 1024) == 1024)
							vertList [10] = vertexInterpolation (0.0f, corners [2], corners [6]);
						if ((MCTable.EDGES [cubeindex] & 2048) == 2048)
							vertList [11] = vertexInterpolation (0.0f, corners [3], corners [7]);

						//
						/*Somthing is up  here and I dont know why */ 
						int texId = 2;// BlockAt(xx,yy,zz).TextureIndex;

						Vector2 tC = new Vector2 ((float)(texId % Config.textureCols)/(float)Config.textureCols,(float)(texId / Config.textureCols)/(float)Config.textureRows);
						Vector2 tS = new Vector2(1/(float)Config.textureCols,1f/(float)Config.textureRows);


						for (int ii = 0; MCTable.TRIANGLES[cubeindex,ii] != -1; ii += 3) 
						{
							vertices.Add (vertList[MCTable.TRIANGLES[cubeindex,ii + 2]]);
							vertices.Add (vertList[MCTable.TRIANGLES[cubeindex,ii + 1]]);
							vertices.Add (vertList[MCTable.TRIANGLES[cubeindex,  ii  ]]);


							triangles.Add (blockCount * 3 + 2);
							triangles.Add (blockCount * 3 + 1);
							triangles.Add (blockCount * 3    );
						

							if(ii%2==0)UVs.Add (new Vector2 (tC.x + nudge, 1f-(tC.y + tS.y - nudge)));
							UVs.Add (new Vector2 (tC.x + tS.x - nudge, 1f-(tC.y + tS.y - nudge)));
							UVs.Add (new Vector2 (tC.x+ nudge, 1f-(tC.y + nudge)));
							if(ii%2!=0)UVs.Add (new Vector2 (tC.x + tS.x - nudge, 1f-(tC.y + nudge)));

							//triangles.Add (MCTable.TRIANGLES[cubeindex,i+2] + (blockCount * vertList.Length));
							//triangles.Add (MCTable.TRIANGLES[cubeindex,i+1] + (blockCount * vertList.Length));
							//triangles.Add (MCTable.TRIANGLES[cubeindex,i  ] + (blockCount * vertList.Length));

							blockCount++;
						}

					}
				}
			}
		}
		//Debug.Log ("Vert Count: " + vertices.Count + ", Trig Count: " + triangles.Count);
	}



	private static Vector3 vertexInterpolation(float isolevel, Vector3 p1, Vector3 p2) 
	{
		return (p1 + p2)/2f;
	}

	public void UpdateChunk ()
	{
		meshPrepThread = new Thread (threadedGen);
		meshPrepThread.IsBackground = true;
		meshPrepThread.Priority =  System.Threading.ThreadPriority.Lowest;
		meshPrepThread.Start ();
	}

	public Block BlockAt(int xx, int yy, int zz)
	{
		if (yy <= 0)
		{
			return Library.blocks ("Bedrock");
		}
		else if (xx >= 0 && xx < 18 && zz >= 0 && zz < 18 && yy >= 0 && yy < 64)
		{
			return data [xx, yy, zz];
		}
		return null;
	}

	public Block BlockAt(Vector3 vect)
	{
		return BlockAt(Mathf.RoundToInt(vect.x),Mathf.RoundToInt(vect.y),Mathf.RoundToInt(vect.z));
	}
		

	public void UpdateMesh()
	{
		mesh.Clear ();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = UVs.ToArray();
		mesh.RecalculateNormals ();

		faceCount=0;
		vertices.Clear();
		triangles.Clear();
		UVs.Clear();

		GetComponent<MeshCollider> ().sharedMesh = null;
		GetComponent<MeshCollider> ().sharedMesh = mesh;


		//GetComponent<NavMeshSurface> ().BuildNavMesh ();
	}
}
