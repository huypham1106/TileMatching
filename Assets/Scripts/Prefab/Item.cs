using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Image imgItem = null;
    public InputField ifAmount = null;
    public int Id;
    public int Amount;

    public void InitItem(Sprite itemImage)
    {
        imgItem.sprite = itemImage;
        imgItem.SetNativeSize();
        Id = int.Parse(imgItem.sprite.name);
    }    

    public void onValueChangeIfAmounr_editor(string value)
    {
        if (value == "")
        {
            ifAmount.text = "0";
            Amount = 0;
        }
        else
        {
            int amountTemp = int.Parse(value);
            if (amountTemp % 3 == 0)
            {
                Amount = int.Parse(value);
            }
            else
            {
                ifAmount.text = "0";
            }
        }
        StartCoroutine( MainGameController.Instance.onClickShowMapTileAndItem());
    }    

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
