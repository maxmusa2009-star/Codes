using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryUI : MonoBehaviour
{
    [SerializeField] public GameObject dictionaryPanel;
    [SerializeField] private Transform wordsContainer;
    [SerializeField] private GameObject wordItemPrefab;

    [SerializeField] private QuizDoor quizDoor;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleDictionary();
        }
    }

    public void ToggleDictionary()
    {
        if (quizDoor != null && quizDoor.isQuizActive)
        {
            Debug.Log("[Dictionary] ќткрытие заблокировано: идЄт викторина.");
            return;
        }

        isOpen = !isOpen;
        dictionaryPanel.SetActive(isOpen);

        if (isOpen)
        {
            UpdateDictionaryDisplay();
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
    }

    public void UpdateDictionaryDisplay()
    {
        foreach (Transform child in wordsContainer)
        {
            Destroy(child.gameObject);
        }

        List<WordEntry> dictionary = DictionaryManager.GetDictionary();

        foreach (WordEntry entry in dictionary)
        {
            GameObject item = Instantiate(wordItemPrefab, wordsContainer);
            TextMeshProUGUI textComponent = item.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = $"{entry.word} Ч {entry.translation}";
        }
    }

    public void ClearLevelWords()
    {
        DictionaryManager.ClearLevelCollectedWords();
        if (isOpen)
        {
            UpdateDictionaryDisplay();
        }
    }

    public void CloseDictionary()
    {
        isOpen = false;
        if (dictionaryPanel != null)
        {
            dictionaryPanel.SetActive(false);
        }
    }
}
