using CardGame;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameLoader
{
    static string saveDir { get => Application.persistentDataPath + "/saves/"; }

    static readonly Regex invalidChars = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);


    public static void SaveGame(GameState state, string saveName)
    {
        string path = GetSavePath(saveName);
        string data = StateToJSON(state);

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.Write(data);
        }
    }

    public static void LoadGame(string saveName)
    {
        string path = GetSavePath(saveName);
        string data;


        using (StreamReader reader = new StreamReader(path))
        {
            data = reader.ReadToEnd();
        }

        if(string.IsNullOrEmpty(data))
        {
            throw new System.Exception("No data found in " + path);
        }

        GameState.Instance = StateFromJSON(data);
    }

    public static string[] GetSaveFiles()
    {
        if (Directory.Exists(saveDir))
        {
            return Directory.GetFiles(saveDir, "*.json");
        }
        else
        {
            return null;
        }
    }

    public static bool SaveFileExist(string saveName)
    {
        string path = GetSavePath(saveName);
        return File.Exists(path);
    }

    // See: https://gist.github.com/sergiorykov/219605a220edf80d4b55fe87a9f92b38
    static string SanatizeName(string fileName, string replacement = "_")
    {
        return invalidChars.Replace(fileName, replacement);
    }

    static string GetSavePath(string saveName)
    {
        return saveDir + SanatizeName(saveName) + ".json";
    }

    static GameState StateFromJSON(string json)
    {
        return JsonUtility.FromJson<GameState>(json);
    }

    static string StateToJSON(GameState state)
    {
        return JsonUtility.ToJson(state);
    }
}
