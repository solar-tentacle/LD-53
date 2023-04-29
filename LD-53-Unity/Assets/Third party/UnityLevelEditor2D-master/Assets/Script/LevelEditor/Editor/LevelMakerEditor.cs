using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Unity.VisualScripting;

[CustomEditor(typeof(LevelMaker))]
public class LevelMakerEditor : Editor {

    LevelMaker grid;

    public void OnEnable()
    {
        grid = (LevelMaker)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // SEPARATOR
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("Back");
        // Customizing the inspector a little bit
        GUILayout.BeginHorizontal();
        for (int i = 0; i < grid.BackTilePrefabs.Length; i++)
        {
            GameObject tilePrefab = grid.BackTilePrefabs[i];
            
            /*if (tilePrefab == null)
            {
                continue;
            }*/
            // We want two buttons per line
            if(i % 2 == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            if (GUILayout.Button(tilePrefab.name))
            {
                grid.SelectTile(i, LevelMaker.TileTypes.Back);
				grid.DisableLoop();
            }
        }
        GUILayout.EndHorizontal();
        // SEPARATOR
        GUILayout.Label("Objects");
        // Customizing the inspector a little bit
        GUILayout.BeginHorizontal();
        for (int i = 0; i < grid.ObjectTilePrefabs.Length; i++)
        {
            GameObject tilePrefab = grid.ObjectTilePrefabs[i];
            
            /*if (tilePrefab == null)
            {
                continue;
            }*/
            // We want two buttons per line
            if(i % 2 == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            if (GUILayout.Button(tilePrefab.name))
            {
                grid.SelectTile(i, LevelMaker.TileTypes.Object);
                grid.DisableLoop();
            }
        }
        GUILayout.EndHorizontal();
		// SEPARATOR
		GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("WARNING AREA!!!");
        /*if (GUILayout.Button("Rebuild Level"))
        {
            grid.RebuildLevel();
        }*/
        if (GUILayout.Button("Reset Level"))
        {
            grid.ResetLevel();

            var prefabInstance = Selection.activeObject.GameObject();

            var levelMaker = prefabInstance.GameObject().GetComponent<LevelMaker>();
        
            ResetTiles(levelMaker.BackTiles, levelMaker.BackTilesParent);
            ResetTiles(levelMaker.ObjectTiles, levelMaker.ObjectsParent);

            // Mark the prefab as dirty
            EditorUtility.SetDirty(prefabInstance);

            AssetDatabase.OpenAsset(target);
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Generate Level"))
        {
            grid.GenerateLevel();
            
            var prefab = target;
            
            
            GameObject prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(target.GameObject());
            
            string baseAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabRoot);

            var basePrefab  = AssetDatabase.LoadAssetAtPath<GameObject>(baseAssetPath);
            var assetPathOrigin = baseAssetPath.Replace(basePrefab.name, prefabRoot.name);


            LevelMaker originalPrefab = AssetDatabase.LoadAssetAtPath<LevelMaker>(assetPathOrigin);


            var prefabInstance = PrefabUtility.InstantiatePrefab(originalPrefab).GameObject();

            var levelMaker = prefabInstance.GetComponent<LevelMaker>();
            var levelDataHolder = prefabInstance.AddComponent<LevelDataHolder>();
            

            GenerateData(levelMaker, levelDataHolder);

            var backParent = levelMaker.BackTilesParent;
            backParent.Rotate(90.0f, 0.0f, 0.0f);
            
            var objectsParent = levelMaker.ObjectsParent;
            objectsParent.Rotate(90.0f, 0.0f, 0.0f);
            
            DestroyImmediate(levelMaker);

            string assetPath = $"Assets/Levels/Generated/{prefab.name} generated.prefab";

            // If the user selected a valid path, save the prefab as an asset
            if (!string.IsNullOrEmpty(assetPath))
            {
                PrefabUtility.SaveAsPrefabAssetAndConnect(prefabInstance, assetPath, InteractionMode.UserAction);

                // Destroy the original instantiated prefab instance
                DestroyImmediate(prefabInstance);

                // Refresh the Asset Database to ensure the new prefab asset is visible in the Project window
                AssetDatabase.Refresh();
                Debug.Log($"Prefab saved as asset: {assetPath}");

                var prefabResult = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                AssetDatabase.OpenAsset(prefabResult);
                
                //PrefabUtility.UnpackPrefabInstance(Selection.activeGameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
            }
        }
    }

    private void ResetTiles(List<GameObject> list, Transform parent)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child == null || child == parent)
            {
                continue;
            }

            DestroyImmediate(child.gameObject);
        }

        // Clear levelTiles list
        list.Clear();
    }

    private void GenerateData(LevelMaker levelMaker, LevelDataHolder levelDataHolder)
    {
        levelDataHolder.LevelData = new LevelData
        {
            BackTiles = new Tile[levelMaker.sizeX, levelMaker.sizeY],
            ObjectTiles = new Tile[levelMaker.sizeX, levelMaker.sizeY]
        };
        
        FillTiles(levelMaker, levelMaker.BackTiles, levelDataHolder.LevelData.BackTiles);
        FillTiles(levelMaker, levelMaker.ObjectTiles, levelDataHolder.LevelData.ObjectTiles);
    }

    private static void FillTiles(LevelMaker levelMaker, List<GameObject> sourceList, Tile[,] resultList)
    {
        foreach (var levelTile in sourceList)
        {
            var position = levelTile.transform.position;
            int xIndex = Mathf.FloorToInt((position.x) / levelMaker.width);
            int yIndex = Mathf.FloorToInt((position.y) / levelMaker.height);
            resultList[xIndex, yIndex] = new Tile
            {
                TileView = levelTile
            };
        }

        string output = "";
        for (int i = 0; i < resultList.GetLength(0); i++)
        {
            output += "[";
            for (int j = 0; j < resultList.GetLength(1); j++)
            {
                var tile = resultList[i, j];
                output += (tile != null ? $" {tile.TileView.name}" : "         0         ").PadLeft(4);
            }

            output += "]\n";
        }

        Debug.Log(output);
    }

    void OnSceneGUI()
    {
        HandleInput();
    }

    /* From what I found on the internet, looks like we need to intercept events and disable their default action in order           *
     * to capture mouse in scene view... seems to work fine, but won't allow us to select objects in the scene if the LevelEditor    *
     * empty is selected, we will need to change focus on hierarchy, to select other things :)                                       */
    void HandleInput()
    {
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = controlID;
                //Debug.Log("MOUSE DOWN");
                e.Use();
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                //Debug.Log("MOUSE UP");
                OnMouseUpHandler(e.mousePosition, e.button);
                e.Use();
                break;
            case EventType.MouseDrag:
                GUIUtility.hotControl = controlID;
                //Debug.Log("MOUSE DRAG");
                OnMouseDragHandler(e.mousePosition, e.button);
                e.Use();
                break;
        }
    }

    void OnMouseUpHandler(Vector2 mpos, int button)
    {
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(mpos.x, -mpos.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;
        // The spawn position should be snapped to the grid
        Vector3 snappedPosition = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f, 
            Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f, 
            0.0f);
        // Add element: left click, remove element, right click
        if (button == 0)
        {
            AddTile(snappedPosition);
        }
        else if(button == 1)
        {
            RemoveTile(snappedPosition);
        }
    }

    // Support mouse drag
    void OnMouseDragHandler(Vector2 mpos, int button)
    {
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(mpos.x, -mpos.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;
        // The spawn position should be snapped to the grid
        Vector3 snappedPosition = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
            Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f,
            0.0f);
        // Add element: left click, remove element, right click
        if (button == 0)
        {
            AddTile(snappedPosition);
        }
		else if(button == 1)
        {
            RemoveTile(snappedPosition);
        }
    }

    private void AddTile(Vector3 snappedPosition)
    {
        if (grid.AddTile(snappedPosition))
        {
            SetDirty();
        }
    }

    private void RemoveTile(Vector3 snappedPosition)
    {
        grid.RemoveTileAt(snappedPosition);
        SetDirty();
    }

    private void SetDirty()
    {
        var prefabInstance = Selection.activeObject.GameObject();
        // Mark the prefab as dirty
        EditorUtility.SetDirty(prefabInstance);
    }
}
