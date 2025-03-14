using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RankConfig", menuName = "Config/RankConfig")]
public class RankConfig : Config
{
    public List<RankBase> ranks;
    private Dictionary<string, RankBase> _dictionary = new Dictionary<string, RankBase>();

    public override IEnumerator Init()
    {
        _dictionary.Clear();

        for (int i = 0; i < ranks.Count; i++)
        {
            if (!_dictionary.ContainsKey(ranks[i].KEY_ID))
            {
                _dictionary.Add(ranks[i].KEY_ID, ranks[i]);
            }
            else
            {
                Debug.LogWarning($"Duplicate key detected: {ranks[i].KEY_ID}");
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    public RankBase GetByID(string key)
    {
        if (_dictionary.TryGetValue(key, out RankBase rank))
        {
            return rank;
        }

        Debug.LogError($"Rank with key '{key}' not found!");
        return null;
    }

    public void InitDictionary()
    {
        _dictionary.Clear();

        for (int i = 0; i < ranks.Count; i++)
        {
            if (!_dictionary.ContainsKey(ranks[i].KEY_ID))
            {
                _dictionary.Add(ranks[i].KEY_ID, ranks[i]);
            }
        }
    }
}
