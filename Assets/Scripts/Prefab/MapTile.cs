using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    public List<MapTile> listLock;
    public List<MapTile> listBeLock;
    [SerializeField] bool initComplete = false;
    [SerializeField] Button btnMapTile = null;
    public Image ImgItem = null;

    public int Depth = 0;//0,1,2 ngoai vao trong
    //Canvas canvas = null;
    CanvasGroup canvasGroup = null;

    private int sortOrder = 50;
    public bool isEmpty;
    private bool allowClick = true;
    private void Awake()
    {
        //canvas = this.GetComponent<Canvas>();
        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        btnMapTile.onClick.AddListener(onClickMapTile);
        isEmpty = false;
    }

    public Color getImgeBackGround()
    {
        return btnMapTile.GetComponent<Image>().color;
    }
    public void setImageBackGround(Color color)
    {
        btnMapTile.GetComponent<Image>().color = color;
    }
    private void onClickMapTile()
    {

        //if (listLock.Count != 0) return; coi lai cho nay
        /*        if (listBeLock != null)
                {
                    foreach (var item in listBeLock)
                    {
                        item.RemoveLock(this);
                    }
                }*/

        if (allowClick)
        {
            this.canvasGroup.alpha = 0f ;
            isEmpty = true;
            allowClick = false;
        }
        else
        {
            this.canvasGroup.alpha = 1f;
            isEmpty = false;
            allowClick = true;
        }
        StartCoroutine(MainGameController.Instance.onClickShowMapTileAndItem());
    }
    public void RemoveLock(MapTile mapTile)
    {
        if (listLock == null) return;
        listLock.Remove(mapTile);
        if (this.listLock.Count == 0)
        {
            this.canvasGroup.alpha = 1f;
        }
    }
/*    private void moveToForward()
    {
        depth--;
        (float, float) result = getAlphaAndScaleByDepth(depth);
        idTweenScale = spriteRenderer.GetInstanceID();
        spriteRenderer.transform.DOScale(result.Item2, TIME_TWEEN).SetDelay(0.1f).SetId(idTweenScale);
        spriteRenderer.DOFade(result.Item1, TIME_TWEEN).SetDelay(0.1f);
    }*/
    public void setDepth(int depth)
    {
        this.Depth = depth;
        SetOrder(sortOrder - depth);
        //updateVisualByDepth();
    }
    public void SetOrder(int order)
    {
        //canvas.sortingOrder = order;
    }
    private void updateVisualByDepth()
    {
        (float, float) result = getAlphaAndScaleByDepth(Depth);

        canvasGroup.alpha = result.Item1 ;
        //spriteRenderer.transform.localScale = Vector3.one * result.Item2;
    }
    private (float, float) getAlphaAndScaleByDepth(int depth)
    {
        if (depth == 1)
        {
            return (1, 1);
        }
        else 
        {
            return (0.2f, 1f);
        }
        return (1, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
/*        //if (IsPlaying) return;
        MapTile collisionItem = collision.gameObject.GetComponent<MapTile>();
        //if (collisionItem.IsPlaying) return;
        if (Depth > collisionItem.Depth)
        {
            //Debug.Log(depth + "  OnTriggerEnter2D " + collision.gameObject.GetComponent<ItemController>().depth);
            

            collisionItem.canvasGroup.alpha = 0.2f;
            collisionItem.UpdateLock(this);
            UpdateBeLock(collisionItem);
            initComplete = true;
        }*/

    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        //listLock.Clear();
        //listBeLock.Clear();
        //this.canvasGroup.alpha = 1f;
        /*        MapTile collisionItem = collision.gameObject.GetComponent<MapTile>();
                if (Depth < collisionItem.Depth)
                {
                    if (listBeLock != null)
                    {
                        foreach (var item in listBeLock)
                        {
                            item.RemoveLock(this);
                        }
                    }
                }*/
        //MapTile collisionItem = collision.gameObject.GetComponent<MapTile>();

/*            if (this.listBeLock != null)
            {

                foreach (var item in listBeLock)
                {
                    item.RemoveLock(this);
                }
                *//*                if (this.listLock.Count == 0)
                                {
                                    this.canvasGroup.alpha = 1f;
                                }*//*
            }*/



    }
    public void UpdateLock(MapTile mapTile)
    {
        if (listLock == null) listLock = new List<MapTile>();
        listLock.Add(mapTile);
    }
    public void UpdateBeLock(MapTile mapTile)
    {
        if (listBeLock == null) listBeLock = new List<MapTile>();
        listBeLock.Add(mapTile);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
