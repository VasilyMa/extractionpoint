using UnityEngine;

public class FootPrintManager : MonoBehaviour
{
    public static FootPrintManager Instance;
    private GameConfig _gameConfig;
    private void Start()
    {
        Instance = this;

        _gameConfig = ConfigModule. GetConfig<GameConfig>();
    }
    public void Print(Transform printPos, Vector3 kudaSmotrec)
    {
        //to do pool print foot
        //var footPrint = PoolModule.Instance.GetFromPool<>();
        GameObject footPrint = GameObject.Instantiate(_gameConfig.FootPrint, printPos.position, Quaternion.identity);
        footPrint.transform.LookAt(kudaSmotrec, Vector3.up);
        footPrint.transform.rotation = Quaternion.Euler(90, footPrint.transform.rotation.y, footPrint.transform.rotation.z);
        //footPrint.SetPositionAndRotation(printPos, targetRotation);
        //footPrint.gameObject.SetActive(true);
    }
}
