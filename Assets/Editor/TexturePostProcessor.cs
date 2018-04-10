
using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{

	void OnPreprocessTexture()
	{
		Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
		if (asset)
			return; //set default values only for new textures;

		TextureImporter importer = assetImporter as TextureImporter;
		importer.textureType = TextureImporterType.Sprite;
		importer.spriteImportMode = SpriteImportMode.Single;
		importer.alphaIsTransparency = true;
		importer.mipmapEnabled = false;
		importer.spritePixelsPerUnit = 32;
		importer.filterMode = FilterMode.Point;
	}


}