using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectScaleClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public System.Action HandleClick;
    public System.Action<PointerEventData> HandleDown;
    [SerializeField]
    private float oldScale;
    private bool isPointerDown;
    public bool AllowPointer = true;
    private static long lastTime;
    private void Awake()
    {
        oldScale = transform.localScale.x;
        isPointerDown = false;
    }

    public void RunEffectPointDown(float scale = 0.9f, float time = 0.1f)
    {
        if (!AllowPointer) return;
        transform.DOScale(oldScale * scale, time);
    }

    public void RunEffectPointUp(float scale = 0.9f, float time = 0.1f)
    {
        if (!AllowPointer) return;
        transform.DOScale(oldScale, time);
    }

    public void RunEffectPointClick(float scale = 0.9f, float time = 0.1f)
    {
        if (!AllowPointer) return;
        transform.DOScale(oldScale * scale, time).SetEase(Ease.OutExpo).OnComplete(() => {
            transform.DOScale(oldScale, time);
        });
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!AllowPointer) return;
        long now = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (lastTime + 10 > now) return;
        lastTime = now;
        RunEffectPointUp();
        HandleClick?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!AllowPointer) return;
        isPointerDown = false;
        RunEffectPointUp();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!AllowPointer)
        {
            HandleDown?.Invoke(eventData);
            return;
        }
        isPointerDown = true;
        RunEffectPointDown();
        HandleDown?.Invoke(eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (isPointerDown) transform.DOScale(oldScale, 0.1f).SetEase(Ease.OutBounce);

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (isPointerDown) RunEffectPointDown();

    }

    public void SetOldScale(float scale)
    {
        oldScale = scale;
    }

    private void OnDisable()
    {
        isPointerDown = false;
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.one * oldScale;
    }
}

