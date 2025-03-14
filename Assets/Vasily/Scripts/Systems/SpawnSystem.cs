using Photon.Pun;

using UnityEngine;

public class SpawnSystem : SourceSystem
{
    private EnemySpawnPoint[] enemySpawnPoints;
    float tickInvoke = 10f;
    GameConfig gameConfig;
    public SpawnSystem(BattleState state) : base(state)
    {

    }

    public override void Dispose()
    {

    }

    public override void FixedRun()
    {

    }

    public override void AfterInit()
    {
        Debug.Log($"After init {this}");
    }

    public override void Run()
    {
        tickInvoke -= Time.deltaTime;

        if (tickInvoke < 0)
        {
            for (global::System.Int32 i = 0; i < enemySpawnPoints.Length; i++)
            {
                Vector3 spawnPos = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position;

                UnitSystem.Instance.InvokeUnit(spawnPos);
            }
            tickInvoke = 1f;
        }
    }

    public override void Init()
    {
        if (enemySpawnPoints == null) enemySpawnPoints = GameObject.FindObjectsOfType<EnemySpawnPoint>();

        gameConfig = ConfigModule.GetConfig<GameConfig>();

        for (int i = 0; i < 3; i++)
        {

            var spawnPos = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position;

            object[] sendData = new object[] { spawnPos.x, spawnPos.y, spawnPos.z };

            PhotonNetwork.Instantiate($"Unit/Enemy/{gameConfig.TestPref[i].name}", Vector3.zero, Quaternion.identity, data: sendData);
        }
    }
}
