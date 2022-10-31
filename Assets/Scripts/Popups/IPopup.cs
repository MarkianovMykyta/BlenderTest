using System.Threading.Tasks;

namespace Popups
{
    public interface IPopup
    {
        Task<PopupResult> Open(PopupData popupData);
        Task Close();
    }
}