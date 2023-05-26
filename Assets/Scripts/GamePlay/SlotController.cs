using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Game4
{
    public class SlotController : MonoBehaviour
    {
        private int id;
        private int index;
        private ItemController itemInSlot;
        public int IdCheckMatch = -1;
        public void UpdateItem(ItemController item)
        {
            itemInSlot = item;
            Id = item != null ? item.Id : 0;
            if (item == null) IdCheckMatch = -1;
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public ItemController Item
        {
            get { return itemInSlot; }
        }       
    }
}
