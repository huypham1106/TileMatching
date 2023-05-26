using System;
using UnityEngine;
using UnityEngine.UI;
public class Toast : MonoBehaviour {

    public enum Time
    {
        threeSecond,
        twoSecond,
        oneSecond
    };
    public enum Position
    {
        top,
        bottom
    };
    public static void ShowMessage ( string msg, Toast.Position position, Toast.Time time )
    {
        GameObject messagePrefab = Resources.Load ( "Message" ) as GameObject;
        GameObject containerObject = messagePrefab.gameObject.transform.GetChild ( 0 ).gameObject;
        GameObject textObject = containerObject.gameObject.transform.GetChild ( 0 ).GetChild ( 0 ).gameObject;
        Text msg_text = textObject.GetComponent<Text> ( );
        msg_text.text = msg;
        SetPosition ( containerObject.GetComponent<RectTransform> ( ), position );
        GameObject clone = Instantiate ( messagePrefab );
      //  LeanTween.move(clone, new Vector3(0.5f, 1000f, 0), 10);
        RemoveClone ( clone, time );
    }

    private static void SetPosition ( RectTransform rectTransform, Position position )
    {
        if (position == Position.top)
        {
            rectTransform.anchorMin = new Vector2 ( 0.5f, 1f );
            rectTransform.anchorMax = new Vector2 ( 0.5f, 1f );
            rectTransform.anchoredPosition = new Vector3 ( 0.5f, -100f, 0 );
        }
        else
        {
            rectTransform.anchorMin = new Vector2 ( 0.5f, 0 );
            rectTransform.anchorMax = new Vector2 ( 0.5f, 0 );
            rectTransform.anchoredPosition = new Vector3 ( 0.5f, 100f, 0 );
        }
    }



    private static void RemoveClone ( GameObject clone, Time time )
    {
        if (time == Time.oneSecond)
        {
            Destroy ( clone.gameObject, 1f );
        }
        else if (time == Time.twoSecond)
        {
            Destroy ( clone.gameObject, 2f );
        }
        else
        {
            Destroy ( clone.gameObject, 3f );
        }
    }
}
