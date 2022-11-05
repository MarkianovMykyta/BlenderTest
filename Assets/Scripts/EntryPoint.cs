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
    [SerializeField] private OrdersProvider _ordersProvider;
    [SerializeField] private Blender.Blender _blender;
    [SerializeField] private IngredientsManager _ingredientsManager;


    private void Start()
    {
        var gameContext = CreateGameContext();

        StartGame(gameContext);
    }

    
    //Main game cycle
    private async void StartGame(GameContext gameContext)
    {
        await gameContext.PopupService.OpenPopup<PopupResult>(FtuePopupData.Default);

        var ordersManager = new OrdersManager(gameContext);
        
        while (true)
        {
            var orderResult = await ordersManager.StartOrder();

            var orderCompletePopupData = new OrderCompletePopupData()
            {
                ResultRate = orderResult.SuccessRate,
                Success = orderResult.Success
            };
            var orderCompletePopupResult = await gameContext.PopupService.OpenPopup<OrderCompletePopupResult>(orderCompletePopupData);

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

        var gameContext = new GameContext(new GameState(), popupsService, _ordersProvider, _blender, _ingredientsManager);

        return gameContext;
    }
}