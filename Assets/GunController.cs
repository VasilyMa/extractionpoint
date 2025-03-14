using UnityEngine;
using UnityEngine.Animations.Rigging; // �� �������� ���������� ���� namespace
using System.Collections;

public class GunController : MonoBehaviour
{
    public Animator animator; // ������ �� Animator
    public Rig rig; // ��������� Rig
    public RigBuilder rigBuilder; // ��������� Rig
    private float rigWeightTarget = 1f; // ������� �������� ����
    public Transform target;
    public float RotationSpeed;
    private void Update()
    {
      // KeepCharacterAligned();
    }
/*
    private IEnumerator Reload()
    {
        // ��������� �������� �����������
        animator.SetTrigger("ReloadTrigger");

        // ��������� ��� ������ (rig.weight) �� 0 �� 0.1 �������
        float time = 0;
        float duration = 0.2f;

        while (time < duration)
        {
            rig.weight = Mathf.Lerp(1f, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rig.weight = 0f; // ��������, ��� ��� ������������� 0

        // ���� ���������� ��������
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ���������� ��� ������� � 1 �� 0.1 �������
        time = 0;
        while (time < duration)
        {
            rig.weight = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rig.weight = 1f; // ��������, ��� ��� ������������� 1
    }*/
    private void KeepCharacterAligned()
    {
        if (target != null)
        {
            // �������� ����������� � ������� � ������������� ������� Y = 1
            Vector3 directionToTarget = new Vector3(target.position.x, 1f, target.position.z) - new Vector3(transform.position.x, 1f, transform.position.z);

            // ���������, ���� �� �����-�� �����������
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed); // ������ �������
            }
        }
    }
}
