using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingDialoguePlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;
    // Nếu bạn dùng Text thường thì đổi thành:
    // [SerializeField] private Text dialogText;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private string[] currentLines;
    private int currentIndex = 0;
    private bool isTyping = false;

    public System.Action OnDialogueFinished;

    void Update()
    {
        if (dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogText.text = currentLines[currentIndex];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void PlayDialogue(string[] lines)
    {
        currentLines = lines;
        currentIndex = 0;

        dialogPanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in currentLines[currentIndex])
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        StartCoroutine(TypeLine());
    }

    void EndDialogue()
    {
        dialogPanel.SetActive(false);
        OnDialogueFinished?.Invoke();
    }
}