using UnityEngine;

public class GhostFloatAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite ghostSprite1;
    public Sprite ghostSprite2;

    public float spriteChangeSpeed = 0.7f;
    public float floatHeight = 0.0003f;
    public float floatSpeed = 0.35f;

    private Vector3 startPosition;
    private float spriteTimer;
    private bool useFirstSprite = true;

    void Start()
    {
        startPosition = transform.position;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null && ghostSprite1 != null)
        {
            spriteRenderer.sprite = ghostSprite1;
        }
    }

    void Update()
    {
        FloatUpAndDown();
        ChangeSprite();
    }

    void FloatUpAndDown()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void ChangeSprite()
    {
        if (spriteRenderer == null || ghostSprite1 == null || ghostSprite2 == null)
            return;

        spriteTimer += Time.deltaTime;

        if (spriteTimer >= spriteChangeSpeed)
        {
            spriteTimer = 0f;
            useFirstSprite = !useFirstSprite;

            if (useFirstSprite)
            {
                spriteRenderer.sprite = ghostSprite1;
            }
            else
            {
                spriteRenderer.sprite = ghostSprite2;
            }
        }
    }
}