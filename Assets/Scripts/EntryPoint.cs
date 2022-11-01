using System;
using System.Collections;
using Food;
using Ftue;
using Orders;
using Orders.Popup;
using Popups;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PopupsProvider _popupsProvider;


    private void Start()
    {
        var gameContext = CreateGameContext();

        StartGame(gameContext);
    }

    private async void StartGame(GameContext gameContext)
    {
        await gameContext.PopupService.OpenPopup<PopupResult>(FtuePopupData.Default);

        var ordersManager = new OrdersManager(gameContext);

        while (true)
        {
            var orderResult = await ordersManager.StartOrder();

            var orderCompletePopupResult = await gameContext.PopupService.OpenPopup<OrderCompletePopupResult>(new OrderCompletePopupData()
            {
                ResultRate = orderResult.SuccessRate,
                Success = orderResult.Success
            });

            if (orderCompletePopupResult.GoToTheNextOrder)
            {
                ordersManager.PrepareNextOrder();
            }
            else
            {
                ordersManager.RestartCurrentOrder();
            }
        }
    }

    private GameContext CreateGameContext()
    {
        var popupsService = new PopupService(_popupsProvider);

        var gameContext = new GameContext(new GameState(), popupsService);

        return gameContext;
    }
}