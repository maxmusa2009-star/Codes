using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager instance;
    private List<WordEntry> masterDictionary = new List<WordEntry>();
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
            Destroy(gameObject);
        }
    }

    private void AddWordToMaster(string word, string translation)
    {
        if (!masterDictionary.Exists(w => w.word == word))
        {
            masterDictionary.Add(new WordEntry(word, translation));
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
    }

    private List<WordEntry> GetFilteredDictionary()
    {
        List<WordEntry> filtered = new List<WordEntry>();
        foreach (WordEntry entry in masterDictionary)
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
        string jsonData = JsonUtility.ToJson(new DictionaryWrapper { words = masterDictionary });
        File.WriteAllText(savePath, jsonData);
    }

    private void LoadDictionary()
    {
        if (string.IsNullOrEmpty(savePath) || !File.Exists(savePath))
        {
           
            masterDictionary = new List<WordEntry>();
            return;
        }

        try
        {
            string jsonData = File.ReadAllText(savePath);
            DictionaryWrapper wrapper = JsonUtility.FromJson<DictionaryWrapper>(jsonData);
            masterDictionary = wrapper.words ?? new List<WordEntry>();
          
        }
        catch (System.Exception)
        {
           
            masterDictionary = new List<WordEntry>();
        }
    }

    public void ClearDictionary()
    {
        masterDictionary.Clear();
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
        instance?.AddWordToMaster(word, translation);
    }

    public static void AddWordToCurrentLevel(string word)
    {
        instance?.AddWordToLevel(word);
    }

    public static void ClearLevelCollectedWords()
    {
        instance?.ClearLevelWords();
    }

    public static List<WordEntry> GetDictionary()
    {
        return instance?.GetFilteredDictionary() ?? new List<WordEntry>();
    }
}

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
