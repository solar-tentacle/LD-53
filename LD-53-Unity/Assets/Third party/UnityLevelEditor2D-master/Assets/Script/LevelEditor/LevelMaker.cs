using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Loop
{
	public string loopName = "";
	public GameObject[] tiles;
}

[Serializable]
public class Tile
{
    public GameObject TileView;
}

public class LevelMaker : MonoBehaviour {

    [Range(1f,2048f)]
    public float width = 32.0f;
    [Range(1f, 2048f)]
    public float height = 32.0f;
    public uint sizeX = 20;
    public uint sizeY = 20;

    [SerializeField] public Transform BackTilesParent;
    [SerializeField] public Transform ObjectsParent;

    public Color gridColor = Color.green;
    public bool gridVisible = true;

    public GameObject[] BackTilePrefabs = Array.Empty<GameObject>();
    public GameObject[] ObjectTilePrefabs = Array.Empty<GameObject>();
    [HideInInspector] public Loop[] loops;
    [HideInInspector] public Loop[] randomLoops;

    [HideInInspector] public List<GameObject> BackTiles = new();
    [HideInInspector] public List<GameObject> ObjectTiles = new();

    int selectedTile = 0;
    TileTypes selectedTileType = TileTypes.Back;
    int loopIndex = 0;
    bool loop;
	bool randomLoop;

    public enum TileTypes
    {
        Back,
        Object
    }

    void OnDrawGizmos()
    {
        CheckTiles(BackTiles, BackTilesParent);
        CheckTiles(ObjectTiles, ObjectsParent);
        DrawGrid();
    }

    private void CheckTiles(List<GameObject> tiles, Transform parent)
    {
        if (parent.GetComponentsInChildren<Transform>().Length - 1 <= tiles.Count)
        {
            return;
        }

        tiles.Clear();
        var children = parent.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (!child.gameObject.Equals(parent.gameObject))
            {
                tiles.Add(child.gameObject);
            }
        }
    }

    void DrawGrid()
    {
        Vector3 pos = Camera.current.transform.position;
		if(gridVisible){
			gridColor.a = 1f;
		}
		else{
			gridColor.a = 0f;
		}
		Gizmos.color = gridColor;

        for (float y = 0; y < sizeY * height + 1; y += this.height)
        {
            Gizmos.DrawLine(new Vector3(0, Mathf.Floor(y / this.height) * this.height, 0.0f),
                            new Vector3(sizeX * width, Mathf.Floor(y / this.height) * this.height, 0.0f));
        }
        for (float x = 0; x < sizeX * width + 1; x += this.width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / this.width) * this.width, 0, 0.0f),
                            new Vector3(Mathf.Floor(x / this.width) * this.width, sizeY * height, 0.0f));
        }
    }

    public bool AddTile(Vector3 position)
    {
        Transform parent = null;
        List<GameObject> list = null;
        GameObject[] prefabsList = null;
        
        switch (selectedTileType)
        {
            case TileTypes.Back:
                parent = BackTilesParent;
                list = BackTiles;
                prefabsList = BackTilePrefabs;
                break;
            case TileTypes.Object:
                parent = ObjectsParent;
                list = ObjectTiles;
                prefabsList = ObjectTilePrefabs;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (position.x < 0 || position.x > sizeX * width || position.y < 0 || position.y > sizeY * height)
        {
            return false;
        }
        
        foreach(GameObject tile in list)
        {
            if (tile == null)
            {
                continue;
            }
            if(tile.transform.position == position)
            {
                // If we get here, tile is already occupied, ignoring adding request
                //Debug.Log("Tile already occupied, ignoring");
                return false;
            }
        }{
			GameObject newTile = PrefabUtility.InstantiatePrefab(prefabsList[selectedTile]).GameObject();
			newTile.transform.position = position;
			newTile.transform.SetParent(parent);
			list.Add(newTile);
            return true;
        }
    }

    public void RemoveTileAt(Vector3 position)
    {
        foreach (GameObject tile in ObjectTiles)
        {
            if (GetTilePosition(tile) == position)
            {
                // Found it! Removing tile.
                //Debug.Log("Removing Tile");
                DestroyImmediate(tile);
                ObjectTiles.Remove(tile);
                return;
            }
        }
        
        foreach (GameObject tile in BackTiles)
        {
            if (GetTilePosition(tile) == position)
            {
                // Found it! Removing tile.
                //Debug.Log("Removing Tile");
                DestroyImmediate(tile);
                BackTiles.Remove(tile);
                return;
            }
        }
    }

    private Vector3 GetTilePosition(GameObject tile)
    {
        return new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
    }

    public void ResetLevel()
    {
    }

    public void SelectTile(int index, TileTypes type)
    {
        selectedTile = index;
        selectedTileType = type;
        // In case of array overflow default to 0
        var list = type == TileTypes.Back ? BackTilePrefabs : ObjectTilePrefabs;
        if(index >= list.Length)
            selectedTile = 0;
    }

	public void EnableRandomLoop(){
		loop = false;
		randomLoop = true;
	}

	public void EnableLoop(){
		randomLoop = false;
		loop = true;
	}

	public void DisableLoop(){
		randomLoop = false;
		loop = false;
	}

    public void GenerateLevel()
    {
    }
}
