using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorTrigger2D : MonoBehaviour
{
    public GameObject winPanel;
    public Button menuButton;
    public string levelSelectSceneName = "LevelSelect";
    public string mainMenuSceneName = "MainMenu";
    public bool hidePlayerOnWin = true;

    private Transform playerTransform;
    private Rigidbody2D playerRigidbody;
    private Renderer playerRenderer;

    private void Start()
    {
        winPanel.SetActive(false);

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ExitToMenu);
        }

        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
        playerRenderer = playerTransform.GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            winPanel.SetActive(true);
            HandleWinSequence();
        }
    }

    private void HandleWinSequence()
    {
      
        DisablePlayerControls();
        LockPlayerMovement();

        if (hidePlayerOnWin)
        {
            HidePlayer();
        }
    }

    void ExitToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void DisablePlayerControls()
    {
        var movement = playerTransform.GetComponent<PlayerController>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }

    private void LockPlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX |
                                         RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void HidePlayer()
    {
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }
    }

    public void RestorePlayer()
    {
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
        }
        var movement = playerTransform.GetComponent<PlayerController>();
        if (movement != null)
        {
            movement.enabled = true;
        }
    }
}
