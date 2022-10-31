using System;
using System.Collections;
using Food;
using Orders;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private OrdersManager _ordersManager;
    
    private void Start()
    {
        var gameContext = new GameContext(new GameState());

        _ordersManager = new OrdersManager(gameContext);
    }

    private async void StartGame()
    {
         await PopupService.OpenPopup(FtuePopupData.Default);

         while (true)
         {
             var orderResult = await _ordersManager.StartOrder();

             var orderCompletePopupResult = await PopupService.OpenPopupWithResult(new OrderCompletePopupData(orderResult));

             if (orderCompletePopupResult.Success)
             {
                 _ordersManager.PrepareNextOrder();
             }
             else
             {
                 _ordersManager.RestartCurrentOrder();
             }
         }
    }
}