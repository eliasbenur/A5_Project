using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class stringAndBool
{
    public string name;
    public bool stolen;
    //[System.NonSerialized]
    public Sprite sprite;
    public stringAndBool(string _name, bool _stolen, Sprite sp)
    {
        name = _name;
        stolen = _stolen;
        sprite = sp;
    }
}
