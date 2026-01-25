using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance;

    [SerializeField] GameObject dialogPanel;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Button nextButton;

    string[] sentences;
    int index;
    System.Action onFinish;

    void Awake()
    {
        Instance = this;
        dialogPanel.SetActive(false);
        nextButton.onClick.AddListener(Next);
    }

    public void Show(string[] lines, System.Action finishCallback = null)
    {
        sentences = lines;
        index = 0;
        onFinish = finishCallback;

        dialogPanel.SetActive(true);
        dialogText.text = sentences[index];
    }

    void Next()
    {
        index++;

        if (index >= sentences.Length)
        {
            dialogPanel.SetActive(false);
            onFinish?.Invoke();
            return;
        }

        dialogText.text = sentences[index];
    }
}
