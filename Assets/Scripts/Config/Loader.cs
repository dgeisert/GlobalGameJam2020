using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Loader
{
    public static string GetAssetPath()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return "jar:file://" + Application.dataPath + "!/assets";
#else
        return "File://" + Application.streamingAssetsPath;
#endif
    }

    public static string GetGameFileName()
    {
        return "game.json";
    }

    public static IEnumerator LoadConfigFromFile<T>(string path, Action<T> callback)
    {
        using (var request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.LogError("Error loading config path: " + path);
            }
            else
            {
                var data = JsonUtility.FromJson<T>(request.downloadHandler.text);
                callback(data);
            }
        }
    }

    public static T LoadConfigFromLocalFile<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    public static IEnumerator LoadAndSetImage(Image target, string path, bool etc2=false)
    {
        using (var request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.LogError("Error loading config path: " + path);
            }
            else
            {
                if (etc2)
                {
                    var texture = new Texture2D((int)target.sprite.rect.width, (int)target.sprite.rect.height, TextureFormat.ETC2_RGB, false);
                    texture.LoadRawTextureData(request.downloadHandler.data);
                    texture.Apply();
                    target.sprite = Sprite.Create(texture, target.sprite.rect, target.sprite.pivot);
                    
                }
                else
                {
                    var texture = new Texture2D((int)target.sprite.rect.width, (int)target.sprite.rect.height);
                    texture.LoadImage(request.downloadHandler.data);
                    target.sprite = Sprite.Create(texture, target.sprite.rect, target.sprite.pivot);
                }
            }
        }
    }
}