using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebWorker : MonoBehaviour {

    struct ShaderProperty
    {
        public string imageURL;
        public string propertyName;
        public string keyword;

        public ShaderProperty(string imageUrl, string propertyName, string keyword)
        {
            this.imageURL = imageUrl;
            this.propertyName = propertyName;
            this.keyword = keyword;
        }
    }

    Renderer myRenderer = null;

    ShaderProperty[] shaderProperties = new ShaderProperty[5] {
        new ShaderProperty("https://s3-us-west-2.amazonaws.com/sharemygame.com/texture-test/Checker+Plate_Albedo.png", "_MainTex", ""),
        new ShaderProperty("https://s3-us-west-2.amazonaws.com/sharemygame.com/texture-test/Checker%20Plate_Heightmap.png", "_ParallaxMap", "_PARALLAXMAP"),
        new ShaderProperty("https://s3-us-west-2.amazonaws.com/sharemygame.com/texture-test/Checker%20Plate_Heightmap.png", "_BumpMap", "_NORMALMAP"),
		new ShaderProperty("", "_OcclusionMap", ""),
		new ShaderProperty("", "_MetallicGlossMap", "_METALLICGLOSSMAP"),
    };

    IEnumerator Start()
    {
        myRenderer = GetComponent<Renderer>();
        foreach (ShaderProperty property in shaderProperties)
        {
            WWW www = new WWW(property.imageURL);
            yield return www;

            if (www.error == null)
            {
                var texture = www.texture;
				print("Assigining " + property.propertyName);
                myRenderer.material.SetTexture(property.propertyName, texture);
                // TODO special case bump map

                // Attemt to force update
                if (property.keyword != "")
                {
                    myRenderer.material.EnableKeyword(property.keyword);
                    print("Enabling " + property.keyword); 
                }
            }
        }
        myRenderer.UpdateGIMaterials();
		print("Finished");
    }

    public static Texture2D ToNormalMap2(Texture2D bumpTexture)
    {
        Texture2D normalMap = new Texture2D(bumpTexture.width, bumpTexture.height, TextureFormat.ARGB32, true);

		for (int y = 0; y < bumpTexture.height; y++)
		{
			for (int x = 0; x < bumpTexture.width; x++)
			{
                float xLeft = bumpTexture.GetPixel(x - 1, y).grayscale;
				float xRight = bumpTexture.GetPixel(x + 1, y).grayscale;
				float yUp = bumpTexture.GetPixel(x, y - 1).grayscale;
				float yDown = bumpTexture.GetPixel(x, y + 1).grayscale;
                float xDelta = ((xLeft - xRight) + 1) * 0.5f;
                float yDelta = ((yUp - yDown) + 1) * 0.5f;

				normalMap.SetPixel(x, y, new Color(xDelta, yDelta, 1.0f, 1.0f));
			}
		}
		normalMap.Apply();
        return normalMap;
	}
}
