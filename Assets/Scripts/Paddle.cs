using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    private Key upKey;
    private Key downKey;
    private SpriteRenderer spriteRenderer;

    private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

    public void SetKeys(Key up, Key down)
    {
        upKey = up;
        downKey = down;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        float direction = (keyboard[upKey].isPressed ? 1f : 0f) - (keyboard[downKey].isPressed ? 1f : 0f);
        float limit = Camera.main.orthographicSize - spriteRenderer.bounds.extents.y;

        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y + direction * speed * Time.deltaTime, -limit, limit);
        transform.position = position;
    }
}
