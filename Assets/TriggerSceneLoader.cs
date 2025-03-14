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

            // �������� �������
            for (int i = 0; i < ObjectsToEnable.Length; i++)
            {
                ObjectsToEnable[i].SetActive(true);
            }

            // ��������� �������� � ��������� 60 ������
            StartCoroutine(CompleteMissionAfterDelay(60f));
        }
    }

    private IEnumerator CompleteMissionAfterDelay(float delay)
    {
        // �������� � ������� delay ������
        yield return new WaitForSeconds(delay);

        // �������� ����� ����� ��������
        MissionCheckComplete();
    }

    public void MissionCheckComplete()
    {
        // ��������� ������� �� ������
        ConfigModule.GetConfig<PlayFabConfig>().RequestMissionReward("Easy");

        // �������� ������� � Photon
        Photon.Pun.PhotonNetwork.LeaveRoom();

        // ��������� ����� � �������� 2
        SceneManager.LoadScene(2);
    }
}
