using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popups
{
    public class PopupBase<TPopupData, TPopupResult> : MonoBehaviour, IPopup 
        where TPopupData : PopupData 
        where TPopupResult : PopupResult
    {
        [SerializeField] private Transform _panel;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _openDuration;
        [SerializeField] private Ease _openEase;
        [SerializeField] private float _closeDuration;
        [SerializeField] private Ease _closeEase;

        private TaskCompletionSource<TPopupResult> _popupTask;

        public async Task<PopupResult> Open(PopupData popupData)
        {
            var targetPopupData = popupData as TPopupData;
            if (targetPopupData == null)
            {
                throw new Exception(
                    $"Popup Data type wi wrong! Expected {typeof(TPopupData)} but got {popupData.GetType()}");
            }
            
            SetupData(targetPopupData);

            _panel.DOScale(Vector3.one, _openDuration).From(Vector3.zero).SetEase(_openEase);
            _canvasGroup.DOFade(1, _openDuration).From(0).SetEase(_openEase);

            await Task.Delay(TimeSpan.FromSeconds(_openDuration / Time.timeScale));
            
            OnPopupOpened();

            _popupTask = new TaskCompletionSource<TPopupResult>();
            return await _popupTask.Task;
        }
        
        public async Task Close()
        {
            _panel.DOScale(Vector3.zero, _closeDuration).SetEase(_closeEase);
            _canvasGroup.DOFade(0, _closeDuration).SetEase(_closeEase);

            await Task.Delay(TimeSpan.FromSeconds(_closeDuration / Time.timeScale));
            
            OnPopupClosed();
        }

        protected virtual void OnPopupOpened(){}
        protected virtual void OnPopupClosed(){}
        
        protected virtual void SetupData(TPopupData popupData){}
    }
}