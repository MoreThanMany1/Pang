using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private Vector2 paddleSize = new Vector2(0.2f, 1.5f);
    [SerializeField] private float paddleOffsetFromCentre = 7f;

    private void Start()
    {
        SpawnPaddle(-paddleOffsetFromCentre);
        SpawnPaddle(paddleOffsetFromCentre);
    }

    private void SpawnPaddle(float x)
    {
        GameObject paddle = Instantiate(paddlePrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
        paddle.GetComponent<SpriteRenderer>().size = paddleSize;
    }
}
