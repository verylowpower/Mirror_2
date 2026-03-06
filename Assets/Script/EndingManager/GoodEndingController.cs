using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GoodEndingController : MonoBehaviour
{
    [SerializeField] private EndingDialoguePlayer dialoguePlayer;
    [SerializeField] private string returnScene = "Menu";

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2f;

    void Start()
    {
        string[] lines =
        {
            "The ancient power that ruled the forest has finally fallen.",
            "Its corruption fades, carried away by the quiet wind between the trees.",
            "The hero absorbs the last fragments of its strength, breaking the long shadow cast upon the woods.",
            "For the first time in ages, the forest falls silent.",
            "The creatures that once hunted in darkness vanish with the fading curse.",
            "Yet the truth behind the forest’s origin remains incomplete.",
            "With unanswered questions lingering in mind, the hero steps beyond the ancient trees.",
            "A new path awaits... somewhere beyond the forest."
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