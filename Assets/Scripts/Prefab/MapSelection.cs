using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{
    public int MapIndex;
    public Map map;
    [SerializeField] private Text txtLabel = null;
    public Toggle tgMapSelection = null;
    [SerializeField] private Button btnDelete = null;
    public Image imgColor = null;

    // Start is called before the first frame update
    void Start()
    {
        btnDelete.onClick.AddListener(onClickDelete);
    }

    private void onClickDelete()
    {
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        MainGameController.Instance.onClickDeleteMap(this.MapIndex);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MainGameController.Instance.onClickShowMapTileAndItem());
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
        MainGameController.Instance.checkWidthAndHeightDepthContent();

    }    

    public void initData(int mapIndex, Map map, Color color)
    {

        this.MapIndex = mapIndex;
        this.map = map;
        gameObject.name = "MapSelect_" + mapIndex;
        txtLabel.text = mapIndex.ToString();
        imgColor.color = color;

    }    
    public void onValueChangeMapSelection_editor(bool isActive)
    {
        if(map != null)
        map.gameObject.SetActive(isActive);

        for(int i=0; i<map.mapTileList.Count; i++)
        {
            if(map.mapTileList[i].isEmpty == true)
            {
                map.mapTileList[i].GetComponent<CanvasGroup>().alpha = 0f;
            }    
        }
        StartCoroutine(MainGameController.Instance.onClickShowMapTileAndItem());

    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
