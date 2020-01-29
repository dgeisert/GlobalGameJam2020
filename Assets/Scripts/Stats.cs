using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class EventProperties
{
    public static string SCENE = "Scene";
    public static string WEAPON = "Weapon";
    public static string SPECIAL = "Special";
    public static string DIFFICULTY = "Difficulty";
    public static string MODE = "Mode";
    public static string SECRETS = "Secrets";
    public static string SETTINGS = "Settings";

    public static Dictionary<string, string> Get()
    {
        return new Dictionary<string, string>()
        {
            //put all the event properties here
            { EventProperties.SCENE, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name },
        };
    }
}

public class UserProperties
{
    public static string VERSION = "Version";
    public static string PLATFORM = "Platform";
    public static string USER_NAME = "UserName";
    public static string COHORT = "Cohort";
    public static string TENURE = "Tenure";
    public static string STARTS = "Starts";
    public static string WINS = "Wins";
    public static string DEATHS = "Deaths";
    public static string UNLOCKED_LEVEL = "UnlockedLevel";
    public static string UNLOCKED_SPECIALS = "UnlockedSpecials";
    public static string UNLOCKED_WEAPONS = "UnlockedWeapons";
    public static string UNLOCKED_SECRETS = "UnlockedSecrets";
    public static string STARTS_EASY = "StartsEasy";
    public static string STARTS_MEDIUM = "StartsMedium";
    public static string STARTS_HARD = "StartsHard";
    public static string STARTS_EXPERT = "StartsExpert";
    public static string STARTS_MASTER = "StartsMaster";
    public static string FAVORITE_WEAPON = "FavoriteWeapon";
    public static string FAVORITE_SPECIAL = "FavoriteSpecial";
    public static string GRAPHICS_CARD = "GraphicsCard";
    public static string HMD = "HMD_Headset";
    public static string OS = "OS";
    public static string PROCESSOR = "Processor";
    public static string MEMORY_CAPACITY = "MemoryCapacity";
}

public class Stats : MonoBehaviour
{
    public static Stats instance;

    public static string Version = "1.3";
    private static string NULL_USER_ID = "null";
    private static string API = "https://api.amplitude.com/httpapi";
    private static string KEY = "TODO";

    public float ConnectionRetryPeriodInS = 5F;

    private Dictionary<string, string> userProperties = new Dictionary<string, string>();
    private bool connected = true;
    private bool disabled = false;
    private string userId = Stats.NULL_USER_ID;
    private List<string> traces = new List<string>();

    public static void AddUserProperty(string prop, string val)
    {
        if (instance == null)
        {
            Debug.LogError("No Stats component found.");
        }

        instance.addUserPropertyInternal(prop, val);
    }

    public static void SetUserID(string newUserId)
    {
        if (instance == null)
        {
            Debug.LogError("No Stats component found.");
        }
        Debug.Log("User id: " + newUserId);
        instance.userId = newUserId;
    }

    public static string GetUserID()
    {
        if (instance != null && instance.userId != NULL_USER_ID)
        {
            return instance.userId;
        }
        else
        {
            return null;
        }
    }
    public static void LogEvent(string eventType, Dictionary<string, string> vals)
    {
        if (instance == null)
        {
            Debug.LogError("No Stats component found.");
            return;
        }

        instance.LogEventInternal(eventType, vals);
    }

    public void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }
    void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            if (!traces.Contains(stackTrace))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("Type", type.ToString());
                dic.Add("Condition", condition);
                dic.Add("StackTrace", stackTrace);
                LogEventInternal("Error", dic);
                traces.Add(stackTrace);
            }
        }
    }
    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }

    private void LogEventInternal(string eventType, Dictionary<string, string> vals)
    {
        if (!disabled)
        {
            StartCoroutine(SetEventPropertiesAndLogEvent(eventType, vals));
        }
    }

    private IEnumerator SetEventPropertiesAndLogEvent(string eventType, Dictionary<string, string> vals)
    {
        if (disabled)
        {
            yield break;
        }
        yield return new WaitUntil(PerfManager.Instance.AvailableForLowPri);
        PerfManager.Instance.LockLowPriActions(2);

        float start = Time.unscaledTime;
        while (userId == NULL_USER_ID && connected)
        {
            yield return new WaitForSeconds(1);

            if (start + ConnectionRetryPeriodInS < Time.unscaledTime)
            {
                connected = false;
                Debug.LogWarning("No Network connection, not logging stats.");
                break;
            }
        }

        if (connected)
        {
            EventProperties.Get().ToList().ForEach(e => vals.Add(e.Key, e.Value));
            send(eventType, vals);
        }
    }

    private void send(string eventType, Dictionary<string, string> vals)
    {
        string myIP = "0.0.0.0";
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                myIP = ip.ToString();
            }
        }
        var curTime = (int) (System.DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        var data =
            API +
            "?api_key=" + KEY +
            "&event=%5B%7B%22user_id%22%3A%22" + userId +
            "%22%2C%20%22event_type%22%3A%22" + eventType +
            "%22%2C%20%22event_properties%22%3A" + dictionaryToString(vals) +
            "%2C%20%22user_properties%22%3A" + dictionaryToString(userProperties) +
            "%2C%22ip%22%3A%22" + myIP +
            "%22%2C%20%22time%22%3A" + curTime + "%7D%5D";

        StartCoroutine(post(data));
    }

    private IEnumerator post(string postData)
    {
        UnityWebRequest www = UnityWebRequest.Get(postData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
    }

    private void addUserPropertyInternal(string prop, string val)
    {
        if (userProperties.ContainsKey(prop))
        {
            userProperties[prop] = val;
        }
        else
        {
            userProperties.Add(prop, val);
        }
    }

    private static string dictionaryToString(Dictionary<string, string> dic)
    {
        string ret = "{";
        foreach (KeyValuePair<string, string> kvp in dic)
        {
            ret += "\"" + kvp.Key + "\":\"" + kvp.Value + "\",";
        }

        ret = ret.Substring(0, ret.Length - 1);
        ret += "}";

        return UnityWebRequest.EscapeURL(ret);
    }
}