using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class WordEntry
{
    public string word;
    public string translation;

    public WordEntry(string w, string t)
    {
        word = w;
        translation = t;
    }
}

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager instance;

    public List<WordEntry> dictionary = new List<WordEntry>();
    private HashSet<string> currentLevelWords = new HashSet<string>();
    private string savePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Path.Combine(Application.persistentDataPath, "dictionary.json");
            LoadDictionary();
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void AddWordToMaster(string word, string translation)
    {
        if (!dictionary.Exists(w => w.word == word))
        {
            dictionary.Add(new WordEntry(word, translation));
            SaveDictionary();
        }
    }

    public void AddWordToLevel(string word)
    {
        currentLevelWords.Add(word);
    }

    public void ClearLevelWords()
    {
        currentLevelWords.Clear();
        SaveDictionary();
    }

    public List<WordEntry> GetFilteredDictionary()
    {
        List<WordEntry> filtered = new List<WordEntry>();
        if (dictionary == null) return filtered;

        foreach (WordEntry entry in dictionary)
        {
            if (currentLevelWords.Contains(entry.word))
            {
                filtered.Add(entry);
            }
        }
        return filtered;
    }

    private void SaveDictionary()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(new DictionaryWrapper { words = dictionary });
            File.WriteAllText(savePath, jsonData);
        }
        catch (System.Exception e)
        {
        }
    }

    private void LoadDictionary()
    {
        if (string.IsNullOrEmpty(savePath) || !File.Exists(savePath))
        {
            return;
        }

        try
        {
            string jsonData = File.ReadAllText(savePath);
            DictionaryWrapper wrapper = JsonUtility.FromJson<DictionaryWrapper>(jsonData);
            dictionary = wrapper.words ?? new List<WordEntry>();
        }
        catch (System.Exception e)
        {
            dictionary = new List<WordEntry>();
        }
    }

    public void ClearDictionary()
    {
        dictionary.Clear();
        currentLevelWords.Clear();
        SaveDictionary();
    }

    [System.Serializable]
    private class DictionaryWrapper
    {
        public List<WordEntry> words;
    }

    public static void AddWord(string word, string translation)
    {
        if (instance != null) instance.AddWordToMaster(word, translation);
    }

    public static void AddWordToCurrentLevel(string word)
    {
        if (instance != null) instance.AddWordToLevel(word);
    }

    public static void ClearLevelCollectedWords()
    {
        if (instance != null) instance.ClearLevelWords();
    }

    public static List<WordEntry> GetDictionary()
    {
        return instance?.GetFilteredDictionary() ?? new List<WordEntry>();
    }
}
