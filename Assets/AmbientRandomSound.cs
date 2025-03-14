using UnityEngine;

public class AmbientRandomSound : MonoBehaviour
{
    public AudioClip[] ambientSounds; // ������ ������ ��� ��������
    public float minWaitTime = 2.0f;  // ����������� ����� �������� ����� �������
    public float maxWaitTime = 6.0f;  // ������������ ����� �������� ����� �������

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomAmbientSound(); // ������ �� ���������� �����
    }

    void PlayRandomAmbientSound()
    {
        if (ambientSounds.Length == 0)
            return; // ���� ������ ����, ������ �� ������

        // �������� ��������� ���� �� �������
        AudioClip randomSound = ambientSounds[Random.Range(0, ambientSounds.Length)];

        // ����������� ����
        audioSource.clip = randomSound;
        audioSource.Play();

        // ��������� �������� ��� �������� ��������� �����
        StartCoroutine(WaitForSoundToFinish());
    }

    System.Collections.IEnumerator WaitForSoundToFinish()
    {
        // ���� ��������� �����
        yield return new WaitForSeconds(audioSource.clip.length);

        // ���� ��������� ����� ����� ���������������� ������ �����
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        // ������������� ��������� ����
        PlayRandomAmbientSound();
    }
}
