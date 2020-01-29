using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Util
{
    public static bool DebugMode = false;
    static Util()
    {
        Debug.Log("DATA: " + Application.persistentDataPath);
        DebugMode = Debug.isDebugBuild;

        //var debugPath = Path.Combine(Application.persistentDataPath, "debug.txt");
        var fileFound = false; //File.Exists(debugPath);

        DebugMode = fileFound || DebugMode;

#if UNITY_EDITOR || CREATOR
        DebugMode = true;
#endif

        if (DebugMode)
        {
            Debug.Log("Loading Game with DEBUG MODE ON");
        }
    }

    public static void LoadFromDebugFile()
    {
        var debugPath = Path.Combine(Application.persistentDataPath, "debug.txt");
        var fileFound = File.Exists(debugPath);

        if (fileFound)
        {
            var lines = File.ReadAllLines(debugPath);
            ApplyMods(GetModDictionary(lines));
        }
    }

    public static bool ApplyRemoteUserMod(string data)
    {
        var lines = data.Split('\n');
        var mods = GetModDictionary(lines);
        Stats.LogEvent("UserMod", new Dictionary<string, string> { { "data", data } });
        if (mods.ContainsKey("usermod"))
        {
            Debug.Log("CHECKING " + Storage.GetInt("usermod") + " " + mods["usermod"]);
            if (mods["usermod"] > Storage.GetInt("usermod"))
            {
                Debug.Log("Applying Remote User Mod");
                ApplyMods(mods);
                return true;
            }
        }
        return false;
    }

    public static void ApplyMods(Dictionary<string, int> mods)
    {
        foreach (var mod in mods)
        {
            Storage.SetInt(mod.Key, mod.Value);
            Debug.Log("User Mod Setting " + mod.Key + " to " + mod.Value);
        }
    }

    public static Dictionary<string, int> GetModDictionary(string[] lines)
    {
        var result = new Dictionary<string, int>();
        foreach (var line in lines)
        {
            var split = line.Trim().Split('=');
            if (split.Length == 2)
            {
                int val;
                if (int.TryParse(split[1], out val))
                {
                    result.Add(split[0], val);
                }
                else if (split[0] == "UserModMessage")
                {
                    Storage.SetString("UserModMessage", split[1]);
                }
            }
        }
        return result;
    }

    public static Vector3 CubeBezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (((-p0 + 3 * (p1 - p2) + p3) * t + (3 * (p0 + p2) - 6 * p1)) * t + 3 * (p1 - p0)) * t + p0;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

    public static string FormatTime(float time)
    {
        string str = "";
        int s = (Mathf.FloorToInt(time % 60));
        int m = Mathf.FloorToInt(time / 60);
        if (m > 59)
        {
            int h = Mathf.FloorToInt(m / 60);
            m = (Mathf.FloorToInt(m % 60));
            str += h.ToString() + ":" + (m < 10 ? "0" : "");
        }
        str += m.ToString() + ":" + (s < 10 ? "0" : "") + s.ToString();
        return str;
    }

    public static string GetUserModURL()
    {
        var userID = Stats.GetUserID();
        var bytes = System.Text.Encoding.UTF8.GetBytes(userID + "uXFCr971cP");
        using(var hash = System.Security.Cryptography.MD5.Create())
        {
            var result = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in result)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            var stringResult = hashedInputStringBuilder.ToString().ToLower();
            return String.Format("https://ninjalegends.s3-us-west-1.amazonaws.com/Users/{0}/usermod.dat", stringResult);
        }
    }

    public static void SetCurrentFOVLevel(int requestedLevel = 0)
    {
        OVRManager.TiledMultiResLevel fovLevel;
        fovLevel = OVRManager.TiledMultiResLevel.LMSHigh;
        /*
        if (Game.Level)
        {
            var minLevel = Math.Max(requestedLevel, Game.Level.FOVLevel);
            switch (minLevel)
            {
                case 0:
                    fovLevel = OVRManager.TiledMultiResLevel.Off;
                    break;
                case 1:
                    fovLevel = OVRManager.TiledMultiResLevel.LMSLow;
                    break;
                case 2:
                    fovLevel = OVRManager.TiledMultiResLevel.LMSMedium;
                    break;
                default:
                    fovLevel = OVRManager.TiledMultiResLevel.LMSHigh;
                    break;
            }
        }
        else
        {
            fovLevel = OVRManager.TiledMultiResLevel.LMSLow;
        }
        */

        OVRManager.tiledMultiResLevel = fovLevel;
    }
}