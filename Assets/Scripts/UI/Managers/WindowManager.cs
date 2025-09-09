using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame.UI
{
    /// <summary>
    /// Opens and closes windows according to their IDs.
    /// You can pass parameters to windows
    /// </summary>
    public class WindowManager : MonoBehaviour, IWindowManager
    {
        [SerializeField] private List<AbstractWindow> windows;

        private void Awake()
        {
            foreach (var window in windows)
            {
                window.HandleCloseClick();
            }
        }

        /// <summary>
        /// Open a window and pass parameters to it
        /// </summary>
        public void OpenWindow(string windowId, object windowParam = default)
        {
            var window = GetWindow(windowId);
            if (window != null)
            {
                window.Open();
                if (windowParam != default)
                    window.SetParam(windowParam);
            }
        }

        public void CloseWindow(string windowId)
        {
            var window = GetWindow(windowId);
            if (window != null)
                window.HandleCloseClick();
        }

        private AbstractWindow GetWindow(string windowId)
        {
            var window = windows.FirstOrDefault(w => string.Equals(w.gameObject.name, windowId, StringComparison.CurrentCultureIgnoreCase));
            if (window != default)
                return window;
            Debug.LogError($"Window with id {windowId} not found.");
            return null;

        }
    }
}
