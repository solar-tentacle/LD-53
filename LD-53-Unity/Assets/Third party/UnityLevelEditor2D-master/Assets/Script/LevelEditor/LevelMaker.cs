using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;

[System.Serializable]
public class Loop
{
	public string loopName = "";
	public GameObject[] tiles;
}

[System.Serializable]
public class Tile
{
    public GameObject TileView;
}

public class LevelData
{
    public Tile[,] Tiles;
}

public class LevelMaker : MonoBehaviour {

    [Range(1f,2048f)]
    public float width = 32.0f;
    [Range(1f, 2048f)]
    public float height = 32.0f;
    public uint sizeX = 20;
    public uint sizeY = 20;

    [SerializeField] public Transform Parent;

    public Color gridColor = Color.green;
    public bool gridVisible = true;

    public GameObject[] tiles;
    [HideInInspector] public Loop[] loops;
    [HideInInspector] public Loop[] randomLoops;

    [HideInInspector] public List<GameObject> LevelTiles = new List<GameObject>();

    int selectedTile = 0;
    int selectedLoop = 0;
    int selectedRandomLoop = 0;
    int loopIndex = 0;
    bool loop;
	bool randomLoop;

    void OnDrawGizmos()
    {
		if(gameObject.GetComponentsInChildren<Transform>().Length - 1 > LevelTiles.Count){
			LevelTiles.Clear();
			foreach(Transform child in Parent.GetComponentsInChildren<Transform>()){
				if(!child.gameObject.Equals(Parent.gameObject)){
					LevelTiles.Add(child.gameObject);
				}
			}
		}
        DrawGrid();
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
        if (position.x < 0 || position.x > sizeX * width || position.y < 0 || position.y > sizeY * height)
        {
            return false;
        }
        
        foreach(GameObject tile in LevelTiles)
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
        }
		if(loop){
			if(loopIndex == loops[selectedLoop].tiles.Length){
				loopIndex = 0;
			}
        	GameObject newTile = PrefabUtility.InstantiatePrefab(loops[selectedLoop].tiles[loopIndex++]).GameObject();
        	newTile.transform.position = position;
        	newTile.transform.SetParent(Parent);
        	LevelTiles.Add(newTile);
            return true;
        }
		else if(randomLoop){
			GameObject newTile = PrefabUtility.InstantiatePrefab(randomLoops[selectedRandomLoop].tiles[Random.Range(0, randomLoops[selectedRandomLoop].tiles.Length)]).GameObject();
			newTile.transform.position = position;
			newTile.transform.SetParent(Parent);
			LevelTiles.Add(newTile);
            return true;
        }
		else{
			GameObject newTile = PrefabUtility.InstantiatePrefab(tiles[selectedTile]).GameObject();
			newTile.transform.position = position;
			newTile.transform.SetParent(Parent);
			LevelTiles.Add(newTile);
            return true;
        }

        return false;
    }

    public void RemoveTileAt(Vector3 position)
    {
        foreach (GameObject tile in LevelTiles)
        {
            if (tile.transform.position == position)
            {
                // Found it! Removing tile.
                //Debug.Log("Removing Tile");
                GameObject.DestroyImmediate(tile);
                LevelTiles.Remove(tile);
                return;
            }
        }
    }

    public void ResetLevel()
    {
    }

    public void RebuildLevel()
    {
        // This actually reinstantiates every gameobject in levelTiles list, for some reason Unity doesn't keep prefab link on instances
        // so this is useful when we make changes on the prefab and want to apply them to the level.
        List<GameObject> newLevelTiles = new List<GameObject>();
        int counter = 0;
        int totalElements = LevelTiles.Count;
        foreach (GameObject tileObj in LevelTiles)
        {
            string prefabName = tileObj.name.Replace("(Clone)", "");
            Vector3 objPosition = tileObj.transform.position;
            GameObject prefabToInstantiate = null;
            foreach (GameObject prefab in tiles)
            {
                if (prefab.name.Equals(prefabName))
                {
                    prefabToInstantiate = prefab;
                    break;
                }
            }
            if (prefabToInstantiate == null)
            {
                foreach (Loop lp in loops)
                {
                    foreach (GameObject prefab in lp.tiles)
                    {
                        if (prefab.name.Equals(prefabName))
                        {
                            prefabToInstantiate = prefab;
                            break;
                        }
                    }
                }
            }
            if (prefabToInstantiate == null)
            {
                foreach (Loop lp in randomLoops)
                {
                    foreach (GameObject prefab in lp.tiles)
                    {
                        if (prefab.name.Equals(prefabName))
                        {
                            prefabToInstantiate = prefab;
                            break;
                        }
                    }
                }
            }
            if (prefabToInstantiate != null)
            {
                GameObject.DestroyImmediate(tileObj);
                GameObject newTile = Instantiate<GameObject>(prefabToInstantiate) as GameObject;
                newTile.transform.position = objPosition;
                newTile.transform.SetParent(Parent);
                counter++;
            }
            else
            {
                Debug.Log("Could not find a prefab named: " + prefabName);
            }
        }
        LevelTiles.Clear();
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.Equals(gameObject))
            {
                LevelTiles.Add(child.gameObject);
            }
        }
        Debug.Log("Reinstantiated " + counter + " of " + totalElements + " gameObjects");
    }

    public void SelectTile(int index)
    {
        selectedTile = index;
        // In case of array overflow default to 0
        if(index >= tiles.Length)
            selectedTile = 0;
    }

	public void SelectLoop(int index)
	{
		selectedLoop = index;
		loopIndex = 0;
		// In case of array overflow default to 0
		if(index >= loops.Length)
			selectedLoop = 0;
	}

	public void SelectRandomLoop(int index)
	{
		selectedRandomLoop = index;
		// In case of array overflow default to 0
		if(index >= randomLoops.Length)
			selectedRandomLoop = 0;
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
