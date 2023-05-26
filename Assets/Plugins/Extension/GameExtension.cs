using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public static class GameExtension
{
    public static V Get<K, V>(this Dictionary<K, V> keyValuePairs, K key)
    {
        if (keyValuePairs.ContainsKey(key))
        {
            return keyValuePairs[key];
        }
        return default(V);
    }
    public static string ToJson(this IDictionary<string, object> dictionary)
    {
        if (dictionary == null)
        {
            dictionary = new Dictionary<string, object>();
        }
        return MiniJSON.Json.Serialize(dictionary);
    }
    public static string ToJson(this IDictionary<int, int> dictionary)
    {
        if (dictionary == null)
        {
            dictionary = new Dictionary<int, int>();
        }
        return MiniJSON.Json.Serialize(dictionary);
    }
    public static string ToJson(this List<object> list)
    {
        if (list == null)
        {
            list = new List<object>();
        }
        return MiniJSON.Json.Serialize(list);
    }
    public static string GetString(this IDictionary<string, object> keyValuePairs, string key)
    {
        if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
        {
            if (keyValuePairs[key] != null)
            {
                return keyValuePairs[key].ToString();
            }
            return null;
        }
        return null;
    }
    public static int GetInt(this IDictionary<string, object> keyValuePairs, string key)
    {
        int result = 0;
        try
        {
            if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
            {
                int.TryParse(keyValuePairs[key].ToString(), out result);
            }
        }
        catch (System.Exception)
        {

        }

        return result;
    }  
    
    public static long GetLong(this IDictionary<string, object> keyValuePairs, string key)
    {
        long result = 0;
        try
        {
            if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
            {
                long.TryParse(keyValuePairs[key].ToString(), out result);
            }
        }
        catch (System.Exception)
        {
        }
        return result;
    }
    public static void Put<K, V>(this Dictionary<K, V> keyValuePairs, K key, V value)
    {
        if (keyValuePairs.ContainsKey(key))
        {
            keyValuePairs.Remove(key);
        }
        keyValuePairs.Add(key, value);
    }
    public static void Wait(this MonoBehaviour mono, float delay, System.Action action)
    {
        mono.StartCoroutine(excute(delay, action));
    }
    private static IEnumerator excute(float delay, System.Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
    public static void WaitEndFrame(this MonoBehaviour mono, System.Action action, int numFrame = 1)
    {
        mono.StartCoroutine(excuteEndFrame(action, numFrame));
    }
    private static IEnumerator excuteEndFrame(System.Action callback, int numFrame)
    {
        for (int i = 0; i < numFrame; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }
    public static void SetAlpha(this UnityEngine.UI.MaskableGraphic maskableGraphic, float alpha)
    {
        var color = maskableGraphic.color;
        color.a = alpha;
        maskableGraphic.color = color;
    }

    public static Dictionary<string, object> ToDictionary(this string source)
    {
        object result = null;
        try
        {
            result = MiniJSON.Json.Deserialize(source);
        }
        catch (System.Exception e)
        {
            //PopupController.Instance.ShowPopUpAlertConnectionError("parse error");
            //ServiceHelper.Instance.InsertLogError(null, "hack__ " + source + " [error] " + e.StackTrace);
            Debug.LogError(e);
        }
        return result as Dictionary<string, object>;
    }
    public static List<object> ToList(this string source)
    {
        object result = null;
        try
        {
            result = MiniJSON.Json.Deserialize(source);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return result as List<object>;
    }

    public static List<K> ListKey<K, V>(this Dictionary<K, V> dictionary)
    {
        return new List<K>(dictionary.Keys);
    }
    public static List<string> ListKey(this Dictionary<string, object> dictionary)
    {
        return new List<string>(dictionary.Keys);
    }
    public static string FirstKey(this Dictionary<string, object> dictionary)
    {
        if (dictionary.Keys.Count < 1) return "";
        return new List<string>(dictionary.Keys)[0];
    }
    public static Dictionary<string, object> GetDictionary(this Dictionary<string, object> keyValuePairs, string key)
    {
        if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
        {
            return keyValuePairs[key] as Dictionary<string, object>;
        }
        return null;
    }
    public static uint GetUInt(this Dictionary<string, object> keyValuePairs, string key)
    {
        uint result = 0;
        if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
        {
            uint.TryParse(keyValuePairs[key].ToString(), out result);
        }
        return result;
    }
    public static string PrintArray<T>(this T[] array)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append(array[i]);
            if (i < array.Length - 1)
            {
                sb.Append("\n");
            }
        }
        return sb.ToString();
    }
    public static string RemoveCharInvisible(this string result)
    {
        return result.Trim(new char[] { '\uFEFF', '\u200B' });
    }
    public static string ConvertByteToString(this byte[] source)
    {
        return source != null ? System.Text.Encoding.UTF8.GetString(source) : null;
    }
    public static string ExceptionToLog(this System.Exception ex)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(" Message: ");
        sb.Append(ex.Message);
        sb.Append(" ");
        sb.Append("Stacktrace: ");
        sb.Append(ex.StackTrace);
        sb.Append(" ");
        return sb.ToString();
    }

   /* public static int GetCode(this Dictionary<string, object> source)
    {
        if (source == null) return int.MinValue;
        return source.GetInt(ServerKey.CODE);
    }

    public static bool CheckCode(this Dictionary<string, object> source)
    {
        if (source == null) return false;
        return source.GetCode() == CodeConstants.SUCCESS;
    }
    public static int GetCode(this ISFSObject source)
    {
        if (source == null) return int.MinValue;
        return source.GetInt(ServerKey.CODE);
    }

    public static bool CheckCode(this ISFSObject source)
    {
        if (source == null) return false;
        return source.GetCode() == CodeConstants.SUCCESS;
    }*/
    public static bool IsNullOrEmpty(this string _string)
    {
        return _string == null || _string == string.Empty;
    }
    public static bool IsNullOrEmpty<T>(this T[] array)
    {
        return array == null || array.Length == 0;
    }
    public static bool IsNullOrEmpty(this ICollection collection)
    {
        return collection == null || collection.Count == 0;
    }
    public static List<object> GetList(this Dictionary<string, object> keyValuePairs, string key)
    {
        if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
            return keyValuePairs[key] as List<object>;
        return null;
    }
    public static long ToLong(this object obj)
    {
        if (obj == null) return 0;
        return GameUtils.ParseStringToLong(obj.ToString());
    }
  /*  public static string ToStringEncrypt<T>(this T source)
    {
        string result = null;
        if (source == null)
        {
            result = Constant.GACH_DUOI;
            return result;
        }
        char[] chars = source.ToString().ToCharArray();
        if (chars.Length == 1)
        {
            chars = GameUtils.AddToArray(chars, Constant.CHAR_GACH_DUOI);
        }
        result = string.Join(Constant.GACH_DUOI, chars);
        return result;
    }
    public static string ToStringDecrypt<T>(this T source)
    {
        string result = null;
        string[] chars = source.ToString().Split(Constant.CHAR_GACH_DUOI);
        result = string.Join("", chars);
        return result;
    }
    public static int GetInt(this string val)
    {
        if (val == null) val = Constant.GACH_DUOI;
        if (val.ToString().Contains(Constant.GACH_DUOI))
        {
            return val.ToStringDecrypt().ToInt();
        }
        else
        {
            GameUtils.GameStop();
        }
        return 0;
    }*/
    public static int ToInt(this object obj)
    {
        if (obj == null) return 0;
        return GameUtils.ParseStringToInt(obj.ToString());
    }
/*    public static string GetString(this string val)
    {
        if (val == null) val = Constant.GACH_DUOI;
        if (val.ToString().Contains(Constant.GACH_DUOI))
        {
            return val.ToStringDecrypt();
        }
        else
        {
            GameUtils.GameStop();
        }
        return string.Empty;
    }*/
    public static Dictionary<string, object> ToDictionary(this Sfs2X.Entities.Data.ISFSObject obj )
    {
        return obj.ToJson().ToDictionary() ;
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, top);
    }
    public static void SetBot(this RectTransform rt, float bot)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bot);
    }
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }
    public static void SetHeight(this Transform transform, float h)
    {
        Vector2 sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, h);
    }
    public static bool IsNetworkError(this UnityEngine.Networking.UnityWebRequest.Result result)
    {
        if (result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
            result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError ||
            result == UnityEngine.Networking.UnityWebRequest.Result.DataProcessingError) return true;
        return false;
    }

    private static Shader shaderGray;
    private static Material materialGray;
    public static void GrayColor(this MaskableGraphic maskableGraphic)
    {
        if (shaderGray == null)
        {
            shaderGray = Shader.Find("Unlit/Grayscale");
            materialGray = new Material(shaderGray);
        }
        maskableGraphic.material = materialGray;
        Outline outline = maskableGraphic.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
    public static void NormalColor(this MaskableGraphic maskableGraphic, bool autoEnableOutline = true, bool checkOtherMaterial = false)
    {
        if (!checkOtherMaterial)
        {
            maskableGraphic.material = null;
        }
        else
        {
            if (maskableGraphic.material == materialGray) maskableGraphic.material = null;
        }
        if (autoEnableOutline)
        {
            Outline outline = maskableGraphic.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
        }
    }
    public static void ToNormal(this Transform transItem, float alpha = 1, bool checkOtherMaterial = false, bool haveSetAlpha = true)
    {
        Image[] images = transItem.GetComponentsInChildren<Image>();
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].NormalColor(true, checkOtherMaterial);
                if (haveSetAlpha) images[i].SetAlpha(alpha);
            }
        }
        RawImage[] rawImages = transItem.GetComponentsInChildren<RawImage>();
        if (rawImages != null)
        {
            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].NormalColor();
                if (haveSetAlpha) rawImages[i].SetAlpha(alpha);
            }
        }
        Text[] texts = transItem.GetComponentsInChildren<Text>();
        if (texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].NormalColor();
                if (haveSetAlpha) texts[i].SetAlpha(alpha);
            }
        }
        transItem.AllowOutline(true);
    }
    public static void ToGrey(this Transform transItem, float alpha = 1)
    {
        Image[] images = transItem.GetComponentsInChildren<Image>();
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].GrayColor();
                images[i].SetAlpha(alpha);
            }
        }
        RawImage[] rawImages = transItem.GetComponentsInChildren<RawImage>();
        if (rawImages != null)
        {
            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].GrayColor();
                rawImages[i].SetAlpha(alpha);
            }
        }
        Text[] texts = transItem.GetComponentsInChildren<Text>();
        if (texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].GrayColor();
                texts[i].SetAlpha(alpha);
            }
        }
        transItem.AllowOutline(false);
    }
    public static void AllowOutline(this Transform transItem, bool allow)
    {
        Outline[] outlines = transItem.GetComponentsInChildren<Outline>(true);

        if (!outlines.IsNullOrEmpty())
        {
            for (int i = 0; i < outlines.Length; i++)
            {
                outlines[i].enabled = allow;
            }
        }
    }
/*    public static bool HaveSkin(this Spine.AnimationState animationState, string skinToUse)
    {
        if (animationState == null)
        {
            return false;
        }

        if (animationState.Data.SkeletonData.FindSkin(skinToUse) != null)
        {
            return true;
        }
        return false;
    }*/
    public static void SetAlpha(this SpriteRenderer sprite, float alpha)
    {
        var color = sprite.color;
        color.a = alpha;
        sprite.color = color;
    }
    public static bool GetBool(this Dictionary<string, object> keyValuePairs, string key)
    {
        bool result = false;
        if (keyValuePairs.ContainsKey(key))
        {
            bool.TryParse(keyValuePairs[key].ToString(), out result);
        }
        return result;
    }
    public static void SetLayerRecursively(this GameObject go, string layer)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
}
