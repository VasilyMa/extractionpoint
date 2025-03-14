using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

public static class RankModule
{
    // Список порогов опыта для различных рангов
    private static List<int> experienceThresholds = new List<int> { 1000, 2000, 4000 }; // Устанавливаем пороги опыта
    private static List<string> rankNames = new List<string> { "Новичок", "Участник", "Профессионал", "Мастер" }; // Названия рангов

    // Метод для добавления опыта игроку
    public static void AddExperience(int experienceToAdd)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            // Получаем текущие статистики игрока
            int experiencePoints = 0;
            foreach (var statistic in result.Statistics)
            {
                if (statistic.StatisticName == "ExperiencePoints")
                {
                    experiencePoints = statistic.Value;
                }
            }

            // Обновляем количество опыта
            experiencePoints += experienceToAdd;

            // Устанавливаем новый опыт и обновляем ранг
            UpdatePlayerRank(experiencePoints);
        }, error =>
        {
            Debug.LogError($"Error getting player statistics: {error.GenerateErrorReport()}");
        });
    }

    // Метод для обновления ранга игрока на основе текущего опыта
    private static void UpdatePlayerRank(int experience)
    {
        int currentRank = GetCurrentRank(experience);

        // Сохранение полученного опыта и ранга
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "ExperiencePoints", Value = experience },
                new StatisticUpdate { StatisticName = "PlayerRank", Value = currentRank + 1 } // Ранг начинается с 1
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

    // Метод для определения текущего ранга на основе опыта
    private static int GetCurrentRank(int experience)
    {
        for (int i = 0; i < experienceThresholds.Count; i++)
        {
            if (experience < experienceThresholds[i])
            {
                return i; // Ранг равен индексу порога
            }
        }
        return experienceThresholds.Count; // Возвращаем последний ранг, если опыт превышает все пороги
    }

    // Метод для отображения информации о текущем ранге и необходимом опыте для следующего
    private static void ReportExperienceStatus(int currentRank, int experience)
    {
        string currentRankName = currentRank < rankNames.Count ? rankNames[currentRank] : "Максимальный ранг";

        if (currentRank < experienceThresholds.Count)
        {
            int thresholdForNextRank = experienceThresholds[currentRank];
            int experienceNeeded = thresholdForNextRank - experience;

            if (experienceNeeded > 0)

            {
                Debug.Log($"{currentRankName}: Это ваш текущий ранг. Вам нужно {experienceNeeded} опыта для достижения следующего ранга '{rankNames[currentRank + 1]}'.");
            }
            else
            {
                Debug.Log($"Поздравляю! Вы достигли ранга '{rankNames[currentRank + 1]}'.");
            }
        }
        else
        {
            Debug.Log($"{currentRankName}: Это ваш текущий ранг. Вы достигли максимального ранга, дальнейший опыт не требуется.");
        }
    }
}
