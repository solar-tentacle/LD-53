using UnityEngine;

public class GridBuilder
{
    private const float GroundHeight = 0;
    private const float ObjectHeight = 1;
    private readonly LevelData _data;
    private readonly AssetsCollection _assetsCollection;

    private Transform _gridTransform;

    public GridBuilder(LevelData data)
    {
        _data = data;
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    public void Build(out GroundGridElement[,] ground, out ObjectGridElement[,] objects)
    {
        _gridTransform = new GameObject("Grid").transform;
        ground = BuildGround(_data.BackTiles);
        objects = BuildObjects(_data.ObjectTiles);
    }

    private GroundGridElement[,] BuildGround(Tile[,] tiles)
    {
        GroundGridElement[,] ground = new GroundGridElement[tiles.GetLength(0), tiles.GetLength(1)];

        for (int i = 0; i < _data.BackTiles.GetLength(0); i++)
        {
            for (int j = 0; j < _data.BackTiles.GetLength(1); j++)
            {
                GroundType type = tiles[i, j].TileView.GetComponent<GroundGridElement>().Type;
                GroundGridElement element = Object.Instantiate(_assetsCollection.GetGround(type), _gridTransform);
                element.transform.position = new Vector3(i + i * 0.1f, GroundHeight, j + j * 0.1f);
                ground[i, j] = element;
            }
        }

        return ground;
    }

    private ObjectGridElement[,] BuildObjects(Tile[,] tiles)
    {
        ObjectGridElement[,] objects = new ObjectGridElement[tiles.GetLength(0), tiles.GetLength(1)];

        for (int i = 0; i < _data.BackTiles.GetLength(0); i++)
        {
            for (int j = 0; j < _data.BackTiles.GetLength(1); j++)
            {
                if (tiles[i, j] == null || tiles[i, j].TileView == null || !tiles[i, j].TileView.TryGetComponent(out ObjectGridElement e)) continue;

                ObjectType type = e.Type;
                ObjectGridElement element = Object.Instantiate(_assetsCollection.GetObject(type), _gridTransform);
                element.transform.position = new Vector3(i + i * 0.1f, ObjectHeight, j + j * 0.1f);

                objects[i, j] = element;

                if (element is EncounterGridElement encounterElement)
                {
                    encounterElement.EncounterData = (e as EncounterGridElement).EncounterData;
                }
                
                if (element is PortalGridElement portalElement)
                {
                    portalElement.Data = (e as PortalGridElement).Data;
                    var dir = portalElement.Data.Direction;

                    var pos = element.transform.position;
                    Vector3 lookAtPosition = new Vector3(pos.x + dir.x * 0.1f, pos.y, pos.z + dir.y * 0.1f);
                    element.transform.LookAt(lookAtPosition);
                    
                }
            }
        }

        return objects;
    }
}