using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popups
{
    public class PopupsProvider : MonoBehaviour
    {
        private List<IPopup> _popups;

        private void Awake()
        {
            _popups = new List<IPopup>(GetComponentsInChildren<IPopup>(true));
        }

        public IPopup GetPopup(PopupData popupData)
        {
            var targetPopupDataType = popupData.GetType();
            
            for (var i = 0; i < _popups.Count; i++)
            {
                var popup = _popups[i];
                
                var type = popup.GetType();
                if (type.BaseType != null)
                {
                    var popupDataType = type.BaseType.GetGenericArguments()[0];

                    if (targetPopupDataType == popupDataType)
                    {
                        return popup;
                    }
                }
            }

            throw new Exception($"There is no popup with popupData of type: {targetPopupDataType}");
        }
    }
}