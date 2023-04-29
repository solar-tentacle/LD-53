using UnityEngine;
using UnityEditor;
using System.Collections;
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
		GUILayout.Label("SINGLE TILE");
        // Customizing the inspector a little bit
        GUILayout.BeginHorizontal();
        for (int i = 0; i < grid.tiles.Length; i++)
        {
            GameObject tilePrefab = grid.tiles[i];
            // We want two buttons per line
            if(i % 2 == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            if (GUILayout.Button(tilePrefab.name))
            {
                grid.SelectTile(i);
				grid.DisableLoop();
            }
        }
        GUILayout.EndHorizontal();
        // SEPARATOR
        /*GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("SEQUENTIAL LOOPS");
		GUILayout.BeginHorizontal();
		for(int j = 0; j < grid.loops.Length; j++){
			if(j % 2 == 0 && j != 0)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			if(GUILayout.Button(grid.loops[j].loopName == "" ? "Loop "+ j : grid.loops[j].loopName)){
				grid.SelectLoop(j);
				grid.EnableLoop();
			}
		}
		GUILayout.EndHorizontal();*/
		// SEPARATOR
		/*GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("RANDOM LOOPS");
		GUILayout.BeginHorizontal();
		for(int z = 0; z < grid.randomLoops.Length; z++){
			if(z % 2 == 0 && z != 0)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			if(GUILayout.Button(grid.randomLoops[z].loopName == "" ? "Loop "+ z : grid.randomLoops[z].loopName)){
				grid.SelectRandomLoop(z);
				grid.EnableRandomLoop();
			}
		}
		GUILayout.EndHorizontal();*/
		// SEPARATOR
		GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("WARNING AREA!!!");
        if (GUILayout.Button("Rebuild Level"))
        {
            grid.RebuildLevel();
        }
        if (GUILayout.Button("Reset Level"))
        {
            grid.ResetLevel();

            var prefabInstance = Selection.activeObject.GameObject();

            var levelMaker = prefabInstance.GameObject().GetComponent<LevelMaker>();
        
            foreach(Transform child in levelMaker.Parent.GetComponentsInChildren<Transform>())
            {
                if (child == null || child == levelMaker.Parent)
                {
                    continue;
                }
                DestroyImmediate(child.gameObject);

            }

            // Clear levelTiles list
            levelMaker.LevelTiles.Clear();
            
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

            var parent = levelMaker.Parent;
            parent.Rotate(90.0f, 0.0f, 0.0f);
            
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

    private void GenerateData(LevelMaker levelMaker, LevelDataHolder levelDataHolder)
    {
        levelDataHolder.LevelData = new LevelData
        {
            Tiles = new Tile[levelMaker.sizeX, levelMaker.sizeY]
        };

        foreach (var levelTile in levelMaker.LevelTiles)
        {
            var position = levelTile.transform.position;
            int xIndex = Mathf.FloorToInt((position.x) / levelMaker.width);
            int yIndex = Mathf.FloorToInt((position.y) / levelMaker.height);
            levelDataHolder.LevelData.Tiles[xIndex, yIndex] = new Tile
            {
                TileView = levelTile
            };
        }

        string output = "";
        for (int i = 0; i < levelDataHolder.LevelData.Tiles.GetLength(0); i++)
        {
            output += "[";
            for (int j = 0; j < levelDataHolder.LevelData.Tiles.GetLength(1); j++)
            {
                var tile = levelDataHolder.LevelData.Tiles[i, j];
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
