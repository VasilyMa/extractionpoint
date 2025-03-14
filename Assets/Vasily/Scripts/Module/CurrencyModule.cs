using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyModule
{
    // ��������� ������� ��� �������� ������� �������� ������ ������
    private static Dictionary<string, int> currencies = new Dictionary<string, int>();

    // ����� ��� ����������� ������ (����� ������������ ��� �������������)
   /* public static void RegisterCurrency(string currencyName, int initialAmount)
    {
        // ������������� ������ � ��������� ���������
        currencies[currencyName] = initialAmount;
    }

    // ����� ��� ���������� ������
    public static void AddCurrency(string currencyName, int amount)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AddCurrency",
            FunctionParameter = new { currencyName = currencyName, amount = amount },
            GeneratePlayStreamEvent = true,
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            dynamic resultData = result.FunctionResult;
            int newBalance = resultData.newBalance;
            Debug.Log($"Successfully added {amount} {currencyName}. New balance: {newBalance}.");
        }, error =>
        {
            Debug.LogError($"Error adding currency: {error.GenerateErrorReport()}");
        });
    }


    // ����� ��� ���������� ������
    public static void SpendCurrency(string currencyName, int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = currencyName,
            Amount = amount
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request, result =>
        {
            Debug.Log($"Successfully subtracted {amount} {currencyName} from the player.");
            GetCurrencyBalance(currencyName); // ���������� ���������� ������� ����� ��������
        }, error =>
        {
            Debug.LogError($"Error subtracting currency: {error.GenerateErrorReport()}");
        });
    }

    // ����� ��� ��������� �������� ������� ������
    public static void GetCurrencyBalance(string currencyName)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            if (result.VirtualCurrency.ContainsKey(currencyName))
            {
                currencies[currencyName] = result.VirtualCurrency[currencyName];
                Debug.Log($"{currencyName} balance is {currencies[currencyName]}");
            }
            else
            {
                Debug.Log($"{currencyName} balance is 0");
            }
        }, error =>
        {
            Debug.LogError($"Error getting currency: {error.GenerateErrorReport()}");
        });
    }

    // ����� ��� ��������� ���� �����
    public static void GetAllCurrencyBalances()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            foreach (var currency in result.VirtualCurrency)
            {
                currencies[currency.Key] = currency.Value;
                Debug.Log($"{currency.Key} balance is {currency.Value}");
            }
        }, error =>
        {
            Debug.LogError($"Error getting all currencies: {error.GenerateErrorReport()}");
        });
    }*/
}
