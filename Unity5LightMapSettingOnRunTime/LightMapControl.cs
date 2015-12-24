using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LightMapControl : MonoBehaviour
{
	public int startIndex = 0;
	public List<GameObject> willLightMapGameObjects = new List<GameObject> ();
	public Texture2D[]lightmapFar, lightmapNear;
	[HideInInspector]
	public LightmapsMode mode;
	#if UNITY_EDITOR
	public void OnEnable()
	{
		UnityEditor.Lightmapping.completed += LoadLightmaps;
	}
	public void OnDisable()
	{
		UnityEditor.Lightmapping.completed -= LoadLightmaps;
	}

	public void LoadLightmaps()
	{
		willLightMapGameObjects = new List<GameObject>();
		mode = LightmapSettings.lightmapsMode;
		lightmapFar = null;
		lightmapNear = null;
		if (LightmapSettings.lightmaps != null && LightmapSettings.lightmaps.Length > 0)
		{
			int l = LightmapSettings.lightmaps.Length;
			lightmapFar = new Texture2D[l];
			lightmapNear = new Texture2D[l];
			for (int i = 0; i < l; i++)
			{
				lightmapFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
				lightmapNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
			}
		}
		MeshLightmapSetting[] savers = FindObjectsOfType<MeshLightmapSetting>();
		for (int i = 0; i < savers.Length; i++)
		{
			if (isAlreadyInwillLightMapGameObjectsOrNull(savers[i]))
			{
				continue;
			}
			willLightMapGameObjects.Add(savers[i].gameObject);
			savers[i].SaveSettings();
		}
	}

	private bool isAlreadyInwillLightMapGameObjectsOrNull(MeshLightmapSetting meshLightMapSetting)
	{
		if (meshLightMapSetting.gameObject == null)
		{
			return true;
		}
		for (int i = 0; i < willLightMapGameObjects.Count; i++)
		{
			if (willLightMapGameObjects[i].GetInstanceID() == meshLightMapSetting.gameObject.GetInstanceID())
			{
				return true;
			}
		}
		return false;
	}
	#endif
	void Start ()
	{
		if (Application.isPlaying) {
			LightmapSettings.lightmapsMode = mode;
			int l1 = (lightmapFar == null) ? 0 : lightmapFar.Length;
			int l2 = (lightmapNear == null) ? 0 : lightmapNear.Length;
			int l = (l1 < l2) ? l2 : l1;

			List<GameObject> sameNameGameObjects = findSameGameObjects ();
			bool findSamePrefab = false;
			for (int i = 0; i < sameNameGameObjects.Count; i++)
			{
				if (sameNameGameObjects [i].GetInstanceID () != gameObject.GetInstanceID ())
				{
					LightMapControl tmp = sameNameGameObjects [i].GetComponent<LightMapControl> ();
					if (tmp == null)
					{
						continue;
					}
					startIndex = tmp.startIndex;
					findSamePrefab = true;
					break;
				}
			}

			if (!findSamePrefab)
			{
				addLightMaps (l, l1, l2);
			}
			else
			{
				bool findLightMap = false;
				for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
				{
					if (lightMapDataInList (LightmapSettings.lightmaps [i]))
					{
						findLightMap = true;
						startIndex = i;
						break;
					}
				}
				if (!findLightMap)
				{
					addLightMaps (l, l1, l2);
				}
			}
			for (int i = 0; i < willLightMapGameObjects.Count; i++)
			{
				willLightMapGameObjects [i].GetComponent<MeshLightmapSetting> ().LoadSettings (startIndex);
			}
			Destroy (this);
		}
	}

	private void addLightMaps (int l, int l1, int l2)
	{
		LightmapData[] lightmaps = new LightmapData[l + startIndex];
		startIndex = LightmapSettings.lightmaps.Length;
		if (l > 0) {
			lightmaps = new LightmapData[l + startIndex];
			for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
			{
				lightmaps [i] = new LightmapData ();
				lightmaps [i].lightmapFar = LightmapSettings.lightmaps [i].lightmapFar;
				lightmaps [i].lightmapNear = LightmapSettings.lightmaps [i].lightmapNear;
			}
			for (int i = 0; i < l; i++)
			{
				lightmaps [startIndex + i] = new LightmapData ();
				if (i < l1)
					lightmaps [startIndex + i].lightmapFar = lightmapFar [i];
				if (i < l2)
					lightmaps [startIndex + i].lightmapNear = lightmapNear [i];
			}
		}
		LightmapSettings.lightmaps = lightmaps;
	}

	private bool lightMapDataInList (LightmapData data)
	{
		bool findFar = false;
		bool findNear = false;
		for (int i = 0; i < lightmapFar.Length; i++)
		{
			if (lightmapFar [i] == data.lightmapFar)
			{
				findFar = true;
			}
		}
		for (int i = 0; i < lightmapNear.Length; i++)
		{
			if (lightmapNear [i] == data.lightmapNear)
			{
				findNear = true;
			}
		}
		if (findFar && findNear)
		{
			return true;
		}
		return false;
	}

	private List<GameObject> findSameGameObjects ()
	{
		List<GameObject> result = new List<GameObject> ();
		Transform parentsTran = transform.parent;
		if (parentsTran != null) {
			for (int i = 0; i < parentsTran.childCount; i++)
			{
				if (parentsTran.GetChild (i).gameObject.name == gameObject.name)
				{
					result.Add (parentsTran.GetChild (i).gameObject);
				}
			}
		}
		else
		{
			Object[] allGameObject = GameObject.FindObjectsOfType (typeof(GameObject));
			for (int i = 0; i < allGameObject.Length; i++)
			{
				GameObject obj = allGameObject [i] as GameObject;
				if (obj.name == gameObject.name)
				{
					result.Add (obj);
				}
			}
		}
		return result;
	}
}
