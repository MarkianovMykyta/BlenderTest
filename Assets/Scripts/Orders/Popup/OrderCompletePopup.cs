using System;
using DG.Tweening;
using Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Orders.Popup
{
    public class OrderCompletePopup : PopupBase<OrderCompletePopupData, OrderCompletePopupResult>
    {
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _rateText;
        [Space]
        [SerializeField] private Slider _restultSlider;
        [Space]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextButton;
        [Space]
        [SerializeField] private string _winMessage;
        [SerializeField] private string _looseMessage;
        
        protected override void OnPopupOpened()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _nextButton.onClick.AddListener(OnNextClicked);

            ShowResultAnimation();
        }

        protected override void OnPopupClosed()
        {
            _restartButton.onClick.RemoveListener(OnRestartClicked);
            _nextButton.onClick.RemoveListener(OnNextClicked);
        }

        protected override void SetupData(OrderCompletePopupData popupData)
        {
            _restultSlider.value = 0;
            _rateText.text = "0%";
            _resultText.text = string.Empty;
            
            _restartButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(false);
        }

        private void ShowResultAnimation()
        {
            var resultRate = Data.ResultRate;
            
            _restultSlider.DOValue(resultRate, 1f).From(0);
            var rateValue = 0f;

            var duration = resultRate;
            
            DOTween
                .To(() => rateValue, x => rateValue = x, resultRate, duration)
                .OnUpdate(() => _rateText.text = $"{(rateValue * 100f):0}%")
                .OnComplete(OnResultAnimationCompleted);
        }

        private void OnResultAnimationCompleted()
        {
            var success = Data.Success;
            _restartButton.gameObject.SetActive(!success);
            _nextButton.gameObject.SetActive(success);

            _resultText.text = success ? _winMessage : _looseMessage;
        }

        private void OnRestartClicked()
        {
            PopupTask.TrySetResult(new OrderCompletePopupResult()
            {
                GoToTheNextOrder = false
            });
            
            Close();
        }
        
        private void OnNextClicked()
        {
            PopupTask.TrySetResult(new OrderCompletePopupResult()
            {
                GoToTheNextOrder = true
            });
            
            Close();
        }
    }
}