using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizDoor : MonoBehaviour
{
    [Header("Настройки двери")]
    public int minWordsRequired = 5;
    [SerializeField] public int numberOfQuestions = 3;
    [SerializeField] public GameObject quizPanel;
    [SerializeField] public TextMeshProUGUI quizWordText;
    [SerializeField] public TMP_InputField translationInput;
    [SerializeField] public Button submitButton;
    [SerializeField] public GameObject winPanel;
    [SerializeField] private DictionaryUI dictionaryUI;

    [SerializeField] public GameObject deathPanel;
    [SerializeField] public TextMeshProUGUI livesText;

    [Header("Настройки игрока")]
    [SerializeField] public Transform player;
    [SerializeField] public Vector3 startPosition;

    private DictionaryManager dictionaryManager;
    private List<WordEntry> levelWords = new List<WordEntry>();
    private int currentQuestion = 0;
    private int correctAnswers = 0;
    private int lives = 3;
    public bool isQuizActive = false;

    void Start()
    {
        dictionaryManager = DictionaryManager.instance;
        quizPanel.SetActive(false);
        winPanel.SetActive(false);

        if (deathPanel != null)
            deathPanel.SetActive(false);

        UpdateLivesText();

        if (submitButton != null)
            submitButton.onClick.AddListener(CheckTranslation);

        UpdateDoorState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CanDoorOpen())
        {
            StartQuiz();
        }
    }

    bool CanDoorOpen()
    {
        levelWords = dictionaryManager.GetFilteredDictionary();
        return levelWords.Count >= minWordsRequired;
    }

    void StartQuiz()
    {
        currentQuestion = 0;
        correctAnswers = 0;
        lives = 3;
        isQuizActive = true;
        UpdateLivesText();
        ShowNextQuestion();

        if (dictionaryUI != null)
        {
            dictionaryUI.CloseDictionary();
        }
    }

    void ShowNextQuestion()
    {
        if (currentQuestion >= numberOfQuestions)
        {
            ShowWinPanel();
            return;
        }

        if (levelWords.Count == 0)
        {
            return;
        }

        WordEntry randomWord = levelWords[Random.Range(0, levelWords.Count)];
        quizWordText.text = randomWord.word;
        translationInput.text = "";

        quizPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void CheckTranslation()
    {
        string userInput = translationInput.text.Trim().ToLower();
        string correctTranslation = GetCorrectTranslation(quizWordText.text);

        bool isCorrect = !string.IsNullOrEmpty(correctTranslation) && userInput == correctTranslation.ToLower();

        if (isCorrect)
        {
            correctAnswers++;
        }
        else
        {
            lives--;
            UpdateLivesText();

            if (lives <= 0)
            {
                if (dictionaryManager != null)
                {
                    dictionaryManager.ClearLevelWords();
                }

                ShowDeathPanel();
                return;
            }
        }

        currentQuestion++;

        quizPanel.SetActive(false);
        Time.timeScale = 1f;
        ShowNextQuestion();
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = $"{lives}";
        }
    }

    string GetCorrectTranslation(string word)
    {
        foreach (WordEntry entry in levelWords)
        {
            if (entry.word.ToLower() == word.ToLower())
                return entry.translation;
        }
        return null;
    }

    void ShowWinPanel()
    {
        quizPanel.SetActive(false);
        winPanel.SetActive(true);
        isQuizActive = false;
        Time.timeScale = 1f;
    }

    void ShowDeathPanel()
    {
        quizPanel.SetActive(false);
        isQuizActive = false;
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
        Time.timeScale = 1f;
    }

    void UpdateDoorState()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = CanDoorOpen() ? Color.green : Color.red;
        }
    }

    void Update()
    {
        UpdateDoorState();
    }
}
