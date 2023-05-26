using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    public float WidthTotal;
    public float HeightTotal;
    public int Width;
    public int Height;
    public int Depth;
    public List<MapTile> mapTileList = new List<MapTile>();

    void Start()
    {
        
    }

    private void OnEnable()
    {
        for(int i = 0; i<mapTileList.Count; i++)
        {
            mapTileList[i].GetComponent<CanvasGroup>().alpha = 1f;
            mapTileList[i].listLock.Clear();
            mapTileList[i].listBeLock.Clear();
        }    
    }

    private void OnDisable()
    {
        if (!gameObject.activeSelf) return;    

/*        Map[] mapResetList = this.gameObject.transform.parent.transform.GetComponentsInChildren<Map>();
        for (int i = 0; i < mapResetList.Length; i++)
        {
            mapResetList[i].gameObject.SetActive(false);
            mapResetList[i].gameObject.SetActive(true);
            for (int j = 0; j < mapResetList[i].mapTileList.Count; j++)
            {

                if (mapResetList[i].mapTileList[j].isEmpty == true)
                {
                    mapResetList[i].mapTileList[j].GetComponent<CanvasGroup>().alpha = 0f;
                }
                else
                {
                    mapResetList[i].mapTileList[j].GetComponent<CanvasGroup>().alpha = 1f;
                }
                mapResetList[i].mapTileList[j].listLock.Clear();
                mapResetList[i].mapTileList[j].listBeLock.Clear();
            }


        }*/
    }

    public void initData(float widthTotal, float heightTotaL, int width, int height)
    {
        WidthTotal = widthTotal;
        HeightTotal = heightTotaL;
        Width = width;
        Height = height;
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
