using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    [Header("Настройки смерти")]
    public string deathObjectTag = "Death";
    [SerializeField] public GameObject deathPanel;
    [SerializeField] private float fallDuration = 0.5f;

    [SerializeField] private DictionaryManager dictionaryManager;
    [SerializeField] private DictionaryUI dictionaryUI;

    private bool isDead;
    private Vector3 originalPosition;
    private Rigidbody2D rigidbody;
    private bool isPanelActive;
    private bool isFalling;
    private bool isVisible;
    private Button retryButton;
    private Button dictionaryButton;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        isVisible = true;

        if (dictionaryManager == null)
            dictionaryManager = DictionaryManager.instance;
    }

    private void Start()
    {
        retryButton = deathPanel.GetComponentInChildren<Button>();
        dictionaryButton = deathPanel.GetComponentsInChildren<Button>()
            .FirstOrDefault(btn => btn.name.Contains("Dictionary") || btn.name.Contains("Словарь"));

        if (retryButton != null)
            retryButton.onClick.AddListener(RetryLevel);

        if (dictionaryButton != null && dictionaryUI != null)
            dictionaryButton.onClick.AddListener(() => dictionaryUI.ToggleDictionary());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(deathObjectTag) && !isDead)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(deathObjectTag) && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        DisablePlayerControls();
        ShowCursor();

        if (dictionaryManager != null)
        {
            dictionaryManager.ClearLevelWords();
        }

        StartCoroutine(FallAndFreeze());
    }

    private IEnumerator FallAndFreeze()
    {
        isFalling = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(fallDuration);
        LockPlayerMovement();
        ShowDeathPanel();
        HidePlayer();
        isFalling = false;
    }

    private void DisablePlayerControls()
    {
        var movement = GetComponent<PlayerController>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }

    private void LockPlayerMovement()
    {
        if (rigidbody != null)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX |
                                   RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ShowDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            isPanelActive = true;
        }
    }

    private void HidePlayer()
    {
        GetComponent<Renderer>().enabled = false;
        isVisible = false;
    }

    private void RetryLevel()
    {
        if (isPanelActive)
        {
            if (rigidbody != null)
            {
                rigidbody.constraints = RigidbodyConstraints2D.None;
                rigidbody.freezeRotation = true;
            }
            transform.position = originalPosition;
            deathPanel.SetActive(false);
            isPanelActive = false;
            ShowPlayer();
            EnablePlayerControls();
            isDead = false;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowPlayer()
    {
        GetComponent<Renderer>().enabled = true;
        isVisible = true;
    }

    private void EnablePlayerControls()
    {
        var movement = GetComponent<PlayerController>();
        if (movement != null)
        {
            movement.enabled = true;
        }
    }
}
