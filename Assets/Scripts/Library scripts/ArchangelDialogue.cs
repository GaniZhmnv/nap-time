using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArchangelDialogue : MonoBehaviour
{
    [Header("Dialogue UI (Same as Ghost)")]
    public GameObject dialoguePanel;
    public GameObject dialogueBorder;
    public GameObject pressETextObject;
    public TMP_Text dialogueText;

    [Header("Choice Panel UI (New)")]
    public GameObject choicePanel;
    public GameObject errorPanel;
    public Button yesButton;
    public Button noButton;
    public Button errorOkButton;

    [Header("Settings")]
    public float typingSpeed = 0.04f;
    public string nextSceneName = "LibraryMaze"; // Name of your maze scene

    private bool playerNear = false;
    private bool dialogueOpen = false;
    private bool isTyping = false;

    private int dialogueIndex = 0;
    private Coroutine typingCoroutine;

    // Archangel's Dialogue Lines
    private string[] dialogueLines =
    {
        "Welcome, traveler.",
        "You seek a way home, do you not?",
        "To find your path, you must pass through this Library.",
        "I will give you a map... but it may not make sense at first.",
        "Follow the signs. Trust your instincts.",
        "Will you enter the Library now?"
    };

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueBorder.SetActive(false);
        pressETextObject.SetActive(false);
        dialogueText.text = "";

        if (choicePanel) choicePanel.SetActive(false);
        if (errorPanel) errorPanel.SetActive(false);

        // Link buttons to functions
        if (yesButton) yesButton.onClick.AddListener(LoadNextScene);
        if (noButton) noButton.onClick.AddListener(ShowError);
        if (errorOkButton) errorOkButton.onClick.AddListener(HideError);
    }

    void Update()
    {
        if (playerNear && !dialogueOpen && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        if (dialogueOpen && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping) ShowFullLine();
            else NextDialogueLine();
        }

        if (dialogueOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueOpen = true;
        dialogueIndex = 0;

        pressETextObject.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueBorder.SetActive(true);

        StartTypingLine();
    }

    void StartTypingLine()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(dialogueLines[dialogueIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void ShowFullLine()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueText.text = dialogueLines[dialogueIndex];
        isTyping = false;
    }

    void NextDialogueLine()
    {
        dialogueIndex++;
        if (dialogueIndex < dialogueLines.Length) StartTypingLine();
        else EndDialogue();
    }

    void EndDialogue()
    {
        dialogueOpen = false;
        isTyping = false;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueText.text = "";

        dialoguePanel.SetActive(false);
        dialogueBorder.SetActive(false);

        if (playerNear) pressETextObject.SetActive(true);

        // --- THIS IS THE MAGIC PART ---
        // When dialogue ends, show the Choice Panel!
        if (choicePanel) choicePanel.SetActive(true);
    }

    // --- Choice Panel Functions ---
    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void ShowError()
    {
        if (errorPanel) errorPanel.SetActive(true);
    }

    void HideError()
    {
        if (errorPanel) errorPanel.SetActive(false);
    }

    // --- Triggers (Copied from your Ghost script) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            // Don't show "Press E" if the choice panel is already open
            if (!dialogueOpen && (choicePanel == null || !choicePanel.activeSelf))
            {
                pressETextObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            // Only close dialogue if it's currently open
            if (dialogueOpen)
            {
                dialogueOpen = false;
                dialogueIndex = 0;
                isTyping = false;

                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                dialogueText.text = "";

                pressETextObject.SetActive(false);
                dialoguePanel.SetActive(false);
                dialogueBorder.SetActive(false);
            }
            else
            {
                pressETextObject.SetActive(false);
            }
        }
    }
}