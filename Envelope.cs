using UnityEngine;

public class Envelope : MonoBehaviour
{
    public string word;
    public string translation;
    public GameObject audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audio.SetActive(true);
            DictionaryManager.AddWord(word, translation);
            DictionaryManager.AddWordToCurrentLevel(word);

            DictionaryUI dictionaryUI = FindObjectOfType<DictionaryUI>();
            if (dictionaryUI != null && dictionaryUI.IsOpen)
            {
                dictionaryUI.UpdateDictionaryDisplay();
            }

            Destroy(gameObject);
        }
    }
}
