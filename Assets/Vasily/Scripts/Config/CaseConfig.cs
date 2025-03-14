using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CaseConfig", menuName = "Config/CaseConfig")]
public class CaseConfig : Config
{
    public List<CaseBase> cases;
    private Dictionary<string, CaseBase> _dictionary = new Dictionary<string, CaseBase>();

    public override IEnumerator Init()
    {
        _dictionary.Clear();

        for (int i = 0; i < cases.Count; i++)
        {
            if (!_dictionary.ContainsKey(cases[i].KEY_ID))
            {
                _dictionary.Add(cases[i].KEY_ID, cases[i]);
            }
            else
            {
                Debug.LogWarning($"Duplicate key detected: {cases[i].KEY_ID}");
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    public CaseBase GetByID(string key)
    {
        if (_dictionary.TryGetValue(key, out CaseBase caseBase))
        {
            return caseBase;
        }

        Debug.LogError($"Case with key '{key}' not found!");
        return null;
    }

    // Метод для ручного обновления словаря
    public void InitDictionary()
    {
        _dictionary.Clear();

        for (int i = 0; i < cases.Count; i++)
        {
            if (!_dictionary.ContainsKey(cases[i].KEY_ID))
            {
                _dictionary.Add(cases[i].KEY_ID, cases[i]);
            }
        }
    }
}
