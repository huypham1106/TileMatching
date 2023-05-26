using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Minigame.Game4
{
    public class ItemController : MonoBehaviour
    {
        private const float TIME_TWEEN = 0.2f;
        int id;
        private int sortOrder = 50;
        //[SerializeField] BoxCollider2D itemCollider2D = null;
        public int MapIndex = 0;//0,1,2 ngoai vao trong
        CanvasGroup canvasGroup = null;
        Canvas canvas = null;
        public Image imgItem;
        [SerializeField] private Button btnItem = null;
        [SerializeField] private List<ItemController> listLock;
        [SerializeField] private List<ItemController> listBeLock;
        [SerializeField] bool initComplete = false;
        public System.Action<ItemController> OnClick;
        [SerializeField] private int intIdMove;
        bool isPlaying = false;
        int idTweenScale;
        public bool isEmpty;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponent<Canvas>();
        }
        private void Start()
        {
            btnItem.onClick.AddListener(onClickItem);
        }
        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public void InitData(int id, Sprite ImageItem)
        {
            Id = id;
            imgItem.sprite = ImageItem;
            imgItem.SetNativeSize();
        }
        public void SetMap(int depth)
        {
            this.MapIndex = depth;
            SetOrder(sortOrder + depth);
            //updateVisualByDepth();
        }
        public void SetOrder(int order)
        {
            canvas.sortingOrder = order; // bat
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsPlaying) return;
            ItemController collisionItem = collision.gameObject.GetComponent<ItemController>();
            if (collisionItem.IsPlaying) return;
            if (MapIndex < collisionItem.MapIndex)
            {
                //Debug.Log(depth + "  OnTriggerEnter2D " + collision.gameObject.GetComponent<ItemController>().depth);
                /*                this.canvasGroup.alpha = 0.4f;
                                collisionItem.UpdateLock(this);
                                UpdateBeLock(collisionItem);
                                initComplete = true;*/

                //this.canvasGroup.alpha = 0.4f;
                setColorItem(106, 106, 106, 255);
                collisionItem.UpdateBeLock(this);
                UpdateLock(collisionItem);
                initComplete = true;

            }

        }
        public void UpdateLock(ItemController itemController)
        {
            if (listLock == null) listLock = new List<ItemController>();
            listLock.Add(itemController);
        }
        public void UpdateBeLock(ItemController itemController)
        {
            if (listBeLock == null) listBeLock = new List<ItemController>();
            listBeLock.Add(itemController);
        }
        public void RemoveLock(ItemController itemController)
        {
            if (listLock == null) return;
            listLock.Remove(itemController);
            if (listLock.Count == 0)
            {
                //this.canvasGroup.alpha = 1f;
                setColorItem(255, 255, 255, 255);
            }
        }
        private void setColorItem(byte r, byte g, byte b, byte a)
        {
            btnItem.GetComponent<Image>().color = new Color32(r,g,b,a);
            imgItem.color = new Color32(r, g, b, a);
        }
/*        public float GetWidth()
        {
            //size = spriteRenderer.sprite.rect.width;
            return itemCollider2D.size.x * 100;
        }*/

        private void onClickItem()
        {
            if (!listLock.IsNullOrEmpty()) return;
            if (!MapManager.ALLOW_ACTION) return;

            if (listBeLock != null)
            {
                foreach (var item in listBeLock)
                {
                    item.RemoveLock(this);
                }
            }
            IsPlaying = true;
            OnClick?.Invoke(this);
        }
        public void ClearAction()
        {
            OnClick = null;
        }
        public void DoDisAppear(System.Action complete)
        {
            this.transform.DOScale(0, 0.3f).OnComplete(() => { complete?.Invoke(); });
        }
        public void Move(Vector3 pos, float time, System.Action onComplete = null)
        {
            if (intIdMove != 0)
            {
                DOTween.Complete(intIdMove, true);
            }
            intIdMove = gameObject.GetInstanceID();
            StartCoroutine(ieMove(pos, time, onComplete));
        }
        private IEnumerator ieMove(Vector3 pos, float time, System.Action onComplete)
        {
            yield return null;
            transform.DOMove(pos, time).SetId(intIdMove).OnComplete(() => {
                intIdMove = 0;
                onComplete?.Invoke();
            });
        }
        public void KillTweenScale()
        {
            if (idTweenScale != 0) DOTween.Kill(idTweenScale);
            idTweenScale = 0;
        }
    }


}
