using UnityEngine;
using UnityEngine.EventSystems;

public class Cleaner : MonoBehaviour
{
    void Awake()
    {
        var allListeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (allListeners.Length > 1)
        {
            foreach (var listener in allListeners)
            {
                if (listener != Camera.main.GetComponent<AudioListener>())
                {
                    listener.enabled = false;
                }
            }
        }

        if (EventSystem.current != null)
        {
            var allES = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
            foreach (var es in allES)
            {
                if (es != EventSystem.current)
                {
                    Destroy(es.gameObject);
                }
            }
        }
    }
}
