using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popups
{
    public class PopupService
    {
        private Queue<PopupData> _popupsToOpenQueue;
        private PopupsProvider _popupsProvider;
        private IPopup _activePopup;

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
            }
            
            await Task.
        }

        private void AddPopupToQueue(PopupData popupData)
        {
            _popupsToOpenQueue.Enqueue(popupData);
        }

        public Task GetOpenPopupTask(PopupData popupData)
        {
            var popup = _popupsProvider.GetPopup(popupData);

            return popup.Open(popupData);
        }
    }
}