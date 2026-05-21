using UnityEngine;
using UnityEngine.UI;

public class CleanDictionary : MonoBehaviour
{
    public Button clearButton;
    public GameObject confirmationPanel;
    public Button yesButton;
    public Button noButton;

    private DictionaryManager dictionaryManager;

    private void Start()
    {
        dictionaryManager = DictionaryManager.instance;
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        if (clearButton != null)
            clearButton.onClick.AddListener(ShowConfirmationPanel);

        if (yesButton != null)
            yesButton.onClick.AddListener(() =>
            {
                ClearDictionaryData();
                HideConfirmationPanel();
            });

        if (noButton != null)
            noButton.onClick.AddListener(() =>
            {
                HideConfirmationPanel();
            });
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
    }

    private void HideConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    private void ClearDictionaryData()
    {
        if (dictionaryManager != null)
        {
            dictionaryManager.ClearDictionary();

            DictionaryUI dictionaryUI = FindObjectOfType<DictionaryUI>();
            if (dictionaryUI != null && dictionaryUI.IsOpen)
            {
                dictionaryUI.UpdateDictionaryDisplay();
            }
        }
    }
}
