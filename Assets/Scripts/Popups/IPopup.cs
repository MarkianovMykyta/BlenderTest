using System;
using System.Threading.Tasks;

namespace Popups
{
    public interface IPopup
    {
        event Action<IPopup> PopupClosed;
        
        Task<PopupResult> Open(PopupData popupData);
        void Close();
    }
}