using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Game4
{
    public class MapItemController : MonoBehaviour
    {
        public  int width;
        
        public int height;
        public int MapIndex;
        public List<ItemController> mapTileList = new List<ItemController>();
        // Start is called before the first frame update
        void Start()
        {

        }

        public void initMapItem(int mapIndex, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.name = "Map_" + mapIndex;
            MapIndex = mapIndex;
        }    

        // Update is called once per frame
        void Update()
        {

        }
    }
}
