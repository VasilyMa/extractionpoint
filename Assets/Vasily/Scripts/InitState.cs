using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class InitState : State
{
    public bool IsTest;
    public List<WeaponBase> WeaponToTest;
    public List<EquipBase> TestEquip;

    public static new InitState Instance
    {
        get
        {
            return (InitState)State.Instance;
        }
    }

    public string DestinationScene;

    protected override void Awake()
    {
        base.Awake();

        InitCanvas();
        
        GetCanvas<LoadingCanvas>().OpenPanel<DefaultPanel>();

        SaveModule.Initialize();

        ConfigModule.InitConfigs(this, onLoadingConfig);
    }

    protected override void Start()
    {

    }

    void onLoadingConfig()
    {
        EntityModule.Initialize();

        StartCoroutine(awaitSceneLoading());
    }

    IEnumerator awaitSceneLoading()
    {
        yield return new WaitUntil(()=> EntityModule.IsInit);

        if (IsTest)
        {
            foreach (var equip in TestEquip)
            {
                EntityModule.GetEntity<InventoryEntity>().TryAddItem(equip.GetEquip());
            }
        }

        SceneManager.LoadScene(DestinationScene);
    }

    public override void Dispose()
    {

    }
}
