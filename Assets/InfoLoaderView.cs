using UnityEngine;
using UnityEngine.UI;

public class InfoLoaderView : MonoBehaviour
{
    [SerializeField] Image _fillIn;
    [SerializeField] Image _fillOut;
    [SerializeField] Text _timeTitle;
    [SerializeField] Text _title;

    bool isIn = true;

    public void UpdateFillView(float value, bool isSwitch = false)
    {
        if (isSwitch)
        {
            if (isIn)
            {
                _fillIn.enabled = false;
                _fillOut.enabled = true;
                isIn = false;
            }
            else
            {
                isIn = true;
                _fillIn.enabled = true;
                _fillOut.enabled = false;
            }
        }

        if (isIn)
        {
            _fillIn.fillAmount = value;
        }
        else
        {
            _fillOut.fillAmount = 1 - value;
        }
    }

    public void UpdateTimerView(int value)
    {
        _timeTitle.text = value.ToString(); 
    }

    public void UpdateInfo(string info)
    {
        _title.text = info;
    }
}
