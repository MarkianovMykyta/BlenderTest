using System.Threading.Tasks;

namespace Popups
{
    public interface IPopupService
    {
        Task<TPopupResult> OpenPopup<TPopupResult>(PopupData popupData) where TPopupResult : PopupResult;
    }
}