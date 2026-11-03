using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private Vector2 paddleSize = new Vector2(0.2f, 1.5f);
    [SerializeField] private float paddleOffsetFromCentre = 7f;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Vector2 ballSize = new Vector2(1.0f, 1.0f);

    private enum CutAxis { Vertical, Horizontal }

    private GameObject leftPaddle;
    private GameObject rightPaddle;
    private GameObject ball;

    // Collision debug view, toggled on C: partitions without the ball get a red transparent overlay, white lines separate partitions.
    private const float DebugLineWidth = 0.03f;
    private static readonly Color DebugOverlayColor = new Color(1f, 0f, 0f, 0.25f);
    private bool collisionDebug;
    private Sprite debugSprite;
    private readonly List<SpriteRenderer> debugPool = new List<SpriteRenderer>();
    private int debugUsed;
    float logTimer;

    private void Start()
    {
        leftPaddle = SpawnPaddle(-paddleOffsetFromCentre, Key.W, Key.S);
        rightPaddle = SpawnPaddle(paddleOffsetFromCentre, Key.UpArrow, Key.DownArrow);
        ball = SpawnBall();

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        debugSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
    }

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame) collisionDebug = !collisionDebug;
        debugUsed = 0;
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            SpawnBall();
        }

        List<GameObject> objects = new List<GameObject> { leftPaddle, rightPaddle, ball };

        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        Vector2 centre = Camera.main.transform.position;
        Rect screen = new Rect(centre.x - width / 2f, centre.y - height / 2f, width, height);

        List<List<GameObject>> finalObjs = BroadCollision(screen, CutAxis.Vertical, objects, new List<List<GameObject>>(), 0);

        logTimer += Time.deltaTime;
        if (logTimer >= 3f)
        {
            logTimer = 0f;
            Debug.Log($"{finalObjs.Count} groups\n");
            Debug.Log(string.Join("\n", finalObjs.Select((group, i) =>
            $"Group {i}: [{string.Join(", ", group.Select(o => o.name))}]")));
        }
    

        // Hide debug quads not used this frame.
        for (int i = debugUsed; i < debugPool.Count; i++)
        {
            debugPool[i].gameObject.SetActive(false);
        }
    }

    private GameObject SpawnPaddle(float x, Key up, Key down)
    {
        GameObject paddle = Instantiate(paddlePrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
        paddle.GetComponent<SpriteRenderer>().size = paddleSize;
        paddle.GetComponent<Paddle>().SetKeys(up, down);
        return paddle;
    }

    private GameObject SpawnBall(float x = 0f, float y = 0f)
    {
        GameObject ball = Instantiate(ballPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        ball.GetComponent<SpriteRenderer>().size = ballSize;
        return ball;
    }

    // Broad phase: recursively halve the screen, up to 4 times, recursing only into partitions holding 2 or more objects.
    // Each group in finalObjs needs narrow phase checks only between its own members.
    private List<List<GameObject>> BroadCollision(Rect screen, CutAxis cutAxis, List<GameObject> objsToCheck, List<List<GameObject>> finalObjs, int partNum)
    {
        if (partNum >= 4)
        {
            finalObjs.Add(objsToCheck);
            return finalObjs;
        }

        Rect partition1;
        Rect partition2;
        Rect cutLine;
        if (cutAxis == CutAxis.Vertical)
        {
            // A vertical line makes left and right halves.
            partition1 = new Rect(screen.x, screen.y, screen.width / 2f, screen.height);
            partition2 = new Rect(screen.x + screen.width / 2f, screen.y, screen.width / 2f, screen.height);
            cutLine = new Rect(partition2.x - DebugLineWidth / 2f, screen.y, DebugLineWidth, screen.height);
        }
        else
        {
            partition1 = new Rect(screen.x, screen.y, screen.width, screen.height / 2f);
            partition2 = new Rect(screen.x, screen.y + screen.height / 2f, screen.width, screen.height / 2f);
            cutLine = new Rect(screen.x, partition2.y - DebugLineWidth / 2f, screen.width, DebugLineWidth);
        }

        List<GameObject> partitionObjectList1 = new List<GameObject>();
        List<GameObject> partitionObjectList2 = new List<GameObject>();
        foreach (GameObject obj in objsToCheck)
        {
            if (HasCornerIn(obj, partition1)) partitionObjectList1.Add(obj);
            if (HasCornerIn(obj, partition2)) partitionObjectList2.Add(obj);
        }

        if (collisionDebug)
        {
            if (!partitionObjectList1.Contains(ball)) DrawDebugRect(partition1, DebugOverlayColor, 100);
            if (!partitionObjectList2.Contains(ball)) DrawDebugRect(partition2, DebugOverlayColor, 100);
            DrawDebugRect(cutLine, Color.white, 101);
        }

        CutAxis flippedAxis = cutAxis == CutAxis.Vertical ? CutAxis.Horizontal : CutAxis.Vertical;
        if (partitionObjectList1.Count >= 2)
        {
            BroadCollision(partition1, flippedAxis, partitionObjectList1, finalObjs, partNum + 1);
        }
        if (partitionObjectList2.Count >= 2)
        {
            BroadCollision(partition2, flippedAxis, partitionObjectList2, finalObjs, partNum + 1);
        }
        return finalObjs;
    }

    // Each corner of the object is checked for whether it lies in the partition.
    private bool HasCornerIn(GameObject obj, Rect partition)
    {
        Bounds bounds = obj.GetComponent<SpriteRenderer>().bounds;
        return partition.Contains(new Vector2(bounds.min.x, bounds.min.y))
            || partition.Contains(new Vector2(bounds.min.x, bounds.max.y))
            || partition.Contains(new Vector2(bounds.max.x, bounds.min.y))
            || partition.Contains(new Vector2(bounds.max.x, bounds.max.y));
    }

    // Draws a coloured rectangle this frame, reusing pooled quads.
    private void DrawDebugRect(Rect rect, Color color, int sortingOrder)
    {
        if (debugUsed == debugPool.Count)
        {
            SpriteRenderer created = new GameObject("CollisionDebugQuad").AddComponent<SpriteRenderer>();
            created.sprite = debugSprite;
            debugPool.Add(created);
        }

        SpriteRenderer quad = debugPool[debugUsed++];
        quad.gameObject.SetActive(true);
        quad.transform.position = new Vector3(rect.center.x, rect.center.y, 0f);
        quad.transform.localScale = new Vector3(rect.width, rect.height, 1f);
        quad.color = color;
        quad.sortingOrder = sortingOrder;
    }
}
