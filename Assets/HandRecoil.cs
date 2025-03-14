using UnityEngine;

public class HandRecoil : MonoBehaviour
{
    public Transform hand; // ������ �� ��������� ���� (Hand_R)
    public float recoilAmount = 0.2f; // ���� ������ �� ��� Y
    public float recoilXOffset = 0.1f; // ���� ������ �� ��� X
    public float recoilDuration = 0.1f; // ������������ ������
    public float returnSpeed = 5f; // �������� �������� ����

    private Vector3 initialPosition; // �������� ������� ����
    private float recoilTimer = 0f; // ������ ��� �������� ������

    private void Start()
    {
        initialPosition = hand.localPosition; // ��������� �������� �������
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) // ��� ���� ������ ��� ��������
        {
            // ��� ��� ��� ��������
            TriggerRecoil(); // �������� ������
        }

        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime; // ��������� ������
            if (recoilTimer <= 0)
            {
                recoilTimer = 0; // ������������� ������ � ����
            }
            else
            {
                // �������� ���� ��� ������
                Vector3 recoilOffset = new Vector3(recoilXOffset, recoilAmount, -recoilAmount);
                hand.localPosition = initialPosition + recoilOffset; // ��������� ������ � ����
            }
        }
        else
        {
            // ���������� ���� � �������� ���������
            hand.localPosition = Vector3.Lerp(hand.localPosition, initialPosition, returnSpeed * Time.deltaTime);
        }
    }

    public void TriggerRecoil()
    {
        recoilTimer = recoilDuration; // �������� ������ ������
    }
}
