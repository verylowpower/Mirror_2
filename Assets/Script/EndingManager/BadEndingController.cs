using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BadEndingController : MonoBehaviour
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
            "In the final battle, the hero falls before the forest’s dark power.",
            "The corruption is not cleansed. It grows even stronger.",
            "Consumed by the darkness, the hero fades, becoming one with the cursed woods.",
            "The cycle continues, and the world sinks deeper into shadow."
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