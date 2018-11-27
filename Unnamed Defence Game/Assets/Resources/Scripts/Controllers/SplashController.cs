using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
	public bool complete = false;
	public float fadeTime = 0.25f;
	public float holdTime = 1.5f;
	public float splashTime = 0f;

	// Update is called once per frame
	void Update () 
	{
		splashTime += Time.deltaTime;
		float alpha = 0f;
		if (splashTime < fadeTime)					{alpha = (splashTime / fadeTime);}
		else if(splashTime < (fadeTime + holdTime))			{alpha = 1f;}
		else if(splashTime < (fadeTime + holdTime + fadeTime))		{alpha = 1f-((splashTime - fadeTime - holdTime) / fadeTime);}
		else if(splashTime >= (fadeTime + holdTime + fadeTime))		{alpha = 0f; complete = true;};

		Image[] SplashImages = GetComponentsInChildren<Image>(true);
		foreach (Image splash in SplashImages)
		{	
			if (splash.CompareTag ("Splash"))
			{
				splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, alpha);
			}
		}
	}
}
