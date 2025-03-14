using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game")]
public class GameConfig : Config
{
    public GameObject FootPrint;
    public PhotonEnemy[] TestPref;
    public override IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
