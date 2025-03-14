using UnityEngine;

public class HandRecoil : MonoBehaviour
{
    public Transform hand; // Ссылка на трансформ руки (Hand_R)
    public float recoilAmount = 0.2f; // Сила отдачи по оси Y
    public float recoilXOffset = 0.1f; // Сила отдачи по оси X
    public float recoilDuration = 0.1f; // Длительность отдачи
    public float returnSpeed = 5f; // Скорость возврата руки

    private Vector3 initialPosition; // Исходная позиция руки
    private float recoilTimer = 0f; // Таймер для контроля отдачи

    private void Start()
    {
        initialPosition = hand.localPosition; // Сохраняем исходную позицию
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Или ваша логика для выстрела
        {
            // Ваш код для стрельбы
            TriggerRecoil(); // Вызываем отдачу
        }

        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime; // Уменьшаем таймер
            if (recoilTimer <= 0)
            {
                recoilTimer = 0; // Устанавливаем таймер в ноль
            }
            else
            {
                // Смещение руки при отдаче
                Vector3 recoilOffset = new Vector3(recoilXOffset, recoilAmount, -recoilAmount);
                hand.localPosition = initialPosition + recoilOffset; // Применяем отдачу к руке
            }
        }
        else
        {
            // Возвращаем руку в исходное положение
            hand.localPosition = Vector3.Lerp(hand.localPosition, initialPosition, returnSpeed * Time.deltaTime);
        }
    }

    public void TriggerRecoil()
    {
        recoilTimer = recoilDuration; // Начинаем отсчет отдачи
    }
}
