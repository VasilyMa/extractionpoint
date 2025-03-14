using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BattlePanel : SourcePanel
{
    public FloatingJoystick JoystickMove;
    public FloatingJoystick JoystickLook;
    [HideInInspector] public PhotonPlayer PhotonPlayer;
    protected PlayerHealthbar playerHealthbar;
    protected BattleButtonsLayout buttonsLayout;

    private List<PlayerInGameBar> _inGameBars = new List<PlayerInGameBar>();

    public override void Init(SourceCanvas canvasParent)
    {
        buttonsLayout = GetComponentInChildren<BattleButtonsLayout>().Init(this);

        var bars = GetComponentsInChildren<PlayerInGameBar>();

        for (int i = 0; i < bars.Length; i++)
        {
            _inGameBars.Add(bars[i]);
            bars[i].gameObject.SetActive(false);
        }

        base.Init(canvasParent);

        gameObject.SetActive(true);
    }

    public override void OnOpen(params Action[] onComplete)
    {
        base.OnOpen(onComplete);

        playerHealthbar = GetComponentInChildren<PlayerHealthbar>().Init(PhotonPlayer);

        buttonsLayout.InitView(PhotonPlayer.EquipmentContainer);
    }

    public void RegisterPlayerHealthbar(PhotonPlayer player)
    {
        var freeBar = _inGameBars.Find(x => !x.IsUsed);

        if (freeBar != null)
        {
            freeBar.Init(player);
            return;
        }

        // Загружаем все префабы из папки Resources
        var allUIManager = Resources.LoadAll<PlayerInGameBar>("UI");

        if (allUIManager.Length == 0)
        {
            Debug.LogError("Не найден ни один UI manager в папке Resources.");
            return;
        }

        // Выбираем первый найденный (или можно искать по условиям)
        var newPlayerBar = allUIManager.FirstOrDefault();

        if (newPlayerBar != null)
        {
            var instance = UnityEngine.Object.Instantiate(newPlayerBar);
            instance.Init(player);
        }
    }

    public override void OnDipose()
    {
        buttonsLayout.Dispose();
    }
}
