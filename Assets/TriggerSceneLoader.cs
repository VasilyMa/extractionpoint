using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneLoader : MonoBehaviour
{
    public GameObject[] ObjectsToEnable;
    private bool isMissionTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isMissionTriggered && other.gameObject.layer == 3)
        {
            isMissionTriggered = true;

            // Включаем объекты
            for (int i = 0; i < ObjectsToEnable.Length; i++)
            {
                ObjectsToEnable[i].SetActive(true);
            }

            // Запускаем корутину с задержкой 60 секунд
            StartCoroutine(CompleteMissionAfterDelay(60f));
        }
    }

    private IEnumerator CompleteMissionAfterDelay(float delay)
    {
        // Ожидание в течение delay секунд
        yield return new WaitForSeconds(delay);

        // Вызываем метод после задержки
        MissionCheckComplete();
    }

    public void MissionCheckComplete()
    {
        // Выполняем награду за миссию
        ConfigModule.GetConfig<PlayFabConfig>().RequestMissionReward("Easy");

        // Покидаем комнату в Photon
        Photon.Pun.PhotonNetwork.LeaveRoom();

        // Загружаем сцену с индексом 2
        SceneManager.LoadScene(2);
    }
}
