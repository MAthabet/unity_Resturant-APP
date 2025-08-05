using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    SpriteRenderer background;
    [SerializeField]
    float smoothSpeed = 0.125f;

    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private float camWidth;

    void Start()
    {
        cam = GetComponent<Camera>();

        camWidth = cam.orthographicSize * cam.aspect;

        minBounds = background.bounds.min;
        maxBounds = background.bounds.max;
    }

    void LateUpdate()
    {
        float clampedX = Mathf.Clamp(player.position.x, minBounds.x + camWidth, maxBounds.x - camWidth);

        Vector3 desiredPosition = new Vector3(clampedX, transform.position.y, transform.position.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }

}
