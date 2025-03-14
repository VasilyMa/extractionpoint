using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public Texture2D fullMapTexture;  // Полная текстура карты
    public RawImage miniMapImage;      // RawImage для отображения мини-карты
    public RectTransform windowRect;    // Родительский элемент для окна мини-карты
    public float mapScale = 0.1f;       // Масштаб мини-карты
    public Transform player;            // Ссылка на объект игрока
    private Vector3 playerInitPos;
    private Vector3 mapStartPoint;      // Начальная точка карты

    void Start()
    {
        // Устанавливаем текстуру для мини-карты
        miniMapImage.texture = fullMapTexture;

        // Настраиваем окно мини-карты в зависимости от текстуры и масштаба
        windowRect.sizeDelta = new Vector2(fullMapTexture.width * mapScale, fullMapTexture.height * mapScale);

        // Определяем начальную точку карты (например, левый нижний угол текстуры)
        mapStartPoint = new Vector3(-fullMapTexture.width / 2, 0, -fullMapTexture.height / 2);
    }

    void Update()
    {
        if (player != null)
        {
            // Обновляем положение мини-карты
            UpdateMiniMapPosition(player.position);
        }
        else
        {
            Debug.LogWarning("Игрок не установлен в MiniMapController!");
        }
    }

    public void UpdateMiniMapPosition(Vector3 playerPosition)
    {
        // Размеры карты
        playerPosition += playerInitPos*2;
        playerPosition *= 10;
        float mapWidth = fullMapTexture.width * mapScale;
        float mapHeight = fullMapTexture.height * mapScale;

        // Рассчитываем смещение игрока относительно начальной точки карты
        float offsetX = (playerPosition.x - mapStartPoint.x) / mapWidth;
        float offsetY = (playerPosition.z - mapStartPoint.z) / mapHeight;

        // Отладочное сообщение
        Debug.Log($"PlayerPosition: {playerPosition}, MapStartPoint: {mapStartPoint}, OffsetX: {offsetX}, OffsetY: {offsetY}");

        // Обновляем UV координаты для корректного отображения в RawImage
        float uvWidth = 1f / mapScale;
        float uvHeight = 1f / mapScale;

        miniMapImage.uvRect = new Rect(offsetX - (uvWidth * 0.5f), offsetY - (uvHeight * 0.5f), uvWidth, uvHeight);

        // Ограничиваем UV-координаты, чтобы они не выходили за пределы текстуры
        miniMapImage.uvRect = new Rect(
            Mathf.Clamp(miniMapImage.uvRect.x, 0, 1 - uvWidth),
            Mathf.Clamp(miniMapImage.uvRect.y, 0, 1 - uvHeight),
            miniMapImage.uvRect.width,
            miniMapImage.uvRect.height
        );
    }
}
