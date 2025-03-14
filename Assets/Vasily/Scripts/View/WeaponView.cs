using UnityEditor;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    public GameObject WeaponGameObject;
    public Vector3 WeaponOffset;
    public Transform FirePointTransform;
    public WeaponHandsData HandsData;
    [Header("Recoil")]
    public float RecoilStrengh;
    public float RecoilCameraShakeStrengh;
    [Header("Magazine View")]
    public Transform Magazine;
    public Rigidbody MagazineRigidbody;
    public Vector3 MagazineOriginalLocalPosition;
    public AnimatorOverrideController AnimatorOverrideController;
    public AudioSource ShootAudioSource;
    public AudioSource ReloadAudioSource;
    [Header("Particles")]
    public ParticleSystem GilzaParticle;
    public ParticleSystem MuzzleParticle;

#if UNITY_EDITOR
    public void OnValidate()
    {
        WeaponGameObject = this.gameObject;
        FirePointTransform = this.transform.Find("FirePoint");
        if(Magazine)
        MagazineOriginalLocalPosition = Magazine.localPosition;
        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.ToLower().Contains("mag")) // Проверка на наличие "mag" в названии
            {
                Magazine = child;
                if(child.GetComponent<Rigidbody>() != null)
                MagazineRigidbody = child.GetComponent<Rigidbody>();    
            }
        }
        /*if (HandsData == null)
        {
            string configPath = $"Assets/Vasily/Data/WeaponHandsConfigs/{WeaponGameObject.name}Hands.asset";
            if(AssetDatabase.LoadAssetAtPath<WeaponHandsData>(configPath) != null)
            HandsData = AssetDatabase.LoadAssetAtPath<WeaponHandsData>(configPath);
        }*/
    }
#endif
}
