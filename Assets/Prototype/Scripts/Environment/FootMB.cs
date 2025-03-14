using UnityEngine;

public class FootMB : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float fadeDuration = 2.0f; // Длительность затухания в секундах
    private float timer = 0.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        timer = 0.0f;
        Color color = spriteRenderer.color;
        color.a = 0.25f; 
        spriteRenderer.color = color;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(0.25f, 0.0f, timer / fadeDuration);
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;

        if (timer >= fadeDuration)
        {
            gameObject.SetActive(false); 
        }
    }
}
