using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Net.Mime.MediaTypeNames;

public class PrefabLibraryScript : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();

    private void Start()
    {
        LoadPrefabs();
    }

    public void SavePrefabs()
    {
        PlayerPrefs.SetInt("PrefabCount", prefabs.Count);
        for (int i = 0; i < prefabs.Count; i++)
        {
            PlayerPrefs.SetString("Prefab" + i, prefabs[i].name);
        }
        PlayerPrefs.Save();
    }

    public void LoadPrefabs()
    {
        prefabs.Clear();
        int prefabCount = PlayerPrefs.GetInt("PrefabCount", 0);
        for (int i = 0; i < prefabCount; i++)
        {
            string prefabName = PlayerPrefs.GetString("Prefab" + i);
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab != null)
            {
                prefabs.Add(prefab);
            }
        }
    }
}
