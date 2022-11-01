using Popups;
using UnityEngine;
using UnityEngine.UI;

namespace Ftue
{
    public class FtuePopup : PopupBase<FtuePopupData, PopupResult>
    {
        [SerializeField] private Button _playButton;

        protected override void OnPopupOpened()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
        }

        protected override void OnPopupClosed()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            PopupTask.TrySetResult(null);

            Close();
        }
    }
}