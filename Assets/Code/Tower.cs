using System;
using UnityEngine;

[Serializable]
public class Tower
{

    public string name;
    public int cost;
    public GameObject prefab;
    public Vector3 offset;      // added for repositioning the tower

    public Tower(string _name, int _cost, GameObject _prefab, Vector3 _offset)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
        offset = _offset;       // added for repositioning the tower

    }
}