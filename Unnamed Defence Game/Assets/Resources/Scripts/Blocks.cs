using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Blocks
{
	public static Block[] blocks = 
	{
		new ColumnBlock("Grass",1.1f,1.2f,0,0,1),
		new Block("Dirt",1.0f,1.0f,0,2),
		new Block("Stone",5.0f,3.0f,0,3),
		new Block("Logs",2.0f,1.5f,5.0f,4),
		new Block("Wood",2.0f,1.5f,3.0f,5),
		new Block("Bedrock",float.MaxValue,float.MaxValue,0f,6),
		new Block("Ore",5.0f,3.0f,0,7),
		new Block("StoneWall",7.5f,3.5f,0f,8)
		
	};
}

