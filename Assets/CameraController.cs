using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Ссылка на объект игрока
    public float distance = 5.0f; // Исходное расстояние до игрока
    public float height = 3.0f; // Высота камеры над игроком
    public float rotationSpeed = 20.0f; // Скорость вращения камеры
    public float zoomSpeed = 2.0f; // Скорость приближения/отдаления
    public float dynamicOffsetStrength = 2.0f; // Сила динамического оффсета

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

        // Динамическое изменение расстояния (плавное приближение/отдаление)
        float targetDistance = distance + Mathf.Sin(Time.time * zoomSpeed) * 2.0f;
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime);

        // Динамический оффсет (колебания камеры)
        float offsetX = Mathf.Sin(Time.time * 1.5f) * dynamicOffsetStrength;
        float offsetY = Mathf.Cos(Time.time * 1.2f) * dynamicOffsetStrength;

        // Вращение камеры вокруг игрока
        currentRotationAngle += rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Позиция камеры с учетом динамического оффсета и высоты
        Vector3 targetPosition = target.position + rotation * new Vector3(offsetX, 0, -currentDistance);
        targetPosition.y = Mathf.Max(target.position.y + currentHeight + offsetY, 1.0f);

        // Плавное перемещение камеры к новой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3.0f);

        // Смотрим на игрока
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
