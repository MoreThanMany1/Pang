using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private Vector2 paddleSize = new Vector2(0.2f, 1.5f);
    [SerializeField] private float paddleOffsetFromCentre = 7f;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Vector2 ballSize = new Vector2(1.0f, 1.0f);

    private void Start()
    {
        SpawnPaddle(-paddleOffsetFromCentre, Key.W, Key.S);
        SpawnPaddle(paddleOffsetFromCentre, Key.UpArrow, Key.DownArrow);
        SpawnBall(0f);
    }

    private void SpawnPaddle(float x, Key up, Key down)
    {
        GameObject paddle = Instantiate(paddlePrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
        paddle.GetComponent<SpriteRenderer>().size = paddleSize;
        paddle.GetComponent<Paddle>().SetKeys(up, down);
    }

    private void SpawnBall(float x)
    {
        GameObject ball = Instantiate(ballPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
        ball.GetComponent<SpriteRenderer>().size = ballSize;
    }
}
