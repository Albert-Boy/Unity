using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class MeshLightmapSetting : MonoBehaviour
{
	[HideInInspector]
	public int lightmapIndex;
	[HideInInspector]
	public Vector4 lightmapScaleOffset;

	public void SaveSettings ()
	{
		Renderer renderer = GetComponent<Renderer> ();
		lightmapIndex = renderer.lightmapIndex;
		lightmapScaleOffset = renderer.lightmapScaleOffset;
	}

	public void LoadSettings (int startIndex)
	{
		Renderer renderer = GetComponent<Renderer> ();
		renderer.lightmapIndex = lightmapIndex + startIndex;
		renderer.lightmapScaleOffset = lightmapScaleOffset;
		if (Application.isPlaying)
		{
			Destroy (this);
		}
	}
}
