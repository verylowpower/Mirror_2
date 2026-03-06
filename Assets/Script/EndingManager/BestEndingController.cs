using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BestEndingController : MonoBehaviour
{
    [SerializeField] private EndingDialoguePlayer dialoguePlayer;
    [SerializeField] private string returnScene = "MainMenu";

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2f;

    void Start()
    {
        string[] lines =
        {
            "After uncovering the forgotten memories scattered across the forest, the truth finally reveals itself.",
            "The hero was never an outsider to this land.",
            "At the heart of the ancient woods lies a power of balance… and the hero is its living core.",
            "The guardian of the forest was never meant to be destroyed.",
            "Its purpose was to test the one who would inherit the forest’s fate.",
            "As the final battle ends, the hero chooses neither domination nor escape.",
            "Instead, they merge with the forest’s power, embracing it with control and understanding.",
            "The ancient guardian fades away, its role finally complete.",
            "The corruption dissolves, and life slowly returns to the forest.",
            "Where darkness once ruled, the balance of nature is restored.",
            "The hero does not leave the forest.",
            "Nor do they disappear.",
            "They become the new guardian of the ancient woods.",
            "A silent protector of life, ensuring that the cycle of destruction will never return.",
            "And so, the forest lives on… under the watch of its new keeper."
        };

        dialoguePlayer.PlayDialogue(lines);
        dialoguePlayer.OnDialogueFinished += OnDialogueFinished;
    }

    void OnDialogueFinished()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene(returnScene);
    }
}