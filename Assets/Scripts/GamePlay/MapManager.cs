using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
namespace Minigame.Game4
{
    public class MapManager : MonoBehaviour
    {
        public string valueTest;

        [SerializeField] private Transform transformMapItem = null;
        [SerializeField] private Transform transformSlot = null;

        [SerializeField] private SlotController prefabSlot = null;
        [SerializeField] private MapItemController prefabMapItem = null;
        [SerializeField] private ItemController prefabItem = null;

        List<SlotController> listSlotTarget = new List<SlotController>();
        List<ItemController> listItem = new List<ItemController>();
        [SerializeField] List<ItemController> listResult = new List<ItemController>();

        [SerializeField] List<Sprite> listSpriteItem = new List<Sprite>();
        List<int> listItemJson = new List<int>();

        private int timeRemain;
        private int combo;
        [SerializeField] private int point;
        private int gold;

        //GameUi gameUI;
        TimerController timerController;
        List<object> listStars;

        public const int MAX_SIZE_OF_WIDTH = 960;
        private const float TIME_MOVE_ITEM = 0.2f;
        public static bool ALLOW_ACTION = true;
        bool isFinish;

        //xoa
        [SerializeField] private GameObject goCanvasGamePlay = null;
        [SerializeField] private GameObject goCanvasCreateMap = null;
        [SerializeField] private Button btnExit = null;

        public static MapManager Instance { get; private set; }

        // Start is called before the first frame update

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            btnExit.onClick.AddListener(OnExitClick_Editor);
        }
        void Start()
        {
            //initGame();
        }

        public void initGame()
        {
            var temp = MiniJSON.Json.Deserialize(valueTest);
            Dictionary<string, object> dataMap = temp as Dictionary<string, object>;
            listResult.Clear();
            if (timerController != null) timerController.DestroyTimer();
            timerController = null;
            timerController = GameUtils.CreateTimer(transform);
            combo = gold = point = 0;
            initMap(dataMap);
        }   
        
        public void initMap(Dictionary<string, object> dataMap)
        {
            transformMapItem.localScale = MainGameController.Instance.vectorScale;
            clearTranform(transformMapItem);
            removeOldSlot();
            listSlotTarget.Clear();

            int totalSlot = dataMap.GetInt("slots");
            initSlot(totalSlot);

            removeOldItem();
            listItem.Clear();
            List<object> items = dataMap.GetList("items");
            getListDataItem(items);
            //initItems(items, dataMap.GetInt("item_type"));

            int mapindex = 1;
            List<object> dataMapList = dataMap.GetList("map");
            foreach (var mapItem in dataMapList)
            {
                Dictionary<string, object> map = mapItem as Dictionary<string, object>;
                int width = map.GetInt("width");
                int height = map.GetInt("height");
                List<object> mapTileList = map.GetList("mapTile");
                createMapItem(mapindex, width, height,mapTileList);
                mapindex++;
            }

            ALLOW_ACTION = true;
        }

        private void onEndTime()
        {
            Debug.LogError("on endgame");

        }
        public void Surrender()
        {
            //handleEndGame(Enums.MiniGameEndGame.LOSE);
        }
        private void getListDataItem(List<object> items)
        {
            listItemJson.Clear();
            foreach(var item in items)
            {
                Dictionary<string, object> itemTemp = item as Dictionary<string, object>;
                int quantity = itemTemp.GetInt("q");
                int id = itemTemp.GetInt("id");
                for (int i = 0; i < quantity; i++)
                {
                    listItemJson.Add(id);
                }
            }    
        }    

        private void createItem(int width, int height, Transform mapItemTranform, List<object> mapTileList)
        {
            int count = width * height;

            float posX = 0;
            float posY = 0;
            float a = prefabItem.GetComponent<RectTransform>().rect.width;
            mapItemTranform.GetComponent<MapItemController>().mapTileList.Clear();
            int mapTileIndex = 0;
            for (int i = 0; i < height; i++)
            {
                posY = height * a * 1.0f / 2 - (i + 0.5f) * a * 1.0f;
                for (int j = 0; j < width; j++)
                {
                    posX = (j + 0.5f) * a * 1.0f - width * a * 1.0f / 2;

                    var copy = Instantiate(prefabItem);
                    copy.transform.SetParent(mapItemTranform, false);
                    copy.transform.localPosition = new Vector3(posX, posY, 0f);
                    copy.SetMap(mapItemTranform.GetComponent<MapItemController>().MapIndex);


                    Dictionary<string, object> mapTile = mapTileList[mapTileIndex] as Dictionary<string, object>;
                    bool isEmpty = mapTile.GetBool("isEmpty");
                    if (isEmpty)
                    {
                        copy.gameObject.SetActive(false);                      
                    }
                    else
                    {
                        copy.OnClick = onChooseItem;
                        int index = GameUtils.RandomRange(0, listItemJson.Count - 1);
                        Sprite imageItem = listSpriteItem[listItemJson[index]-1];
                        listItemJson.RemoveAt(index);
                        copy.InitData(int.Parse(imageItem.name), imageItem);
                        copy.name = "Item_" + copy.imgItem.sprite.name + "_" + copy.MapIndex + "_" + (copy.transform.GetSiblingIndex() + 1);
                        mapItemTranform.GetComponent<MapItemController>().mapTileList.Add(copy);
                    }
                    mapTileIndex++;


                }
            }
        } 
        private void setImageAndIdItem(List<object> items)
        {

        }    
        private void createMapItem(int mapIndex, int width, int height, List<object> mapTileList)
        {
            var mapIns = Instantiate(prefabMapItem);
            mapIns.transform.SetParent(transformMapItem.transform, false);
            //depthIns.transform.SetAsFirstSibling();
            mapIns.initMapItem(mapIndex, width, height);
            createItem(width, height, mapIns.transform, mapTileList);

        }
        private void onChooseItem(ItemController item)
        {
            SlotController slotContainer = getBlankSlotForItem(item);
            if (slotContainer != null)
            {
                if (slotContainer.Id != 0)
                {
                    for (int i = listSlotTarget.Count - 1; i >= slotContainer.Index; i--)
                    {
                        SlotController checkSlot = listSlotTarget[i];
                        int beforeIndex = i - 1;
                        if (beforeIndex < slotContainer.Index) continue;
                        SlotController moveSlot = listSlotTarget[beforeIndex];
                        checkSlot.UpdateItem(moveSlot.Item);

                        if (moveSlot.Item != null)
                        {
                            Vector3 pos = getPos(checkSlot, moveSlot.Item);
                            moveSlot.Item.Move(pos, 0.1f);
                        }
                        moveSlot.UpdateItem(null);
                    }
                }
                slotContainer.UpdateItem(item);

                item.ClearAction();

                item.SetOrder(200);
                item.name = "ToSlot";
                item.Move(getPos(slotContainer, item), TIME_MOVE_ITEM);
                bool isFull = isFullSlot();
                if (isFull)
                {
                    //Debug.LogError("endgame " + isFull);
                    ALLOW_ACTION = false;
                    //return;
                }

                List<SlotController> listMatch = null;
                int count = listSlotTarget.Count;
                for (int i = 0; i < count; i++)
                {
                    SlotController slotController = listSlotTarget[i];
                    listMatch = getListMatch(slotController);
                    if (listMatch.Count >= 3)
                    {
                        listMatch.ForEach(_item => { _item.IdCheckMatch = item.GetInstanceID(); });
                        //ALLOW_ACTION = false;
                        break;
                    }
                }
                if (listMatch == null) listMatch = new List<SlotController>();
                List<ItemController> listItemScored = null;

                List<DataMove> listMove = null;
                checkLogicScore(isFull, listMatch, out listItemScored, out listMove);
                //this.Wait(TIME_MOVE_ITEM + 0.1f, () => { checkLogicScore(isFull, listMatch); });
                if (listItemScored.Count != 0) this.Wait(TIME_MOVE_ITEM + 0.1f, () => {
                    runGUIScore(listItemScored);
                    runGUIFragment(listMove);

                    if (listResult.Count == listItem.Count)
                    {
                        //handleEndGame(Enums.MiniGameEndGame.WIN);
                    }
                });
            }

        }
        private Vector2 getPos(SlotController slotContainer, ItemController item)
        {
            Vector2 pos = slotContainer.transform.position;
            float width = 60f;
            float height = 60f; // coi lai cho nay
            float sizeBgSlot = Game4Constants.SIZE_ITEM_SLOT;

            float scale = 0.75f;
            item.transform.localScale = Vector3.one * scale;
            pos.y -= height / 100 /70;//dua xuong center          
            item.KillTweenScale();
            return pos;
        }
        private bool isFullSlot()
        {
            int length = listSlotTarget.Count;
            int count = 0;
            for (int i = 0; i < length; i++)
            {
                SlotController slotController = listSlotTarget[i];
                if (slotController.Id != 0) count++;
            }
            return count == listSlotTarget.Count;
        }
        private List<SlotController> getListMatch(SlotController slotController)
        {
            int count = listSlotTarget.Count;
            List<SlotController> result = new List<SlotController>();
            for (int i = slotController.Index; i < count; i++)
            {
                SlotController slot = listSlotTarget[i];
                if (slot.Id != 0 && slot.Id == slotController.Id && slotController.IdCheckMatch == -1) result.Add(slot);
            }
            return result;
        }
        private void checkLogicScore(bool isFull, List<SlotController> listMatch, out List<ItemController> listItemScored, out List<DataMove> listMove)
        {
            listItemScored = new List<ItemController>();
            listMove = new List<DataMove>();
            int count = listSlotTarget.Count;
            bool haveScore = false;
            if (listMatch.Count >= 3)
            {
                haveScore = true;
                for (int j = 0; j < listMatch.Count; j++)
                {
                    SlotController slotItem = listMatch[j];
                    listResult.Add(slotItem.Item);
                    listItemScored.Add(slotItem.Item);
                    slotItem.UpdateItem(null);
                }
                ALLOW_ACTION = true;
                //don` hang
                checkFragment(out listMove);

                //CheckCombo(); // bat

            }
            if (!haveScore)
            {
                if (isFull)
                {
                    Debug.LogError("checkLogicScore " + isFull);
                    ALLOW_ACTION = false;
                    //handleEndGame(Enums.MiniGameEndGame.LOSE); // bat
                }
            }
            else
            {
                ALLOW_ACTION = true;
            }
        }
        private void checkFragment(out List<DataMove> listItemMove)
        {
            listItemMove = new List<DataMove>();
            int count = listSlotTarget.Count;
            //fragment
            for (int i = 0; i < count; i++)
            {
                SlotController slotController = listSlotTarget[i];
                if (slotController.Item != null)
                {
                    SlotController emptySlot = getFirstSlotEmpty();
                    if (emptySlot.Index < slotController.Index)
                    {
                        emptySlot.UpdateItem(slotController.Item);
                        if (slotController.Item != null)
                        {
                            Vector2 pos = getPos(emptySlot, slotController.Item);
                            DataMove dataMove = new DataMove();
                            dataMove.Position = pos;
                            dataMove.Item = slotController.Item;
                            //slotController.Item.Move(pos, 0.1f);
                            listItemMove.Add(dataMove);
                        }
                        slotController.UpdateItem(null);
                    }
                }
            }

        }
        private void runGUIFragment(List<DataMove> listMove)
        {
            for (int i = 0; i < listMove.Count; i++)
            {
                DataMove dataMove = listMove[i];
                dataMove.Item.Move(dataMove.Position, 0.1f);
            }
        }
        private void runGUIScore(List<ItemController> listItemScored)
        {
            int count = listItemScored.Count;
            for (int i = 0; i < count; i++)
            {
                ItemController itemController = listItemScored[i];
                GameObject go = itemController.gameObject;
                System.Action onComplete = null;
                bool isFinal = i == 0;
                onComplete += () =>
                {
                };

                //if (i == 0) onComplete += checkFragment;
                itemController.DoDisAppear(onComplete);
            }

        }
        private SlotController getFirstSlotEmpty()
        {
            int count = listSlotTarget.Count;
            for (int i = 0; i < count; i++)
            {
                SlotController slotController = listSlotTarget[i];
                if (slotController.Item == null) return slotController;
            }
            return null;
        }

        private void removeOldSlot()
        {
            foreach (var item in listSlotTarget)
            {
                Destroy(item.gameObject);
            }
        }
        private void removeOldItem()
        {
            foreach (var item in listItem)
            {
                if (item != null) Destroy(item.gameObject);
            }
        }
        #region slots
        private void initSlot(int maxSlot)
        {
            GameUtils.ClearTransform(transformSlot);
            for (int i = 0; i < maxSlot; i++)
            {
                SlotController slotController = Instantiate(prefabSlot, transformSlot);
                slotController.name = "Slot_" + (i + 1);
                slotController.Index = i;
                listSlotTarget.Add(slotController);
                slotController.transform.localPosition = new Vector3(Game4Ultils.GetXOfSlot(i, maxSlot), 0, 0);
            }
        }
        private SlotController getBlankSlotForItem(ItemController item)
        {
            int countSlot = listSlotTarget.Count;
            SlotController lastSlot = null;
            for (int i = 0; i < countSlot; i++)
            {
                SlotController slot = listSlotTarget[i];
                if (slot.Id == 0)
                {
                    lastSlot = slot;
                    break;
                }
                else
                {
                    if (slot.Id == item.Id)
                    {
                        int nextIndex = i + 1;
                        if (nextIndex < countSlot)
                        {
                            SlotController nextSlot = listSlotTarget[nextIndex];
                            if (nextSlot.Id == 0) return nextSlot;
                            if (nextSlot.Id != item.Id) return nextSlot;
                        }
                    }
                }
            }
            return lastSlot;
        }
        #endregion
       
        private void restartGame()
        {
            initGame();
            //PopupController.Instance.ForceHidePopupEndGame(); bat
        }
        public struct DataMove
        {
            public ItemController Item;
            public Vector2 Position;
        }
        public static void ActiveStar(int star)
        {
            //PopupController.Instance.PopupEndGame?.ForceUpdateStar(star); // bat
        }
        public void OnExitClick_Editor()
        {
            goCanvasGamePlay.SetActive(false);
            StartCoroutine(MainGameController.Instance.onClickShowMapTileAndItem());
            goCanvasCreateMap.SetActive(true);

        }
        private void clearTranform(Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

}
