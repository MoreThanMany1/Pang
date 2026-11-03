using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    private Vector2 direction;
    private bool debugMode;

    private void Start()
    {
        float angle = Random.value < 0.5f
            ? Random.Range(-Mathf.PI / 4f, Mathf.PI / 4f)
            : Random.Range(3f * Mathf.PI / 4f, 5f * Mathf.PI / 4f);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void Update()
    {
        // Debug mode, toggled on D: the ball is stuck on the mouse position instead of moving normally.
        if (Keyboard.current.dKey.wasPressedThisFrame) debugMode = !debugMode;
        if (debugMode)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(mouse.x, mouse.y, 0f);
            return;
        }
        else
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        float cameraEdge = Camera.main.orthographicSize;
        float halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
        Vector3 position = transform.position;

        // Detection: the top of the ball is equal to or greater than the top of the camera screen, mirrored for the bottom.
        if (position.y + halfHeight >= cameraEdge)
        {
            // Resolution: place the ball on the boundary of the side it hit, adjusted for its height, then negate direction.y.
            position.y = cameraEdge - halfHeight;
            direction.y = -direction.y;
        }
        if (position.y - halfHeight <= -cameraEdge)
        {
            position.y = -cameraEdge + halfHeight;
            direction.y = -direction.y;
        }

        transform.position = position;
    }
}
