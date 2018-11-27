using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
	public string BlockName;
	public float Hardness;
	public float BlastResist;
	public float BurnTime;
	public int TextureIndex;

	public Block()
	{
		BlockName = "";
		Hardness = 0.0f;
		BlastResist = 0.0f;
		BurnTime = 0.0f;
		TextureIndex = 0;
	}
	public Block (string blockName, float hardness, float blastResist, float burnTime, int textureIndex)
	{
		BlockName = blockName;
		Hardness = hardness;
		BlastResist = blastResist;
		BurnTime = burnTime;
		TextureIndex = textureIndex;
	}
	public virtual int blockID()
	{
		int rVal = -1;
		for(int ii = 0; ii < Blocks.blocks.Length; ii++)
		{
			Block block = Blocks.blocks[ii];
			if (block.BlockName.Equals (BlockName))
			{
				return ii;
			}
		}
		Debug.LogError ("Block \"" + BlockName + "\" was not found in the block regestry.\nAll blocks must have an instance in \"Blocks.cs\"");
		return rVal;
	}
	public virtual int getTexture()
	{
		return TextureIndex;
	}
	public virtual int getTexture(int side)
	{
		side++;
		return getTexture ();
	}
	public static implicit operator bool(Block b)
	{
		return (b != null);
	}
}
