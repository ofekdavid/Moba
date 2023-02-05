using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "Prefab Library")]
public class PrefabLibrary : ScriptableObject
{
    public List<GameObject> prefabs;
}

public class PrefabWindow : EditorWindow
{
    private GameObject prefab;
    public List<GameObject> prefabLibrary = new List<GameObject>();
    private Vector2 scrollPos;
    private Vector2Int gridSize = new Vector2Int(4, 4);
    private string prefabLibraryPath = "Assets/PrefabLibrary.asset";

    [MenuItem("Window/Prefab Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PrefabWindow));
    }
    private void OnEnable()
    {
        LoadPrefabLibrary();
    }

    private void OnDestroy()
    {
        SavePrefabLibrary();
    }

    private void LoadPrefabLibrary()
    {
        PrefabLibrary library = AssetDatabase.LoadAssetAtPath<PrefabLibrary>(prefabLibraryPath);
        if (library != null)
        {
            prefabLibrary = new List<GameObject>(library.prefabs);
        }
        else
        {
            prefabLibrary = new List<GameObject>();
        }
    }

    private void SavePrefabLibrary()
    {
        PrefabLibrary library = ScriptableObject.CreateInstance<PrefabLibrary>();
        library.prefabs = prefabLibrary;

        if (!Directory.Exists(Path.GetDirectoryName(prefabLibraryPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(prefabLibraryPath));
        }

        AssetDatabase.CreateAsset(library, prefabLibraryPath);
        AssetDatabase.SaveAssets();
    }

    private void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Add to Library"))
        {
            if (prefab != null)
            {
                prefabLibrary.Add(prefab);
                prefab = null;
            }
            else
            {
                UnityEngine.Debug.LogError("Please select a prefab first.");
            }
        }

        EditorGUILayout.LabelField("Prefab Library");
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            gridSize.x = (int)(position.width / 70);
            gridSize.y = (int)(position.height / 70);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));

            EditorGUILayout.BeginVertical();
            {
                for (int i = 0; i < prefabLibrary.Count; i++)
                {
                    if (i % gridSize.x == 0)
                        EditorGUILayout.BeginHorizontal();

                    GameObject prefabSelected = prefabLibrary[i];
                    Texture2D texture = AssetPreview.GetAssetPreview(prefabSelected);

                    if (GUILayout.Button(texture, GUILayout.Width(64), GUILayout.Height(64)))
                    {
                        prefab = prefabSelected;
                    }

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        prefabLibrary.RemoveAt(i);
                        i--;
                    }


                    if (i % gridSize.x == gridSize.x - 1 || i == prefabLibrary.Count - 1)
                        EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();


        if (GUILayout.Button("Insert Prefab"))
        {
            if (prefab != null)
            {
                GameObject selectedObject = Selection.activeGameObject;
                if (selectedObject != null)
                {
                    GameObject newPrefab = Instantiate(prefab);
                    newPrefab.transform.position = selectedObject.transform.position;
                    newPrefab.transform.rotation = selectedObject.transform.rotation;
                }
                else
                {
                    UnityEngine.Debug.LogError("Please select a prefab first.");
                }
            }
        }
    }
}