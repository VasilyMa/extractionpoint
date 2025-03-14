using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrateViewObject : MonoBehaviour
{
    [SerializeField] Image _timerFill;
    [SerializeField] Text _timerTitle;
    
    public IEnumerator Timer(InteractiveViewData viewData)
    {
        Camera camera = Camera.main;

        _timerFill.fillAmount = 1f;
        _timerTitle.text = viewData.RemainingTime.ToString("F1");

        float remainingTime = viewData.RemainingTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            _timerTitle.text = remainingTime.ToString("F1");
            _timerFill.fillAmount = remainingTime / viewData.RemainingTime;

            transform.position = camera.WorldToScreenPoint(viewData.Target.position + new Vector3(0, 1.5f, 0));
            yield return null;
        }
    }
}
