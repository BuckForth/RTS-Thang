using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class NoiseOperand
{
	public NoiseMask mask = NoiseMask.none;
	public float maskParam1 = 0;
	public float maskParam2 = 0;

	public float smoothness = 0;
	public float minHeight = 0;
	public float noiseMagnitude = 0;
	public float noisePower = 1;

	public NoiseOperand multiply = null;

	public Vector2 octiveOffset = new Vector2 ();


	public float getNoise(int px, int py)
	{
		float rVal = Noise(px + 64000, py + 64000, smoothness, noiseMagnitude, noisePower);
		if (mask == NoiseMask.gradient)
		{
			rVal = Noise(px + 64000 + (int)octiveOffset.x, py + 64000 + (int)octiveOffset.y, smoothness, 1, 1);
			float distFromParam1 = Mathf.Abs (maskParam1 - rVal);
			float gradientAmount = Mathf.Max (0.0f,(maskParam2 - distFromParam1) / maskParam2);
			float noiseVal = (1 + Mathf.Cos (Mathf.PI + gradientAmount * Mathf.PI))/2 * Mathf.Abs (noiseMagnitude);
			rVal = minHeight + ((Mathf.Abs(noiseMagnitude)/noiseMagnitude)*Mathf.Pow(noiseVal,noisePower));
		}
		if (multiply != null)
		{
			rVal *= multiply.getNoise(px,py);
		}
		return rVal;
	}

	float Noise (int x, int y, float scale, float mag, float exp)
	{
		float val = (Mathf.PerlinNoise (x / scale, y / scale) * mag);
		if (val == 0)
		{
			return minHeight;
		}
		else if (val < 0)
		{
			return (float)minHeight - (Mathf.Pow (Mathf.Abs (val), (exp)));
		}
		if (scale == 0)
		{
			return minHeight;
		}
		return (float) minHeight + (Mathf.Pow (val,(exp) )); 
	}
}
[Serializable]
public class WorldNoise
{
	public NoiseOperand[] octives;
	public float getNoise(int px, int py)
	{
		float rVal = 0;
		foreach (NoiseOperand octive in octives)
		{
			rVal += octive.getNoise(px,py);
		}
		return rVal;
	}
}


public class NoiseOperand3D
{
	public NoiseMask mask = NoiseMask.none;
	public float maskParam1 = 0;
	public float maskParam2 = 0;

	public float smoothness = 0;
	public float minHeight = 0;
	public float noiseMagnitude = 0;
	public float noisePower = 1;

	public NoiseOperand3D multiply = null;

	public Vector2 octiveOffset = new Vector2 ();


	public float getNoise(int px, int py, int pz)
	{
		float rVal = Noise(px + 64000, py + 64000, pz + 64000 , smoothness, noiseMagnitude, noisePower);
		rVal = Mathf.Pow (rVal, noisePower);
		if (mask == NoiseMask.gradient)
		{
			rVal = Noise3D.GetNoise((px/smoothness) + 64000 + (int)octiveOffset.x, (py/smoothness), (pz/smoothness) + 64000 + (int)octiveOffset.y);
			float distFromParam1 = Mathf.Abs (maskParam1 - rVal);
			float gradientAmount = Mathf.Max (0.0f,(maskParam2 - distFromParam1) / maskParam2);
			float noiseVal = (1 + Mathf.Cos (Mathf.PI + gradientAmount * Mathf.PI))/2 * Mathf.Abs (noiseMagnitude);
			rVal = minHeight + ((Mathf.Abs(noiseMagnitude)/noiseMagnitude)*Mathf.Pow(noiseVal,noisePower));
		}
		if (multiply != null)
		{
			rVal *= multiply.getNoise(px,py,pz);
		}
		return rVal;
	}

	float Noise (int x, int y, int z, float scale, float mag, float exp)
	{
		float val = Noise3D.GetNoise (x / scale, y / scale, z / scale);
			//(Mathf.PerlinNoise (x / scale, y / scale) * mag);
		if (val == 0)
		{
			return minHeight;
		}
		else if (val < 0)
		{
			return (float)minHeight - (Mathf.Pow (Mathf.Abs (val), (exp)));
		}
		if (scale == 0)
		{
			return minHeight;
		}
		return (float) minHeight + (Mathf.Pow (val,(exp) )); 
	}
}
[Serializable]
public class WorldNoise3D
{
	public NoiseOperand3D[] octives;
	public float getNoise(int px, int py, int pz)
	{
		float rVal = 0;
		foreach (NoiseOperand3D octive in octives)
		{
			rVal += octive.getNoise(px,py,pz);
		}
		return rVal;
	}
}

[Serializable]
public enum NoiseMask
{
	none,
	gradient
};