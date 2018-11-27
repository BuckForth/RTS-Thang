using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBlock : Block
{
	public int TextureSideIndex;
	public ColumnBlock (string blockName, float hardness, float blastResist, float burnTime, int textureIndexTopBot, int textureIndexSides)
	{
		
		BlockName = blockName;
		Hardness = hardness;
		BlastResist = blastResist;
		BurnTime = burnTime;
		TextureIndex = textureIndexTopBot;
		TextureSideIndex = textureIndexSides;
	}

	public override int getTexture (int side)
	{
		if(side == 4 || side == 5)
		{
			return base.getTexture (side);
		}
		return TextureSideIndex;
	}
}
