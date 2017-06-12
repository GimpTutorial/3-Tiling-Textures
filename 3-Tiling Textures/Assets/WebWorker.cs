using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebWorker : MonoBehaviour {

    [SerializeField] string albedoURL = "https://s3-us-west-2.amazonaws.com/sharemygame.com/texture-test/Checker+Plate_Albedo.png";
    [SerializeField] string heightMapURL = "https://s3-us-west-2.amazonaws.com/sharemygame.com/texture-test/Checker%20Plate_Heightmap.png";
	[SerializeField] string normalMapURL = "https://s3-us-west-2.amazonaws.com/sharemygame.com/texture-test/Checker%20Plate_Heightmap.png";
	[SerializeField] string occlusionMapURL = "";
	[SerializeField] string metallicURL = "";

	const string HEIGHT_MAP_SHADER_PROPERTY = "_ParallaxMap";
    const string NORMAL_MAP_SHADER_PROPERTY = "_BumpMap";
    const string OCCLUSION_MAP_SHADER_PROPERTY = "_OcclusionMap";

	IEnumerator Start()
	{
		// Download textures
        WWW wwwAlbedo = new WWW(albedoURL);
		yield return wwwAlbedo;
        WWW wwwHeightMap = new WWW(heightMapURL);
		yield return wwwHeightMap;
        WWW wwwNormalMap = new WWW(normalMapURL);
		yield return wwwNormalMap;
        WWW wwwOcclusionMap = new WWW(occlusionMapURL);
		yield return occlusionMapURL;

	
		// Assign textures
		Renderer myRenderer = GetComponent<Renderer>();
        myRenderer.material.mainTexture = wwwAlbedo.texture;
        myRenderer.material.SetTexture(HEIGHT_MAP_SHADER_PROPERTY, wwwHeightMap.texture);

        var normalMap = wwwNormalMap.texture;
        myRenderer.material.SetTexture(NORMAL_MAP_SHADER_PROPERTY, normalMap);


        if (wwwOcclusionMap.error == null)
        {
            myRenderer.material.SetTexture(OCCLUSION_MAP_SHADER_PROPERTY, wwwOcclusionMap.texture);
        }

        PlayerPrefs.SetString("TextureURL", albedoURL);

        print(myRenderer.material.GetTexture(NORMAL_MAP_SHADER_PROPERTY));
        print(myRenderer.material.GetTexture(OCCLUSION_MAP_SHADER_PROPERTY)) ; 
	}
}
