using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level 
{
    public int slots;
    public List<int> stars = new List<int>();
    public int time;
    public string positon;
    //public List<List<MapTileJson>> Map = new List<List<MapTileJson>>();
    public List<MapJson> map = new List<MapJson>();
    public List<ItemJson> items = new List<ItemJson>();
}
