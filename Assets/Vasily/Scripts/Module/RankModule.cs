using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

public static class RankModule
{
    // ������ ������� ����� ��� ��������� ������
    private static List<int> experienceThresholds = new List<int> { 1000, 2000, 4000 }; // ������������� ������ �����
    private static List<string> rankNames = new List<string> { "�������", "��������", "������������", "������" }; // �������� ������

    // ����� ��� ���������� ����� ������
    public static void AddExperience(int experienceToAdd)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            // �������� ������� ���������� ������
            int experiencePoints = 0;
            foreach (var statistic in result.Statistics)
            {
                if (statistic.StatisticName == "ExperiencePoints")
                {
                    experiencePoints = statistic.Value;
                }
            }

            // ��������� ���������� �����
            experiencePoints += experienceToAdd;

            // ������������� ����� ���� � ��������� ����
            UpdatePlayerRank(experiencePoints);
        }, error =>
        {
            Debug.LogError($"Error getting player statistics: {error.GenerateErrorReport()}");
        });
    }

    // ����� ��� ���������� ����� ������ �� ������ �������� �����
    private static void UpdatePlayerRank(int experience)
    {
        int currentRank = GetCurrentRank(experience);

        // ���������� ����������� ����� � �����
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "ExperiencePoints", Value = experience },
                new StatisticUpdate { StatisticName = "PlayerRank", Value = currentRank + 1 } // ���� ���������� � 1
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
        {
            Debug.Log($"Player has achieved rank {currentRank + 1} with {experience} experience points.");
            ReportExperienceStatus(currentRank, experience);
        }, error =>
        {
            Debug.LogError($"Error updating player rank: {error.GenerateErrorReport()}");
        });
    }

    // ����� ��� ����������� �������� ����� �� ������ �����
    private static int GetCurrentRank(int experience)
    {
        for (int i = 0; i < experienceThresholds.Count; i++)
        {
            if (experience < experienceThresholds[i])
            {
                return i; // ���� ����� ������� ������
            }
        }
        return experienceThresholds.Count; // ���������� ��������� ����, ���� ���� ��������� ��� ������
    }

    // ����� ��� ����������� ���������� � ������� ����� � ����������� ����� ��� ����������
    private static void ReportExperienceStatus(int currentRank, int experience)
    {
        string currentRankName = currentRank < rankNames.Count ? rankNames[currentRank] : "������������ ����";

        if (currentRank < experienceThresholds.Count)
        {
            int thresholdForNextRank = experienceThresholds[currentRank];
            int experienceNeeded = thresholdForNextRank - experience;

            if (experienceNeeded > 0)

            {
                Debug.Log($"{currentRankName}: ��� ��� ������� ����. ��� ����� {experienceNeeded} ����� ��� ���������� ���������� ����� '{rankNames[currentRank + 1]}'.");
            }
            else
            {
                Debug.Log($"����������! �� �������� ����� '{rankNames[currentRank + 1]}'.");
            }
        }
        else
        {
            Debug.Log($"{currentRankName}: ��� ��� ������� ����. �� �������� ������������� �����, ���������� ���� �� ���������.");
        }
    }
}
