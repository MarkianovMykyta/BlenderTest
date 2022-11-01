using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popups
{
    public class PopupService : IPopupService
    {
        private const int DELAY_BETWEEN_QUEUE_CHECK_MS = 100;
        
        private Queue<PopupData> _popupsToOpenQueue;
        private PopupsProvider _popupsProvider;
        private IPopup _activePopup;

        private PopupData _nextPopupToOpen;

        public PopupService(PopupsProvider popupsProvider)
        {
            _popupsProvider = popupsProvider;
            
            _popupsToOpenQueue = new Queue<PopupData>();
        }

        public async Task<TPopupResult> OpenPopup<TPopupResult>(PopupData popupData) where TPopupResult : PopupResult
        {
            if (_activePopup != null)
            {
                AddPopupToQueue(popupData);
                
                while (_nextPopupToOpen != popupData)
                {
                    await Task.Delay(DELAY_BETWEEN_QUEUE_CHECK_MS);
                }
            }

            var popup = _popupsProvider.GetPopup(popupData);

            _activePopup = popup;
            popup.PopupClosed += OnPopupClosed;
            
            var result = await popup.Open(popupData);

            return result as TPopupResult;
        }

        private void AddPopupToQueue(PopupData popupData)
        {
            _popupsToOpenQueue.Enqueue(popupData);
        }
        
        private void OnPopupClosed(IPopup popup)
        {
            popup.PopupClosed -= OnPopupClosed;
            
            _activePopup = null;
            
            if (_popupsToOpenQueue.Count > 0)
            {
                _nextPopupToOpen = _popupsToOpenQueue.Dequeue();
            }
        }
    }
}