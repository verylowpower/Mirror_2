using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private float zPosition = 10f;

    private BoxCollider2D mapBounds;

    private float minX, maxX, minY, maxY;
    private float camHalfWidth, camHalfHeight;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        TryCatchPlayer();
        TryFindMapBound();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryCatchPlayer();
        TryFindMapBound();
    }

    void LateUpdate()
    {
        if (_target == null || mapBounds == null) return;

        Vector3 targetPos = _target.position;

        float clampX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampY = Mathf.Clamp(targetPos.y, minY, maxY);

        _camera.transform.position =
            new Vector3(clampX, clampY, -zPosition);
    }

    void TryCatchPlayer()
    {
        if (PlayerController.instance != null)
            _target = PlayerController.instance.transform;
    }

    void TryFindMapBound()
    {
        GameObject boundObj = GameObject.FindGameObjectWithTag("MapBound");

        if (boundObj == null)
        {
            Debug.LogWarning("Found MapBound");
            return;
        }

        mapBounds = boundObj.GetComponent<BoxCollider2D>();

        if (mapBounds == null)
        {
            Debug.LogWarning("Not Found MapBound");
            return;
        }

        CalculateCameraBounds();
    }

    void CalculateCameraBounds()
    {
        camHalfHeight = _camera.orthographicSize;
        camHalfWidth = camHalfHeight * _camera.aspect;

        Bounds bounds = mapBounds.bounds;

        minX = bounds.min.x + camHalfWidth;
        maxX = bounds.max.x - camHalfWidth;
        minY = bounds.min.y + camHalfHeight;
        maxY = bounds.max.y - camHalfHeight;
    }
}
