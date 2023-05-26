using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using SimpleFileBrowser;
using FullSerializer;
using UnityEngine.UI;
using Minigame.Game4;
public class MainGameController : MonoBehaviour
{
    private Level currentLevel;

    [Header("Information")]
    [SerializeField] private InputField ifSlots = null;
    [SerializeField] private InputField ifStar1 = null;
    [SerializeField] private InputField ifStar2 = null;
    [SerializeField] private InputField ifStar3 = null;
    [SerializeField] private InputField ifTime = null;
    [SerializeField] private Dropdown ddPosition = null;


    [Header("Item")]
    [SerializeField] private Item item = null;
    [SerializeField] private GameObject goItemContent = null;
    [SerializeField] private Sprite[] listItemSprite;
    public Dictionary<string, Sprite> DictBubbleSprite = new Dictionary<string, Sprite>();

    [Header("Map")]
    [SerializeField] private GameObject goDepthContent = null;
    [SerializeField] private Map mapPrefab = null;
    [SerializeField] private InputField ifWidth = null;
    [SerializeField] private InputField ifHeight = null;
    [SerializeField] private Button btnGenerateMap = null;
    [SerializeField] private MapTile mapTilePrefab = null;
    [SerializeField] private GameObject goMapSelectionContent = null;
    [SerializeField] private MapSelection mapSelection = null;
    [SerializeField] private Button btnEditMap = null;

    public Vector3 vectorScale = new Vector3();
    List<Map> mapList = new List<Map>();
    List<MapSelection> mapSelectionList = new List<MapSelection>();
    private int width;
    private int height;
    private float widthContent;
    private float heightContent;
    private int mapIndex = 1;
    private const int maxMapContent = 832; 

    [Header("Other")]
    [SerializeField] private Button btnNew = null;
    [SerializeField] private Button btnOpen = null;
    [SerializeField] private Button btnSave = null;
    [SerializeField] private Button btnPlayGame = null;
    [SerializeField] private Button btnCopy = null;
    public Text txtMapTile = null;
    public Text txtItem = null;
    public string jsonData;

    private bool allowReset = false;
    private string txtPathFolderAsset = "";
    private string pathfoldersave = "C:\\";

    [Header("GamePlay")]
    [SerializeField] private GameObject goCanvasGamePlay;
    [SerializeField] private GameObject goCanvasCreateMap;

    public static MainGameController Instance { get; private set; }
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
        currentLevel = new Level();
        listItemSprite = Resources.LoadAll<Sprite>("items");
        goCanvasGamePlay.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        btnGenerateMap.onClick.AddListener(onClickGenerateMap);
        btnEditMap.onClick.AddListener(onClickEditMap);
        btnOpen.onClick.AddListener(LoadAsFile);
        btnSave.onClick.AddListener(SaveAsFile);
        btnNew.onClick.AddListener(onClickNewData);
        btnPlayGame.onClick.AddListener(onClickPlayGame);
        btnCopy.onClick.AddListener(CopyToClipboard);
        InitItem();

        txtPathFolderAsset = Application.dataPath + "/StreamingAssets/pathfoldersave.txt";
        if (File.Exists(txtPathFolderAsset))
        {
            string content = File.ReadAllText(txtPathFolderAsset);
            pathfoldersave = content;
        }
        onClickNewData();
    }

    #region Other

    private void onClickPlayGame()
    {
        if(txtItem.text != txtMapTile.text)
        {
            Toast.ShowMessage("** ** The Map Tile not equal Item (Amount) ** **", Toast.Position.top, Toast.Time.twoSecond);
            return;
        }
        if (txtItem.text == "0"  || txtMapTile.text == "0")
        {
            Toast.ShowMessage("** ** The Map Tile or Item not null ** **", Toast.Position.top, Toast.Time.twoSecond);
            return;
        }




        saveLevelData();
        SaveJson(currentLevel);
    }
    void CopyToClipboard()
    {
        saveLevelData();
        string json = saveJsonToCopy(currentLevel);
        if (json != null || json != "")
        {
            TextEditor te = new TextEditor();
            te.text = json;
            te.SelectAll();
            te.Copy();
            Toast.ShowMessage("** Copied **", Toast.Position.top, Toast.Time.twoSecond);
        }
    }
    public IEnumerator onClickShowMapTileAndItem()
    {
        yield return new WaitForSeconds(0.1f);
        Map[] mapListShowing = goDepthContent.transform.GetComponentsInChildren<Map>();
        int mapTile = 0;
        for(int i=0; i<mapListShowing.Length; i++)
        {
            //mapTile += mapListShowing[i].mapTileList.Count;
            for(int j= 0; j<mapListShowing[i].mapTileList.Count; j++)
            {
                if (mapListShowing[i].mapTileList[j].isEmpty == false)
                {
                    mapListShowing[i].mapTileList[j].GetComponent<CanvasGroup>().alpha = 1f;
                    mapTile += 1;
                }
                else
                {
                    mapListShowing[i].mapTileList[j].GetComponent<CanvasGroup>().alpha = 0f;
                }    
            }    
        }

        int item = 0;
        Item[] itemList = goItemContent.transform.GetComponentsInChildren<Item>();
        for(int j=0; j<itemList.Length; j++)
        {
            item += itemList[j].Amount;
        }

        txtItem.text = item.ToString();
        txtMapTile.text = mapTile.ToString();
    }    
   
    private void onClickNewData()
    {
        goCanvasGamePlay.SetActive(false);
        goCanvasCreateMap.SetActive(true);
        currentLevel = new Level();
        allowReset = true;
        loadLevelData(currentLevel);
    }

    public void SaveAsFile()
    {
        if (int.Parse(txtMapTile.text) ==0 && int.Parse(txtItem.text) == 0)
        {
            Toast.ShowMessage("** The Map Tile and Item is null (Amount) **", Toast.Position.top, Toast.Time.threeSecond);
        }

        else  if (int.Parse(txtMapTile.text) != int.Parse(txtItem.text) )
        {
            Toast.ShowMessage("** The Map Tile not equal Item (Amount) **", Toast.Position.top, Toast.Time.threeSecond);
        }    
        else
        FileBrowser.ShowSaveDialog((paths) => onSuccessSave(paths), null, FileBrowser.PickMode.Files, false, pathfoldersave, "name.json", "Save As", "Save");
    }
    private void onSuccessSave(string[] paths)
    {
        if (paths.Length > 0)
        {
            pathfoldersave = Path.GetDirectoryName(paths[0]);
            saveLevelData();
            SaveJsonFile(FileBrowser.Result[0], currentLevel);
        }
    }

    public void LoadAsFile()
    {
        FileBrowser.ShowLoadDialog((paths) => onSuccessLoad(paths), null, FileBrowser.PickMode.Files, false, pathfoldersave, "", "Load File", "Load");
    }
    private void onSuccessLoad(string[] paths)
    {
        if (paths.Length > 0)
        {
            pathfoldersave = Path.GetDirectoryName(paths[0]);
            currentLevel = LoadJsonFile<Level>(FileBrowser.Result[0]);
            loadLevelData(this.currentLevel);
        }
    }

    #endregion

    #region Information


    #endregion

    #region Item
    private void InitItem()
    {
        for(int i=0; i< listItemSprite.Length; i++)
        {
            var itemIns = Instantiate(item);
            itemIns.transform.SetParent(goItemContent.transform, false);
            itemIns.InitItem(listItemSprite[i]);

        }    
    }


    #endregion

    #region Map
    //
    //private void 

  

/*     public void onValueChangeDepth_edittor(string value)
    {
        int valueDepth = int.Parse(value);
        int mapCreated = goDepthContent.transform.childCount;
        int map = valueDepth - goDepthContent.transform.childCount;
        if (valueDepth > 0)
        {
            ddDepth.gameObject.SetActive(true);
            ddDepth.ClearOptions();
            for (int i = 1; i <= valueDepth; i++)
            {
                ddDepth.options.Add(new Dropdown.OptionData(i.ToString()));
            }
            ddDepth.RefreshShownValue();

            if (map > 0)
            {
                for (int j = mapCreated + 1; j <= valueDepth; j++)
                {
                    var mapIns = Instantiate(mapPrefab);
                    mapIns.transform.SetParent(goDepthContent.transform, false);
                    //depthIns.transform.SetAsFirstSibling();
                    mapIns.name = "Map_" + j;
                    mapIns.Depth = j;
                    mapList.Add(mapIns);

                    var mapSelectionIns = Instantiate(mapSelection);
                    mapSelectionIns.transform.SetParent(goMapSelectionContent.transform, false);
                    mapSelectionIns.initData(j, mapIns);
                    mapSelectionList.Add(mapSelectionIns);
                }
            }
            else if(map < 0)
            {
                for(int j = 0; j < Mathf.Abs(map); j++)
                {
                    var childMap = goDepthContent.transform.GetChild(mapList.Count-1);
                    mapList.Remove(childMap.GetComponent<Map>());
                    Destroy(childMap.gameObject);

                    var childMapSelection = goMapSelectionContent.transform.GetChild(mapSelectionList.Count - 1);
                    mapSelectionList.Remove(childMapSelection.GetComponent<MapSelection>());
                    Destroy(childMapSelection.gameObject);
                }  
            }
            ActiveMap(false, true);
            checkMapPosition();
        }
        else
        {
            ddDepth.gameObject.SetActive(false);
        }    
     }*/

    public void checkMapPosition()
    {
        string position = ddPosition.options[ddPosition.value].text;
        float point = 0;
        for (int i = mapList.Count - 2; i >= 0; i--)
        {
            mapList[i].transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        if (position == "Up")
        {

            for (int i = 1; i < mapList.Count; i++)
            {
                point += 6f;
                mapList[i].transform.localPosition = new Vector3(mapList[i].transform.localPosition.x, point, mapList[i].transform.localPosition.z);
            }
        }
        else if (position == "Down")
        {

            for (int i = 1; i < mapList.Count; i++)
            {
                point -= 6f;
                mapList[i].transform.localPosition = new Vector3(mapList[i].transform.localPosition.x, point, mapList[i].transform.localPosition.z);
            }
        }
        else if (position == "Left")
        {
            for (int i = 1; i < mapList.Count; i++)
            {
                point -= 6f;
                mapList[i].transform.localPosition = new Vector3(point, mapList[i].transform.localPosition.y, mapList[i].transform.localPosition.z);
            }
        }
        else if (position == "Right")
        {
            for (int i = 1; i < mapList.Count; i++)
            {
                point += 6f;
                mapList[i].transform.localPosition = new Vector3(point, mapList[i].transform.localPosition.y, mapList[i].transform.localPosition.z);
            }
        }  
    }

    private void onClickEditMap()
    {
        Map[] mapListTemp = goDepthContent.transform.GetComponentsInChildren<Map>();
        if(mapListTemp.Length > 1)
        {
            Toast.ShowMessage("** Please choose only 1 map to edit **", Toast.Position.top, Toast.Time.threeSecond);
        }
        else if (mapListTemp.Length == 1)
        {
            for(int i=0;i<mapList.Count; i++)
            {
                if(mapList[i].Depth == mapListTemp[0].Depth)
                {
                    Color color = mapList[i].mapTileList[0].getImgeBackGround();
                    StartCoroutine(generateMap(i, color));
                    break;
                }
            }

            
        }

    }

    private void onClickGenerateMap()
    {
        if (ifWidth.text != null && ifHeight.text != null)
        {
            int.TryParse(ifWidth.text, out width);
            int.TryParse(ifHeight.text, out height);

            if (width > 0 && height > 0)
            {
                var mapIns = Instantiate(mapPrefab);
                mapIns.transform.SetParent(goDepthContent.transform, false);

                var mapSelectionIns = Instantiate(mapSelection);
                mapSelectionIns.transform.SetParent(goMapSelectionContent.transform, false);
                Color newColor = randomColor();
                //depthIns.transform.SetAsFirstSibling();
                if (mapList.Count > 0)
                {
                    mapIns.name = "Map_" + (mapList[mapList.Count - 1].Depth + 1);
                    mapIns.Depth = mapList[mapList.Count - 1].Depth + 1;
                    mapSelectionIns.initData(mapList[mapList.Count - 1].Depth + 1, mapIns, newColor);
                }
                else
                {
                    mapIns.name = "Map_" + mapIndex;
                    mapIns.Depth = mapIndex;
                    mapSelectionIns.initData(mapIndex, mapIns, newColor);
                }
                mapList.Add(mapIns);
                mapSelectionList.Add(mapSelectionIns);

                StartCoroutine(generateMap((mapList.Count - 1), newColor));
            }
        }
    }    
    IEnumerator generateMap(int mapIndex, Color color)
    {
        if (ifWidth.text != null && ifHeight.text != null)
        {
            int.TryParse(ifWidth.text, out width);
            int.TryParse(ifHeight.text, out height);

            if (width > 0 && height > 0)
            {

                //Transform mapContent = goDepthContent.transform.GetChild(ddDepth.value).transform;
                Transform mapContent = mapList[mapIndex].transform;
                //Transform mapContent = goDepthContent.transform.GetComponentInChildren<Map>().Depth

                clearTranform(mapContent);
                yield return new WaitForSeconds(0.1F);
                mapContent.transform.localScale = new Vector3(1f, 1f, 1f);
                //txtScaleMap.text = "1.1x";
                //sdScaleMap.value = 1.1f;



                int count = width * height;

                float posX = 0;
                float posY = 0;
                float a = mapTilePrefab.GetComponent<RectTransform>().rect.width;
                mapContent.GetComponent<Map>().mapTileList.Clear();
                for (int i = 0; i < height; i++)
                {
                    posY = height * a * 1.0f / 2 - (i + 0.5f) * a * 1.0f;
                    for (int j = 0; j < width; j++)
                    {
                        posX = (j + 0.5f) * a * 1.0f - width * a * 1.0f / 2;

                        var copy = Instantiate(mapTilePrefab);
                        copy.transform.SetParent(mapContent.transform, false);
                        copy.transform.localPosition = new Vector3(posX, posY, 0f);
                        copy.setDepth(mapContent.GetComponent<Map>().Depth);
                        copy.name = "Item_" + copy.ImgItem.sprite.name + "_" + copy.Depth + "_" + (copy.transform.GetSiblingIndex() + 1);
                        copy.setImageBackGround(color);

                        mapContent.GetComponent<Map>().mapTileList.Add(copy);


                    }
                }


                widthContent = 104f * width;
                heightContent = 104f * height;
                mapContent.GetComponent<Map>().initData(widthContent, heightContent, width, height);
                //mapContent.GetComponent<Map>().WidthTotal = widthContent;
                checkWidthAndHeightDepthContent();
            }
        }
        StartCoroutine(onClickShowMapTileAndItem());
    }  

    private Color randomColor()
    {
        Color newColor = new Color(Random.Range(0f, 1f),
                                    Random.Range(0f, 1f),
                                    Random.Range(0f, 1f));
        return newColor;
    }
    
    public void onClickDeleteMap(int mapIndex)
    {
        //MapSelection[] mapSelectionList = goMapSelectionContent.transform.GetComponentsInChildren<MapSelection>();
/*        List<int> indexMap = new List<int>();
        int count = mapSelectionList.Count-1;
        for (int i= count; i>= 0; i--)
        {
            if(mapSelectionList[i].tgMapSelection.isOn)
            {
                Map mapTemp = mapSelectionList[i].map;
                mapList.Remove(mapTemp);
                Destroy(mapTemp.gameObject);

                MapSelection mapSelectionTemp = mapSelectionList[i];
                mapSelectionList.Remove(mapSelectionTemp);
                Destroy(mapSelectionTemp.gameObject);


            }    
        }*/

        for(int i=0; i<mapList.Count; i++)
        {
            if(mapList[i].Depth == mapIndex)
            {
                Map mapTemp = mapList[i];
                mapList.Remove(mapTemp);
                Destroy(mapTemp.gameObject);
            }
        }
        

    }    
    public void checkWidthAndHeightDepthContent()
    {
        float widthDepthContent = 0;
        float heightDepthContent = 0;
        for(int i= 0; i< mapList.Count; i++)
        {
            if(widthDepthContent < mapList[i].WidthTotal)
            {
                widthDepthContent = mapList[i].WidthTotal;
            }
            if (heightDepthContent < mapList[i].HeightTotal)
            {
                heightDepthContent = mapList[i].HeightTotal;
            }
        }

        if(widthDepthContent > maxMapContent || heightDepthContent > maxMapContent)
        {
            float width = widthDepthContent / 104;
            float height = heightDepthContent / 104;
             float max = Mathf.Max(width, height);
            max = max - 8;
            float scale = 1f - (max * 0.1f);
            vectorScale = new Vector3(scale, scale, scale);
        }    
        else
        {
            vectorScale = new Vector3(1f, 1f, 1f);
        }
        goDepthContent.transform.localScale = vectorScale;

        //goDepthContent.GetComponent<RectTransform>().sizeDelta = new Vector2(widthDepthContent, heightDepthContent);
    }

/*    public void onValueChangeDdDepth_editor()
    {
        string mapIndex = ddDepth.options[ddDepth.value].text;
        ActiveMap(false, false);
        for (int i = 0; i < mapList.Count; i++)
        {
            if (mapList[i].Depth == int.Parse(mapIndex))
            {
                mapList[i].gameObject.SetActive(true);
                if(mapList[i].Width != 0 && mapList[i].Height !=0)
                {
                    ifHeight.text = mapList[i].Height.ToString();
                    ifWidth.text = mapList[i].Width.ToString();
                }   
                else
                {
                    ifWidth.text = "0";
                    ifHeight.text = "0";
                }    
                break;
            }    
        }
    }   */ 

    public void onValueChangeTgShowAll_editor(bool isShowAll)
    {
        if(isShowAll)
        {
            ActiveMap(true, true);
            for(int i= 0; i<mapSelectionList.Count; i++)
            {
                mapSelectionList[i].tgMapSelection.isOn = true;

            }    

        }   
        else
        {
            ActiveMap(false, true);
            for (int i = 1; i < mapSelectionList.Count; i++)
            {
                
                mapSelectionList[i].tgMapSelection.isOn = false;
            }
        }    
    }    
    private void ActiveMap(bool isActive, bool isFristActive)
    {
        if (mapList.Count > 0)
        {

            for (int i = 0; i < mapList.Count; i++)
            {
                mapList[i].gameObject.SetActive(isActive);
                for (int j = 0; j < mapList[i].mapTileList.Count; j++)
                {
                    if (mapList[i].mapTileList[j].isEmpty == true)
                    {
                        mapList[i].mapTileList[j].GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }


            }
            if (isFristActive)
            {
                mapList[0].gameObject.SetActive(isFristActive);
                for (int i = 0; i < mapList[0].mapTileList.Count; i++)
                {
                    if (mapList[0].mapTileList[i].isEmpty == true)
                    {
                        mapList[0].mapTileList[i].GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }

            }

        }
    }

    #endregion

    #region SaveData
    private void saveLevelData()
    {
        currentLevel.slots = int.Parse(ifSlots.text);
        saveStarsObject();
        currentLevel.time = int.Parse(ifTime.text);
        currentLevel.positon = ddPosition.options[ddPosition.value].text;

        saveMapOject();
        saveItemObject();
    }    
    private void saveStarsObject()
    {
        currentLevel.stars = new List<int>();
        currentLevel.stars.Add(int.Parse(ifStar1.text));
        currentLevel.stars.Add(int.Parse(ifStar2.text));
        currentLevel.stars.Add(int.Parse(ifStar3.text));
    }    
    private void saveMapOject()
    {
        currentLevel.map = new List<MapJson>();
        List<int> listMapChoose = new List<int>();
        for(int i=0; i<mapSelectionList.Count; i++)
        {
            if(mapSelectionList[i].tgMapSelection.isOn)
            {
                listMapChoose.Add(mapSelectionList[i].MapIndex);
            }    
        }    


        for(int i= 0; i<mapList.Count; i++)
        {
            if (listMapChoose.Contains(mapList[i].Depth))
            {
                MapJson mapJson = new MapJson();
                mapJson.mapTile = new List<MapTileJson>();

                mapJson.width = mapList[i].Width;
                mapJson.height = mapList[i].Height;

                List<MapTile> mapTileList = mapList[i].mapTileList;
                for (int j = 0; j < mapTileList.Count; j++)
                {
                    MapTileJson mapTileJson = new MapTileJson();
                    mapTileJson.isEmpty = mapTileList[j].isEmpty;
                    mapJson.mapTile.Add(mapTileJson);
                }
                currentLevel.map.Add(mapJson);
            }

        }    
    }   
    private void saveItemObject()
    {
        Item[] itemList = goItemContent.transform.GetComponentsInChildren<Item>();
        currentLevel.items = new List<ItemJson>();
        List<ItemJson> Items = new List<ItemJson>();
        for (int i=0; i<itemList.Length; i++)
        {
            if (itemList[i].Amount == 0) continue;
            ItemJson itemJson = new ItemJson();
            itemJson.q = itemList[i].Amount;
            itemJson.id = itemList[i].Id;
            currentLevel.items.Add(itemJson); 
        }    
    }

    #endregion

    #region loadData
    private void loadLevelData(Level currentLevel)
    {
        if( currentLevel.slots > 0)
        ifSlots.text = currentLevel.slots.ToString();
        else
        {
            ifSlots.text = "7";
        }
        if (currentLevel.stars.Count > 0)
        {
            ifStar1.text = currentLevel.stars[0].ToString();
            ifStar2.text = currentLevel.stars[1].ToString();
            ifStar3.text = currentLevel.stars[2].ToString();
            
        }
        else
        {
            ifStar1.text = "0";
            ifStar2.text = "0";
            ifStar3.text = "0";
        }
        ifTime.text = currentLevel.time.ToString();
        if (currentLevel.positon == null)
        {
            ddPosition.value = ddPosition.options.FindIndex(option => option.text == "Nothing");
        }
        else
        {
            ddPosition.value = ddPosition.options.FindIndex(option => option.text == currentLevel.positon);
        }
        if (currentLevel.map.Count != 0)
        {
            StartCoroutine(loadMapObject(currentLevel));
        }
        else
        {
            mapList.Clear();
            clearTranform(goDepthContent.transform);
            clearTranform(goMapSelectionContent.transform);
            ifHeight.text = "";
            ifWidth.text = "";
            txtItem.text = "0";
            txtMapTile.text = "0";

        }    
        loadItemObject(currentLevel);
        StartCoroutine(onClickShowMapTileAndItem());
    }    

    IEnumerator loadMapObject(Level currentLevel)
    {
        
            //onValueChangeDepth_edittor(currentLevel.Map.Count.ToString());
            yield return new WaitForSeconds(0.1f);
            int indexMap = 0;
            for (int i = 0; i < currentLevel.map.Count; i++)
            {
                if (currentLevel.map[i].width < 0 && currentLevel.map[i].height < 0) break;

                ifWidth.text = currentLevel.map[i].width.ToString();
                ifHeight.text = currentLevel.map[i].height.ToString();

                onClickGenerateMap();
                //StartCoroutine(generateMap(indexMap));
                yield return new WaitForSeconds(0.1f);
                List<MapTileJson> mapTileList = currentLevel.map[i].mapTile;
                for (int j = 0; j < mapTileList.Count; j++)
                {
                    mapList[indexMap].mapTileList[j].isEmpty = mapTileList[j].isEmpty;
                    if (mapList[indexMap].mapTileList[j].isEmpty == true)
                    {
                        mapList[indexMap].mapTileList[j].gameObject.SetActive(false);
                    }
                }
                indexMap++;           
            }

        //ActiveMap(false, true);
    }   
    private void loadItemObject(Level currentLevel)
    {
        Item[] itemList = goItemContent.transform.GetComponentsInChildren<Item>();
        if(currentLevel.items.Count == 0)
        {
            for (int j = 0; j < itemList.Length; j++)
            {
                itemList[j].ifAmount.text = "0";
                itemList[j].Amount = 0;
            }
            return;
        }    
        for(int i = 0; i<currentLevel.items.Count; i++)
        {
            for(int j = 0; j<itemList.Length; j++)
            {
                if(currentLevel.items[i].id == itemList[j].Id)
                {
                    itemList[j].ifAmount.text = currentLevel.items[i].q.ToString();
                    itemList[j].Amount = currentLevel.items[i].q;
                    break;
                }  
            }    
        }
    }
    #endregion

    public void SaveJson(Level data)
    {

        fsData serializedData;
        var serializer = new fsSerializer();
        fsResult result = serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        var json = fsJsonPrinter.PrettyJson(serializedData);
        Debug.Log(json.ToString());
        jsonData = "";
        jsonData = json.ToString();
        if(jsonData!= "" )
        {
            Toast.ShowMessage("Load Game Success !!!!", Toast.Position.top, Toast.Time.twoSecond);
            goCanvasCreateMap.SetActive(false);
            goCanvasGamePlay.SetActive(true);
            MapManager.Instance.valueTest = jsonData;
            MapManager.Instance.initGame();

        }    
        else
        {
            Toast.ShowMessage("Pls check data is not empty !!!!", Toast.Position.top, Toast.Time.twoSecond);
            goCanvasGamePlay.SetActive(false) ;
            goCanvasCreateMap.SetActive(true);
        }    
    }
    private string saveJsonToCopy(Level data)
    {
        fsData serializedData;
        var serializer = new fsSerializer();
        fsResult result = serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        if(!result.Failed)
        {
            var json = fsJsonPrinter.PrettyJson(serializedData);
            Debug.Log(json.ToString());
            jsonData = "";
            jsonData = json.ToString();
            return jsonData;
        }
        else
        {
            Toast.ShowMessage("Pls check data is not empty !!!!", Toast.Position.top, Toast.Time.twoSecond);
        }    
        return null;


    }    
   
    protected void SaveJsonFile<T>(string path, T data) where T : class
    {
        fsData serializedData;
        var serializer = new fsSerializer();
        fsResult result = serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        if (result.Failed)
        {
            Toast.ShowMessage("Save error !!!!", Toast.Position.top, Toast.Time.threeSecond);
        }
        else
        {
            Toast.ShowMessage("** Save successfully **", Toast.Position.top, Toast.Time.threeSecond);
        }
        var file = new StreamWriter(path);
        var json = fsJsonPrinter.PrettyJson(serializedData);
        Debug.Log(json.ToString());
        file.WriteLine(json);
        file.Close();
    }

    protected T LoadJsonFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var file = new StreamReader(path);
        var fileContents = file.ReadToEnd();
        var data = fsJsonParser.Parse(fileContents);
        object deserialized = null;
        var serializer = new fsSerializer();
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
        file.Close();
        return deserialized as T;
    }

    private void clearTranform(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    private string removesapce(string text)
    {
        string textConvert = text.Replace(" ", string.Empty);
        return textConvert;
    }
}
