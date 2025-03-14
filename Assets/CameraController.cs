using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ������ �� ������ ������
    public float distance = 5.0f; // �������� ���������� �� ������
    public float height = 3.0f; // ������ ������ ��� �������
    public float rotationSpeed = 20.0f; // �������� �������� ������
    public float zoomSpeed = 2.0f; // �������� �����������/���������
    public float dynamicOffsetStrength = 2.0f; // ���� ������������� �������

    private Vector3 offset;
    private float currentDistance;
    private float currentRotationAngle;
    private float currentHeight;

    void Start()
    {
        currentDistance = distance;
        currentHeight = height;
        offset = new Vector3(0, height, -distance);
        target = FindAnyObjectByType<PhotonPlayer>().transform;   
    }

    void LateUpdate()
    {
        if (!target) return;

        // ������������ ��������� ���������� (������� �����������/���������)
        float targetDistance = distance + Mathf.Sin(Time.time * zoomSpeed) * 2.0f;
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime);

        // ������������ ������ (��������� ������)
        float offsetX = Mathf.Sin(Time.time * 1.5f) * dynamicOffsetStrength;
        float offsetY = Mathf.Cos(Time.time * 1.2f) * dynamicOffsetStrength;

        // �������� ������ ������ ������
        currentRotationAngle += rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // ������� ������ � ������ ������������� ������� � ������
        Vector3 targetPosition = target.position + rotation * new Vector3(offsetX, 0, -currentDistance);
        targetPosition.y = Mathf.Max(target.position.y + currentHeight + offsetY, 1.0f);

        // ������� ����������� ������ � ����� �������
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3.0f);

        // ������� �� ������
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
