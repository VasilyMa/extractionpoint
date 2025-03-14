using UnityEngine;

public class AmbientRandomSound : MonoBehaviour
{
    public AudioClip[] ambientSounds; // Массив звуков для эмбиента
    public float minWaitTime = 2.0f;  // Минимальное время ожидания между звуками
    public float maxWaitTime = 6.0f;  // Максимальное время ожидания между звуками

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomAmbientSound(); // Начнем со случайного звука
    }

    void PlayRandomAmbientSound()
    {
        if (ambientSounds.Length == 0)
            return; // Если массив пуст, ничего не делаем

        // Выбираем случайный звук из массива
        AudioClip randomSound = ambientSounds[Random.Range(0, ambientSounds.Length)];

        // Проигрываем звук
        audioSource.clip = randomSound;
        audioSource.Play();

        // Запускаем корутину для ожидания окончания звука
        StartCoroutine(WaitForSoundToFinish());
    }

    System.Collections.IEnumerator WaitForSoundToFinish()
    {
        // Ждем окончания звука
        yield return new WaitForSeconds(audioSource.clip.length);

        // Ждем случайное время перед воспроизведением нового звука
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        // Воспроизводим следующий звук
        PlayRandomAmbientSound();
    }
}
