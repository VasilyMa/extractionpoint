[System.Serializable]
public class PlayerData : IDatable
{
    public string RankID;
    public int CreditCurrency;
    public int BarrelCurrency;
    public int Experience;
    public WeaponData MainWeapon;
    public WeaponData HeavyWeapon;
    public BattleModuleData BattleModule;
    public ArmorData Head;
    public ArmorData Body;
    public ArmorData Foot;
    public ArmorData Hand;

    public PlayerData()
    {
        CreditCurrency = 0;
        BarrelCurrency = 0;
        Experience = 0;
        RankID = "Recruit";
    }

    public string DATA_ID => "PlayerData_ID"; // name data

    public void ProcessUpdataData()
    {
        if (PlayerEntity.Instance == null) return;

        CreditCurrency = PlayerEntity.Instance.CreditCurrency;
        BarrelCurrency = PlayerEntity.Instance.BarrelCurrency;
        Experience = PlayerEntity.Instance.Experience;
        RankID = PlayerEntity.Instance.RankID;
        MainWeapon = new WeaponData(PlayerEntity.Instance.FirstWeaponData);
        HeavyWeapon = new WeaponData(PlayerEntity.Instance.SecondWeaponData);
        BattleModule = new BattleModuleData(PlayerEntity.Instance.BattleModuleData);
        Head = new ArmorData(PlayerEntity.Instance.HelmetData);
        Body = new ArmorData(PlayerEntity.Instance.BodyData);
        Foot = new ArmorData(PlayerEntity.Instance.FootData);
        Hand = new ArmorData(PlayerEntity.Instance.HandData);
        // TODO: Add data update logic
    }

    public void Dispose()
    {
        // TODO: Remove data to default values, invokes where Clear Data
    }
}
