using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

    public class GameUtils : MonoBehaviour
    {
        public static string GetPlatformAssetBundle()
        {
            string platform = RuntimePlatform.Android.ToString();
#if UNITY_STANDALONE
            platform = RuntimePlatform.WindowsPlayer.ToString();
#endif
#if UNITY_EDITOR
            //Debug.Log("Unity Editor");
#endif

#if UNITY_IPHONE

#endif
#if UNITY_IOS
        platform = "IOS";
#endif

#if UNITY_WEBGL
        platform = "WebGL";
#endif
            return platform;
        }

        /*    public static string GetURLAssetBundle()
            {
                string result = null;
        #if UNITY_EDITOR
                string urlLocal = UrlConstant.URL_ASSET_LOCAL;
                if (SystemInfo.deviceName == "KUN")
                {
                    urlLocal = UrlConstant.URL_ASSET_LOCAL_KUN;
                }
                result = urlLocal + GetPlatformAssetBundle() + "/";

        #else
                result = UrlStatic.URL_ASSET + GetPlatformAssetBundle() + "/";
        #endif

        #if UNITY_EDITOR_OSX
                result = UrlStatic.URL_ASSET + GetPlatformAssetBundle() + "/";
        #endif
                return result;
            }*/

        /*    public static string GetURLAsset()
            {
                string result = null;
        #if UNITY_EDITOR
                result = UrlConstant.URL_ASSET_LOCAL;
                if (SystemInfo.deviceName == "KUN")
                {
                    result = UrlConstant.URL_ASSET_LOCAL_KUN;
                }
        #else
                result = UrlStatic.URL_ASSET;
        #endif
                return result;
            }*/

        private static string[] statusReceived = new string[3] {
        "none","claim","done"
    };

        /*    public static Enums.StatusReceive GetStatusReceived(string status)
            {
                for (int i = 0; i < statusReceived.Length; i++)
                {
                    if (status == statusReceived[i])
                    {
                        return (Enums.StatusReceive)i;
                    }
                }
                return Enums.StatusReceive.None;
            }*/

        public static string AppendColor(string msg, string hexColor)
        {
            return "<color=" + hexColor + ">" + msg + "</color>";
        }
        public static string UppercaseWord(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            string[] tmp = s.Split(' ');
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < tmp.Length; i++)
            {
                sb.Append(UppercaseFirst(tmp[i]));
                sb.Append(' ');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static string ConvertSecondToMMSS(int second)
        {
            int minute = second / 60;
            int s = second % 60;
            return minute.ToString("00") + ":" + s.ToString("00");
        }

        public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
        {
            if ((Object)cam == (Object)null)
                return new Vector2(worldPoint.x, worldPoint.y);
            return (Vector2)cam.WorldToScreenPoint(worldPoint);
        }

        public static void SetLayerRecursively(GameObject go, int layerNumber)
        {
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }
        public static T[] AddToArray<T>(T[] Source, T Value)
        {
            if (Value == null) return Source;
            T[] New = new T[Source.Length + 1];
            Source.CopyTo(New, 0);
            New[Source.Length] = Value;
            return New;
        }
        public static T[] AddToArray<T>(T[] Source, T[] Values)
        {
            if (Source == null) Source = new T[0];
            if (Values == null) return Source;
            T[] New = new T[Source.Length + Values.Length];
            Source.CopyTo(New, 0);
            for (int i = 0; i < Values.Length; i++)
            {
                int index = i + Source.Length;
                New[index] = Values[i];
            }
            return New;
        }
        public static IEnumerator DelayFunction(System.Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
        public static IEnumerator DelayFunction(IEnumerator callback)
        {
            yield return null;
            yield return callback;
        }

        public static IEnumerator DelayFunction(float delay, System.Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
        public static IEnumerator DelayFunction(float delay, System.Action<bool> callback, bool param)
        {
            yield return new WaitForSeconds(delay);
            callback(param);
        }

        public static long GenerateGameId()
        {
            return RandomRange(1, 1000000);
        }
        public static int RandomRange(int min, int max)
        {
            return Random.Range(min, max + 1);
        }
        public static float RandomRange(float min, float max)
        {
            return Random.Range(min, max + 1f);
        }


        public static void ClearTransform(Transform transform)
        {
            if (transform == null) return;
            int count = transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                GameObject go = transform.GetChild(i).gameObject;
                if (go.name == "GerenalItem")
                {
                    //  GiftManager.Instance.AddPool(go);
                }
                else
                {
                    Destroy(go);
                }

            }
        }


        public static int ParseStringToInt(string str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }

        public static bool ParseStringToBool(string str)
        {
            bool result = false;
            bool.TryParse(str, out result);
            return result;
        }
        public static bool ParseIntToBool(int str)
        {
            bool result = str != 0;
            return result;
        }
        public static float ParseStringToFloat(string str)
        {
            float result = 0;
            float.TryParse(str, out result);
            return result;
        }
        public static double ParseStringToDouble(string str)
        {
            double result = 0;
            double.TryParse(str, out result);
            return result;
        }
        public static List<object> ParseStringToListObject(string source)
        {
            List<object> result = MiniJSON.Json.Deserialize(source) as List<object>;
            return result as List<object>;
        }

        public static long ParseStringToLong(string str)
        {
            long result = 0;
            long.TryParse(str, out result);
            return result;
        }

        private static string GetPlayerPref(string strPref, bool checkUser = true)
        {
            //  if (checkUser)
            //return "MegaFun_" + UserInfo.SmartFoxName + "_" + strPref;
            return "MegaFun_" + strPref;
        }

        public static void SaveDataPref(string strPref, string value, bool checkUser = true)
        {
            string keyPref = GetPlayerPref(strPref, checkUser);
            PlayerPrefs.SetString(keyPref, value);
        }
        public static void SaveDataPref(string strPref, int value, bool checkUser = true)
        {
            PlayerPrefs.SetInt(GetPlayerPref(strPref, checkUser), value);
        }
        public static string GetStringPref(string strPref, string _default = "", bool checkUser = true)
        {
            string keyPref = GetPlayerPref(strPref, checkUser);
            string result = PlayerPrefs.GetString(keyPref, _default);
            return result;
        }
        public static int GetIntPref(string strPref, bool checkUser = true)
        {
            return PlayerPrefs.GetInt(GetPlayerPref(strPref, checkUser));
        }

        public static int GetIntPref(string strPref, int _default, bool checkUser = true)
        {
            return PlayerPrefs.GetInt(GetPlayerPref(strPref, checkUser), _default);
        }

        public static void RemovePref(string strPref, bool checkUser = true)
        {
            if (PlayerPrefs.HasKey(GetPlayerPref(strPref, checkUser)))
                PlayerPrefs.DeleteKey(GetPlayerPref(strPref, checkUser));
        }


        public static string GetDeviceId()
        {
            return string.Empty;
            //#if UNITY_IOS
            //        return UnityEngine.iOS.Device.advertisingIdentifier;
            //#else
            //        return SystemInfo.deviceUniqueIdentifier;
            //#endif
        }




        public static int GetLayerIndexByString(string str)
        {
            return LayerMask.NameToLayer(str);
        }


        public static string GetHashMD5(string usedString)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(usedString);
            byte[] hash = md5.ComputeHash(bytes);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static string ShortCutNumber(long value)
        {
            string result = value.ToString();

            int leng = result.Length;
            if (leng > 10) result = result.Substring(0, result.Length - 9) + "B";
            else if (leng > 7) result = result.Substring(0, result.Length - 6) + "M";
            else if (leng > 5) result = result.Substring(0, result.Length - 3) + "k";
            return result;
        }


        static public object GetValProObject(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public static System.DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            System.TimeZoneInfo zoneInfo = System.TimeZoneInfo.Local;
            int deltaHour = zoneInfo.BaseUtcOffset.Hours;
            int offSet = 0;
            if (deltaHour <= 0) offSet = 1;
            dtDateTime = dtDateTime.AddHours(7 - deltaHour - offSet);
            return dtDateTime;
        }

        public static string ConvertSecondToHHMMSS(int seconds)
        {
            int hour = Mathf.FloorToInt(seconds / (60 * 60));
            int minute = Mathf.FloorToInt((seconds % 3600) / 60);
            int s = seconds % 60;
            return hour.ToString("00") + ":" + minute.ToString("00") + ":" + s.ToString("00");

        }
        /*    public static string ConverSecondToDays(int seconds)
            {

                int day = Mathf.FloorToInt(seconds / (60 * 60 * 24));
                if (day > 0)
                {
                    return day.ToString() + " " + LanguageHelper.GetTextByKey("day");
                }
                return ConvertSecondToHHMMSS(seconds);
            }

            public static string StrikeThrough(string s) // chữ gạch ngang
            {
                string strikethrough = "";
                foreach (char c in s)
                {
                    strikethrough = strikethrough + c + '\u0336';
                }
                return strikethrough;
            }

            public static float TrimNumber(float theNumber, float decPlaces)
            {
                if (decPlaces >= 0)
                {
                    float temp = Mathf.Pow(10, decPlaces);
                    return Mathf.Round(theNumber * temp) / temp;
                }
                return 0;
            }
            public static string AddDotNumber(float value)
            {
                System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                nfi.CurrencyDecimalSeparator = ",";
                nfi.CurrencyGroupSeparator = ".";
                nfi.CurrencySymbol = "";
                return System.Convert.ToDecimal(value).ToString("C0", nfi);
                // return System.Convert.ToDecimal(value).ToString("#.##0,###");
            }
            public static TimerController CreateTimer(Transform parent)
            {
                GameObject go = new GameObject();
                go.name = "Timer";
                go.transform.SetParent(parent);
                go.transform.localScale = Vector3.one;
                return go.AddComponent<TimerController>();
            }
            public static bool LoadingHaveProgress(Enums.TypeLoading typeLoading)
            {
                switch (typeLoading)
                {
                    case Enums.TypeLoading.CheckingResource:
                        return true;
                    default:
                        return false;
                }
            }
            public static List<string> GetListAssetByScene(string sceneName)
            {
                List<string> result = null;
                switch (sceneName)
                {
                    case SceneConstant.HOME_SCENE:
                        result = new List<string>() { NameAssetBundle.HOME };
                        break;

                    default:
                        result = new List<string>();
                        break;
                }
                return result;
            }
            public static void GameStop()
            {
                Time.timeScale = 0;
                CalaLogger.LogError("stop hack");
                //PopupController.Instance.ShowPopUpHack(false, false);
            }
            public static void SetURLByRegion(Dictionary<string, object> bestRegion)
            {
                UrlStatic.URL_ASSET = bestRegion.GetString("asset");
                UrlStatic.URL_HOST_HTTP = bestRegion.GetString("host_http");
                UrlStatic.URL_API = bestRegion.GetString("api");
                UrlStatic.URL_HOST = bestRegion.GetString("host");
                UrlStatic.URL_API_LOGIN = bestRegion.GetString("api_login");
                UrlStatic.URL_API_PAYMENT = bestRegion.GetString("api_payment");
                //if (GameStatic.USE_URL_BK)
                //{
                //    UrlStatic.URL_ASSET = bestRegion.GetString("asset_bk");
                //    UrlStatic.URL_API = bestRegion.GetString("api_bk");
                //}
            }

            public static void InitItemsIcon()
            {
                if (GameStatic.ItemIcons == null)
                {
                    AssetBundle assetBundle = AssetBundleHelper.GetAssetBundleByName(NameAssetBundle.ITEMS_ICON);
                    GameStatic.ItemIcons = assetBundle.LoadAllAssets<Sprite>();
                }
            }
            public static void InitCards()
            {
                if (GameStatic.CardSprites == null)
                {
                    AssetBundle assetBundle = AssetBundleHelper.GetAssetBundleByName(NameAssetBundle.CARDS);
                    GameStatic.CardSprites = assetBundle.LoadAllAssets<Sprite>();
                }
            }
            public static Sprite GetSpriteItem(int id)
            {
                return SpriteAtlasHelper.GetSpriteByName(GameStatic.ItemIcons, id + Constant.GACH_DUOI + "i");
            }
            public static Sprite GetSpriteCard(int id)
            {
                int idImg = 0;
                if (id > 1000) idImg = id;
                else if (id > 200) idImg = 201;
                else if (id > 100) idImg = 101;
                else if (id > 0) idImg = 1;
                return SpriteAtlasHelper.GetSpriteByName(GameStatic.CardSprites, idImg + Constant.GACH_DUOI + "c");
            }

            public static UserItem GetUserItemById(int id)
            {
                UserItem userItem = null;
                if (UserInfo.UserItems.TryGetValue(id, out userItem)) return userItem;
                else return null;
            }
            public static UserCard GetUserCardById(int id)
            {
                UserCard userCard = null;
                if (UserInfo.UserCards.TryGetValue(id, out userCard)) return userCard;
                return userCard;
            }
            public static ItemModel GetItemModelById(int id)
            {
                foreach (ItemModel itemMod in GameStatic.ConfigItemModel)
                {
                    if (itemMod.ID == id) return itemMod;
                }
                return null;
            }

            public static MapModel GetMapModelCM3(int level, int node)
            {
                foreach (var map in GameStatic.ConfigMapCandyMatch3)
                {
                    if (map.Level == level && map.Node == node) return map;
                }
                return null;
            }
        *//*    public static MapModel GetMapModelBS(int level, int node)
            {
                foreach (var map in GameStatic.ConfigMapBubbleShooter)
                {
                    if (map.Level == level && map.Node == node) return map;
                }
                return null;
            }*//*
            public static int ConvertAvatarIdToSpineId(int id, int type)
            {
                int resourceId = id - ((type - 1) * 1000);
                return resourceId;
            }
            private static GameObject prefabsCardItem;
            public static CardItem CreateCardItem(Transform transParent)
            {
                if (prefabsCardItem == null)
                {
                    AssetBundle assetBundle = AssetBundleHelper.GetAssetBundleByName(NameAssetBundle.HOME);
                    prefabsCardItem = assetBundle.LoadAsset<GameObject>("CardItem");
                }

                GameObject go = PrefabsHelper.CloneGameObject(prefabsCardItem, transParent);
                return go.GetComponent<CardItem>();
            }
            public static MapModel GetMapModelMM(int level, int node)
            {
                foreach (var map in GameStatic.ConfigMapMergeMart)
                {
                    if (map.Level == level && map.Node == node) return map;
                }
                return null;
            }
            public static int GetEnergyRequiredMap(string idGame, int level, int node)
            {
                int result = 0;
                switch (idGame)
                {
                    case IdGameConstant.CANDY_MATCH_3:
                        MapModel mapModel = GetMapModelCM3(level, node);
                        if (mapModel != null) result = mapModel.RequireEnergy;
                        break;
                    case IdGameConstant.BUBBLE_SHOOTER:
                        MapModel bSMap = GetMapModelBS(level, node);
                        if (bSMap != null) result = bSMap.RequireEnergy;
                        break;
                    case IdGameConstant.MERGE_MART:
                        MapModel mMMap = GetMapModelMM(level, node);
                        if (mMMap != null) result = mMMap.RequireEnergy;
                        break;
                }
                return result;
            } 

            public static MapModel GetMapModel(string idGame, int level, int node)
            {
                MapModel result = null;
                switch (idGame)
                {
                    case IdGameConstant.CANDY_MATCH_3:
                        MapModel mapModel = GetMapModelCM3(level, node);
                        if (mapModel != null) result = mapModel;
                        break;
                    case IdGameConstant.BUBBLE_SHOOTER:
                        MapModel bSMap = GetMapModelBS(level, node);
                        if (bSMap != null) result = bSMap;
                        break;
                    case IdGameConstant.MERGE_MART:
                        MapModel mMMap = GetMapModelMM(level, node);
                        if (mMMap != null) result = mMMap;
                        break;
                }
                return result;
            }

            public static bool CompareUserEnoughItem(List<object> listRequired)
            {
                if (listRequired.IsNullOrEmpty()) return true;
                foreach (var obj in listRequired)
                {
                    Dictionary<string, object> dataObj = obj as Dictionary<string, object>;
                    int id = dataObj.GetInt(ServerKey.ID);
                    UserItem uItem = GetUserItemById(id);
                    if (uItem == null) return false;
                    if (uItem.Quantity < dataObj.GetInt(ServerKey.QUANTITY)) return false;
                }
                return true;
            }

            public static void DecreaseUserItem(List<object> listItems)
            {
                if (listItems.IsNullOrEmpty()) return;
                foreach (var obj in listItems)
                {
                    Dictionary<string, object> dataCost = obj as Dictionary<string, object>;
                    UserItem userItem = GetUserItemById(dataCost.GetInt(ServerKey.ID));
                    if (userItem != null)
                    {
                        userItem.Quantity -= dataCost.GetInt(ServerKey.QUANTITY);
                        MainGameController.Instance.Dev1Module.InfoUser.ShowValue(userItem.Id, userItem.Quantity);
                    }
                    else Debug.LogError(" User ko có item " + dataCost.GetInt(ServerKey.ID));
                }
            }
            public static string GetDeviceInfo()
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("model", SystemInfo.deviceModel);
                data.Add("device", SystemInfo.deviceType);
                data.Add("sys", SystemInfo.operatingSystem);
                return data.ToJson();
            }
            public static T ParseEnum<T>(string value)
            {
                return (T)System.Enum.Parse(typeof(T), value, true);
            }
            public static CardModel GetCardModelById(int id)
            {
                foreach (CardModel cardMod in GameStatic.ConfigCardModel)
                {
                    if (cardMod.ID == id) return cardMod;
                }
                return null;
            }*/
        public static TimerController CreateTimer(Transform parent)
        {
            GameObject go = new GameObject();
            go.name = "Timer";
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            return go.AddComponent<TimerController>();
        }
    }


