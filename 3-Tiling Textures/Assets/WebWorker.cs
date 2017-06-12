using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebWorker : MonoBehaviour {

	[SerializeField] string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";

	IEnumerator Start()
	{
		// Start a download of the given URL
		WWW www = new WWW(url);

		// Wait for download to complete
		yield return www;

		// assign texture
		Renderer myRenderer = GetComponent<Renderer>();
		myRenderer.material.mainTexture = www.texture;

        PlayerPrefs.SetString("TextureURL", url);
	}
}
