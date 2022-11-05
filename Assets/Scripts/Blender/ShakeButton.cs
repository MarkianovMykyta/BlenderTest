using System;
using UnityEngine;

namespace Blender
{
    public class ShakeButton : MonoBehaviour
    {
        public event Action ShakeClicked;
        
        private void OnMouseDown()
        {
            ShakeClicked?.Invoke();
        }
    }
}