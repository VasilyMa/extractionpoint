using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RankConfigEditor : EditorWindow
{
    private string[] rankNames = new string[]
    {
        // ������� ����������� ������ (47�37)
        "Recruit", "Private", "Private First Class", "Lance Corporal", "Corporal", "Senior Corporal",
        "Specialist", "Sergeant", "Staff Sergeant", "Master Sergeant", "First Sergeant",

        // ������� ���������� ������ (36�16)
        "Second Lieutenant", "First Lieutenant", "Captain", "Major", "Lieutenant Colonel", "Colonel",
        "Senior Colonel", "Commander", "Chief Commander", "Field Commander", "Deputy Chief",
        "Chief of Operations", "Tactical Officer", "Strategic Officer", "Operations Director",
        "Chief Tactician", "Battle Director", "War Strategist", "Senior War Strategist",
        "Chief of Staff", "Supreme Commander",

        // ������� ������ ������ ����������� (15�0)
        "Guard of the Immortals", "Watcher of the Immortals", "Warden of the Immortals",
        "Shadow Commander", "Phantom Commander", "Ghost Operative", "Revenant of the Immortals",
        "Deathbringer of the Immortals", "Black Guard", "Silent Reaper", "Harbinger of the Immortals",
        "Dreadlord of the Immortals", "Immortal Warlord", "Supreme Commander of the Immortals",
        "Immortal Overlord", "Dark Monarch"
    };

    private int baseExp = 100;
    private RankConfig rankConfig; // ������ �� RankConfig

    [MenuItem("Tools/Rank Config Generator")]
    public static void ShowWindow()
    {
        GetWindow<RankConfigEditor>("Rank Config Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Rank Config Settings", EditorStyles.boldLabel);

        baseExp = EditorGUILayout.IntField("Base Exp", baseExp);
        rankConfig = (RankConfig)EditorGUILayout.ObjectField("Rank Config", rankConfig, typeof(RankConfig), false);

        if (GUILayout.Button("Generate Ranks"))
        {
            if (rankConfig != null)
            {
                GenerateRanks();
            }
            else
            {
                Debug.LogError("Rank Config is not assigned!");
            }
        }
    }

    private void GenerateRanks()
    {
        string folderPath = "Assets/Configs/Ranks";
        string iconPath = "Assets/Import/RanksIcons"; // ���� � ����� � ��������

        // �������� �����, ���� � ���
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh(); // ��������� ���� �������
        }

        RankBase[] ranks = new RankBase[rankNames.Length];
        List<RankBase> rankList = new List<RankBase>();

        for (int i = 0; i < rankNames.Length; i++)
        {
            RankBase rank = ScriptableObject.CreateInstance<RankBase>();
            rank.name = rankNames[i];

            // ? ������������ ������ ��� ����� exp
            int neededExp = Mathf.RoundToInt(baseExp * Mathf.Pow(i + 1, 2) / 100) * 100;

            // ? ��������� ������ �������� ����� AssetDatabase
            string iconFilePath = $"{iconPath}/������ {rankNames.Length - 1 - i}.png";
            Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconFilePath);

            if (icon == null)
            {
                Debug.LogError($"Icon not found at path: {iconFilePath}");
            }

            // ��������� ������ � ������� ����������
            Rank tempRank = new Rank()
            {
                Rank_Name = rankNames[i],
                Rank_Icon = icon, // ��������� ������
                NeededExp = neededExp,
                CurrentExp = 0,
                IsComplete = false,
                IsRewarded = false
            };

            rank.SourceRank = tempRank;
            tempRank.KEY_ID = rank.name;

            ranks[i] = rank;
            rankList.Add(rank);

            // �������� ������
            AssetDatabase.CreateAsset(rank, $"{folderPath}/{rankNames[i]}.asset");
        }

        // ? ��������� ����� ����� �����
        for (int i = 0; i < ranks.Length; i++)
        {
            if (i > 0)
                ranks[i].SourceRank.PreviousRank = ranks[i - 1];
            if (i < ranks.Length - 1)
                ranks[i].SourceRank.NextRank = ranks[i + 1];

            EditorUtility.SetDirty(ranks[i]);
        }

        // ? ��������� RankConfig
        rankConfig.ranks = rankList;

        // ? ������� ������� � ��������� ��� �� ������
        rankConfig.InitDictionary();

        AssetDatabase.SaveAssets();
        Debug.Log("Ranks generated successfully and assigned to RankConfig!");
    }
}
