using UnityEngine;
using UnityEngine.UI;

public class LevelSelectDictionary : MonoBehaviour
{
    [SerializeField] private DictionaryUI targetDictionaryUI;
    [SerializeField] private Button openDictionaryButton;

    private bool isOpen = false;

    void Start()
    {
        
        if (openDictionaryButton != null)
        {
            openDictionaryButton.onClick.AddListener(ToggleDictionary);
        }
       
    }


    public void ToggleDictionary()
    {

        if (targetDictionaryUI == null)
        {
            targetDictionaryUI = FindObjectOfType<DictionaryUI>();
            if (targetDictionaryUI == null)
            {
                return;
            }
        }

        targetDictionaryUI.ToggleDictionary();
    }
}
