using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterRotationController : MonoBehaviour
{
    public Transform spine; // ������� spine_1
    public Transform characterBody; // ������� ������ ���������
    public Transform target; // ������� ����, � ������� ����� �������������� ��������
    public float rotationThreshold = 90f; // ����� �������� � ��������
    public float rotationSpeed = 5f; // �������� ��������

    public Rig rig;
    public Animator anim;

    public float weightChangeDuration = 0.5f; // ������������ ��������� ����
    public float animationDelay = 0.2f; // �������� ����� ������� �������� � ����������� ����
    public float endDelay = 0.2f; // �������� ����� ����������� �������� � ����������� ����

    private void Start()
    {
        anim = GetComponent<Animator>();

        // ��������� ������� �����������
        if (rig == null)
        {
            Debug.LogError("Rig component not found on " + gameObject.name);
        }

        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ReloadCoroutine());
        }

        // ���� ������ �������� ����� �����, ���� ��� ����������.
    }

    private IEnumerator ReloadCoroutine()
    {
        // �������� �������� �����������
        anim.CrossFade("Reload", 0.15f);

        // ���� animationDelay ����� ����������� ����
        yield return new WaitForSeconds(animationDelay);

        // ��������� ������ �������� ��� ���������� ����
        yield return StartCoroutine(ReduceRigWeightCoroutine());

        // ���� ��������� ��������
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            yield return null; // ���� ��������� ����
        }

        // ���� endDelay ����� ����������� ����
        yield return new WaitForSeconds(endDelay);

        // ��������� ������ �������� ��� ���������� ����
        yield return StartCoroutine(IncreaseRigWeightCoroutine());
    }

    private IEnumerator ReduceRigWeightCoroutine()
    {
        float elapsedTime = 0f;

        // ��������� ��� �� weightChangeDuration
        while (elapsedTime < weightChangeDuration)
        {
            rig.weight = Mathf.Lerp(1, 0, elapsedTime / weightChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� ��������� ����
        }

        // ������������� ��� ���� �� 0
        rig.weight = 0;
    }

    private IEnumerator IncreaseRigWeightCoroutine()
    {
        float elapsedTime = 0f;

        // ����������� ��� �� weightChangeDuration
        while (elapsedTime < weightChangeDuration)
        {
            rig.weight = Mathf.Lerp(0, 1, elapsedTime / weightChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� ��������� ����
        }

        // ����������, ��� ��� ���� ���������� �� 1 ����� ����������
        rig.weight = 1;
    }
}
