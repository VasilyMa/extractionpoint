using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public Texture2D fullMapTexture;  // ������ �������� �����
    public RawImage miniMapImage;      // RawImage ��� ����������� ����-�����
    public RectTransform windowRect;    // ������������ ������� ��� ���� ����-�����
    public float mapScale = 0.1f;       // ������� ����-�����
    public Transform player;            // ������ �� ������ ������
    private Vector3 playerInitPos;
    private Vector3 mapStartPoint;      // ��������� ����� �����

    void Start()
    {
        // ������������� �������� ��� ����-�����
        miniMapImage.texture = fullMapTexture;

        // ����������� ���� ����-����� � ����������� �� �������� � ��������
        windowRect.sizeDelta = new Vector2(fullMapTexture.width * mapScale, fullMapTexture.height * mapScale);

        // ���������� ��������� ����� ����� (��������, ����� ������ ���� ��������)
        mapStartPoint = new Vector3(-fullMapTexture.width / 2, 0, -fullMapTexture.height / 2);
    }

    void Update()
    {
        if (player != null)
        {
            // ��������� ��������� ����-�����
            UpdateMiniMapPosition(player.position);
        }
        else
        {
            Debug.LogWarning("����� �� ���������� � MiniMapController!");
        }
    }

    public void UpdateMiniMapPosition(Vector3 playerPosition)
    {
        // ������� �����
        playerPosition += playerInitPos*2;
        playerPosition *= 10;
        float mapWidth = fullMapTexture.width * mapScale;
        float mapHeight = fullMapTexture.height * mapScale;

        // ������������ �������� ������ ������������ ��������� ����� �����
        float offsetX = (playerPosition.x - mapStartPoint.x) / mapWidth;
        float offsetY = (playerPosition.z - mapStartPoint.z) / mapHeight;

        // ���������� ���������
        Debug.Log($"PlayerPosition: {playerPosition}, MapStartPoint: {mapStartPoint}, OffsetX: {offsetX}, OffsetY: {offsetY}");

        // ��������� UV ���������� ��� ����������� ����������� � RawImage
        float uvWidth = 1f / mapScale;
        float uvHeight = 1f / mapScale;

        miniMapImage.uvRect = new Rect(offsetX - (uvWidth * 0.5f), offsetY - (uvHeight * 0.5f), uvWidth, uvHeight);

        // ������������ UV-����������, ����� ��� �� �������� �� ������� ��������
        miniMapImage.uvRect = new Rect(
            Mathf.Clamp(miniMapImage.uvRect.x, 0, 1 - uvWidth),
            Mathf.Clamp(miniMapImage.uvRect.y, 0, 1 - uvHeight),
            miniMapImage.uvRect.width,
            miniMapImage.uvRect.height
        );
    }
}
