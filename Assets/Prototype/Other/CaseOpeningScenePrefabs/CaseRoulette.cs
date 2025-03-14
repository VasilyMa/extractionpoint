using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseRoulette : MonoBehaviour
{
    public Animator caseAnimator;
    public ParticleSystem[] rarityParticles;
    public List<ImageChance> _imagesCommon = new List<ImageChance>();
    public List<ImageChance> _imagesRare = new List<ImageChance>();
    public List<ImageChance> _imagesLegendary = new List<ImageChance>();
    public int rarityIndexDrop;
    public Sprite commonEffect;
    public Sprite rareEffect;
    public Sprite legendaryEffect;
    public Image _imageDrop; // �����������, ������� ��������
    public Image _imagePreDrop; // ����������� � ������� �������
    public List<Image> _imagesOther = new List<Image>(); // ����������� � ������� �������

    public RectTransform RectTransformPanel;
    public Vector3 _initialPosition;



    ///new data
    public Transform CellParent;
    public CaseBase CaseBase;
    private void Start()
    {
        _initialPosition = RectTransformPanel.transform.localPosition;
    }

    public void LaunchRoulette()
    {
        // ��������� �������� �������
        foreach (var image in _imagesOther)
        {
            image.transform.GetChild(0).GetComponent<Image>().sprite = GetRandomImage(_imagesCommon, _imagesRare, _imagesLegendary).Item1;
            image.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetRandomImage(_imagesCommon, _imagesRare, _imagesLegendary).Item2;
        }
        // �������� �����������
        _imageDrop.transform.GetChild(0).GetComponent<Image>().sprite = GetRandomDropImage(_imagesCommon, _imagesRare, _imagesLegendary).Item1;
        _imageDrop.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetRandomDropImage(_imagesCommon, _imagesRare, _imagesLegendary).Item2;
        _imagePreDrop.transform.GetChild(0).GetComponent<Image>().sprite = GetPreferredImage(_imagesCommon, _imagesRare, _imagesLegendary).Item1;
        _imagePreDrop.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetPreferredImage(_imagesCommon, _imagesRare, _imagesLegendary).Item2;

        RectTransformPanel.transform.DOLocalMoveX(-4500 + Random.Range(-140, 40), 7.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(ResetPositionRoulette);
        
    }

    public void ResetPositionRoulette()
    {
        rarityParticles[rarityIndexDrop].Play();
        gameObject.SetActive(false);
        RectTransformPanel.transform.localPosition = _initialPosition;
        caseAnimator.CrossFade("CaseClose",0);
    }

    private (Sprite,Sprite) GetRandomImage(List<ImageChance> commonImages, List<ImageChance> rareImages, List<ImageChance> legendaryImages)
    {
        float totalChance = 0;

        // ������� ����� �����������
        foreach (var img in commonImages)
        {
            totalChance += img.chance;
        }
        foreach (var img in rareImages)
        {
            totalChance += img.chance;
        }
        foreach (var img in legendaryImages)
        {
            totalChance += img.chance;
        }

        // �������� ��������� �����
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0;

        // �������� ����������� �� ������ �����������
        foreach (var img in commonImages)
        {
            cumulativeChance += img.chance;
            if (randomValue <= cumulativeChance)
                return (img.Sprite, commonEffect);
        }
        foreach (var img in rareImages)
        {
            cumulativeChance += img.chance;
            if (randomValue <= cumulativeChance)
                return (img.Sprite, rareEffect);
        }
        foreach (var img in legendaryImages)
        {
            cumulativeChance += img.chance;
            if (randomValue <= cumulativeChance)
                return (img.Sprite, legendaryEffect);
        }

        // ���������� ������ ����������� �� ���������, ���� ������ �� ������� (�� ������ ������)
        return (commonImages[0].Sprite,commonEffect);
    }

    private (Sprite,Sprite) GetPreferredImage(List<ImageChance> commonImages, List<ImageChance> rareImages, List<ImageChance> legendaryImages)
    {

        // ������� ������, �� �������� � ����� �������� �������
        // �� ������ �������� ����� ��� ��������� ��������, ����� ��� ����� ���� �������� ���������
        float totalChance = 0;

        foreach (var img in rareImages) // ������ ����� ����������, � ����������� �� ����� ������������
        {
            totalChance += img.chance * 1.5f; // ����������� ����� ��� ������ �����������
        }
        foreach (var img in legendaryImages)
        {
            totalChance += img.chance * 2f; // ����������� ����� ��� ����������� �����������
        }

        // ��������� ����� ����� �� ��� � ����
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0;

        foreach (var img in rareImages)
        {
            cumulativeChance += img.chance * 1.5f;
            if (randomValue <= cumulativeChance)
                return (img.Sprite, rareEffect);
        }
        foreach (var img in legendaryImages)
        {
            cumulativeChance += img.chance * 10f;
            if (randomValue <= cumulativeChance)
                return (img.Sprite, legendaryEffect);
        }

        // ���� ������ �� �������, �� ���������� ��������� ����������� �� �����
        return (commonImages[Random.Range(0, commonImages.Count)].Sprite,commonEffect);
    }
    private (Sprite, Sprite) GetRandomDropImage(List<ImageChance> commonImages, List<ImageChance> rareImages, List<ImageChance> legendaryImages)
    {
        float totalChance = 0;

        // ������� ����� �����������
        foreach (var img in commonImages)
        {
            totalChance += img.chance; 
        }
        foreach (var img in rareImages)
        {
            totalChance += img.chance;
        }
        foreach (var img in legendaryImages)
        {
            totalChance += img.chance;
        }

        // �������� ��������� �����
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0;

        // �������� ����������� �� ������ �����������
        foreach (var img in commonImages)
        {
            cumulativeChance += img.chance;
            if (randomValue <= cumulativeChance)
            {
                rarityIndexDrop = 0;
                return (img.Sprite, commonEffect);
            }
        }
        foreach (var img in rareImages)
        {
            cumulativeChance += img.chance;
            if (randomValue <= cumulativeChance)
            {
                rarityIndexDrop = 1;
                return (img.Sprite, rareEffect);

            }
        }
        foreach (var img in legendaryImages)
        {
            cumulativeChance += img.chance;
            if (randomValue <= cumulativeChance)
            {
                rarityIndexDrop = 2;
                return (img.Sprite, legendaryEffect);

            }
        }

        // ���������� ������ ����������� �� ���������, ���� ������ �� ������� (�� ������ ������)
        rarityIndexDrop = 1;
        return (commonImages[0].Sprite, commonEffect);
    }
}
[System.Serializable]
public class ImageChance
{
    public Sprite Sprite; // �����������
    public float chance; // ����������� (0 �� 1)
}
