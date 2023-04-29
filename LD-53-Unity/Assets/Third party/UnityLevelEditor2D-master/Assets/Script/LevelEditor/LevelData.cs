using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class LevelData: ISerializationCallbackReceiver
{
    public Tile[,] BackTiles;
    public Tile[,] ObjectTiles;

// A list that can be serialized
    [SerializeField, HideInInspector] private List<Package<Tile>> _serializableBackTiles;
    [SerializeField, HideInInspector] private List<Package<Tile>> _serializableObjectTiles;
    [SerializeField, HideInInspector] private Vector2Int _size;
// A package to store our stuff
    [System.Serializable]
    struct Package<TElement>
    {
        public int Index0;
        public int Index1;
        public TElement Element;
        public Package(int idx0, int idx1, TElement element)
        {
            Index0 = idx0;
            Index1 = idx1;
            Element = element;
        }
    }
    public void OnBeforeSerialize()
    {
        // Convert our Tiles array into a serializable list
        _serializableBackTiles = new List<Package<Tile>>();
        _serializableObjectTiles = new List<Package<Tile>>();
        _size.x = BackTiles.GetLength(0);
        _size.y = BackTiles.GetLength(1);
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                _serializableBackTiles.Add(new Package<Tile>(i, j, BackTiles[i, j]));
                _serializableObjectTiles.Add(new Package<Tile>(i, j, ObjectTiles[i, j]));
            }
        }
    }
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our Tiles array
        BackTiles = new Tile[_size.x, _size.y];
        foreach(var package in _serializableBackTiles)
        {
            BackTiles[package.Index0, package.Index1] = package.Element;
        }
        
        ObjectTiles = new Tile[_size.x, _size.y];
        foreach(var package in _serializableObjectTiles)
        {
            ObjectTiles[package.Index0, package.Index1] = package.Element;
        }
    }
}