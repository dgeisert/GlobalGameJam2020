using System;
using System.Collections.Generic;
using UnityEngine;

public class Cache<T>
{
    private Func<string, T> get;
    private Action<string, T> set;
    private Action<string> remove;
    private Dictionary<string, T> cache;

    public Cache(Func<string, T> get, Action<string, T> set, Action<string> remove)
    {
        this.get = get;
        this.set = set;
        this.remove = remove;
        this.cache = new Dictionary<string, T>();
    }

    public T Get(string key)
    {
        if (!cache.ContainsKey(key))
        {
            cache[key] = get(key);
        }

        return cache[key];
    }

    public void Set(string key, T value)
    {
        cache[key] = value;
        set(key, value);
    }

    public void Remove(string key)
    {
        cache.Remove(key);
        remove(key);
    }

    public void RemoveAll()
    {
        cache = new Dictionary<string, T>();
    }

    public string ExportData()
    {
        string result = "";
        foreach (var item in cache)
        {
            result += item.Key + "=" + item.Value.ToString() + "\n";
        }
        return result;
    }
}

public class StorageKeys
{
    public static string HasSeenFTUE = "HasSeenFTUE";
    public static string SelectedLevel = "SelectedLevel";
    public static string SelectedWorld = "SelectedWorld";
    public static string UnlockedLevel = "UnlockedLevel";
    public static string Difficulty = "Difficulty";
    public static string Mode = "Mode";
    public static string Special = "Special";
    public static string Weapon = "Weapon";
    public static string LastSpecial = "LastSpecial";
    public static string LastWeapon = "LastWeapon";
    public static string SpecialUnlocks = "SpecialUnlocks";
    public static string WeaponUnlocks = "WeaponUnlocks";
    public static string SetWeapon = "SetWeapon";
    public static string SettingsActive = "SettingsActive";
    public static string SecretsActive = "SecretsActive";
    public static string SecretsUnlocked = "SecretsUnlocked";
    public static string Install = "Install";
    public static string Wins = "Wins";
    public static string Deaths = "Deaths";
    public static string Starts = "Starts";
    public static string StartsSpecial = "StartsSpecial";
    public static string StartsWeapon = "StartsWeapon";
    public static string StartsEasy = "StartsEasy";
    public static string StartsMedium = "StartsMedium";
    public static string StartsHard = "StartsHard";
    public static string StartsExpert = "StartsExpert";
    public static string StartsMaster = "StartsMaster";
    public static string Leaderboard = "Leaderboard";
    public static string LevelsCompleted = "LevelsCompleted";
    public static string TotalSwings = "TotalSwings";
    public static string TotalKills = "TotalKills";
    public static string ExpertCompleted = "ExpertCompleted";
}

public class Storage : MonoBehaviour
{
    public static Storage Instance
    {
        get
        {
            if (!store)
            {
                store = FindObjectOfType<Storage>();
                store.Init();
            }

            return store;
        }
    }

    public static void Reset()
    {
        Instance.bools.RemoveAll();
        Instance.ints.RemoveAll();
        Instance.strings.RemoveAll();
        PlayerPrefs.DeleteAll();
    }

    public static bool GetBool(string key)
    {
        return Instance.bools.Get(key);
    }

    public static void SetBool(string key, bool value)
    {
        Instance.bools.Set(key, value);
    }

    public static string GetString(string key)
    {
        return Instance.strings.Get(key);
    }

    public static void SetString(string key, string value)
    {
        Instance.strings.Set(key, value);
    }

    public static int GetInt(string key)
    {
        return Instance.ints.Get(key);
    }

    public static void SetInt(string key, int value)
    {
        Instance.ints.Set(key, value);
    }

    public static string ExportIntsData()
    {
        return Instance.ints.ExportData();
    }

    public static bool CheckFileExists(string path)
    {
        return System.IO.File.Exists(Application.persistentDataPath + "/" + path);
    }

    public static byte[] LoadFile(string path)
    {
        return System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + path);
    }

    public static byte[] LoadResource(string path)
    {
        try
        {
            return Resources.Load<TextAsset>(path).bytes;
        }
        catch
        {
            Debug.Log(path);
            return new byte[0];
        }
    }

    public static void SaveFile(string path, byte[] data)
    {
        string[] dirs = path.Split('/');
        if (dirs.Length > 1)
        {
            string createDir = "";
            for (int i = 0; i < dirs.Length - 1; i++)
            {
                createDir += "/" + dirs[i];
                if (!System.IO.Directory.Exists(Application.persistentDataPath + createDir))
                {
                    System.IO.Directory.CreateDirectory(Application.persistentDataPath + createDir);
                }
            }
        }
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + path, data);
    }

    private static Storage store;

    public float SaveEveryNSeconds = 5F;

    private Cache<bool> bools;
    private Cache<int> ints;
    private Cache<string> strings;
    private float lastSaveTime;

    public void Init()
    {
        bools = new Cache<bool>(
            (string key) =>
            {
                return PlayerPrefs.GetInt(key) == 1;
            },
            (string key, bool value) =>
            {
                PlayerPrefs.SetInt(key, value ? 1 : 0);
            },
            (string key) =>
            {
                PlayerPrefs.DeleteKey(key);
            }
        );

        ints = new Cache<int>(
            (string key) =>
            {
                return PlayerPrefs.GetInt(key);
            },
            (string key, int value) =>
            {
                PlayerPrefs.SetInt(key, value);
            },
            (string key) =>
            {
                PlayerPrefs.DeleteKey(key);
            }
        );

        strings = new Cache<string>(
            (string key) =>
            {
                return PlayerPrefs.GetString(key);
            },
            (string key, string value) =>
            {
                PlayerPrefs.SetString(key, value);
            },
            (string key) =>
            {
                PlayerPrefs.DeleteKey(key);
            }
        );
    }

    public void Start()
    {
        lastSaveTime = Time.unscaledTime;
    }

    public void Update()
    {
        if (Time.unscaledTime >= lastSaveTime + SaveEveryNSeconds)
        {
            save();
        }
    }

    private void save()
    {
        PlayerPrefs.Save();
        lastSaveTime = Time.unscaledTime;
    }
}