using UnityEngine;
using UnityEngine.UI;

public class StatSlotView : MonoBehaviour
{
    [SerializeField] Text _title;
    [SerializeField] Text _value;
    [SerializeField] Image _icon;

    public void UpdateView(float fill, string value, string title)
    {
        gameObject.SetActive(true);
        _icon.fillAmount = fill;
        _value.text = value;
        _title.text = title;
    } 

    private void OnValidate()
    {
        _icon = transform.GetChild(0).GetComponent<Image>();
        _title = transform.GetChild(1).GetComponent<Text>();
        _value = transform.GetChild(2).GetComponent<Text>();
    }
}
