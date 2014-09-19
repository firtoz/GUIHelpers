#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers {
    public class HelperPopupWindow : EditorWindow {
        private Action<Action> onGUI;
        private Action<Action, bool> onGUIFocus;

        private readonly Action closeAction;
        private bool started = false;
        private bool closing = false;

        public HelperPopupWindow() {
            closeAction = () => { Close(); };
        }

        public void OnGUI()
        {

            if (!started || EditorApplication.isCompiling) {
                if (!closing) {
                    closing = true;
                    Close();
                }
                return;
            }

            if (onGUIFocus != null && closeAction != null)
            {
                onGUIFocus(closeAction, focusedWindow == this);
            } else if (onGUI != null && closeAction != null) {
                onGUI(closeAction);
            }
            else {
                Close();
            }
        }

        public void ShowAsDropDown(Action<Action> onGUI, Rect windowRect) {
            started = true;
            this.onGUI = onGUI;
            ShowAsDropDown(windowRect, windowRect.size);
        }

        public void ShowAsDropDown(Action<Action, bool> onGUIFocus, Rect windowRect)
        {
            started = true;
            this.onGUIFocus = onGUIFocus;
            ShowAsDropDown(windowRect, windowRect.size);
        }

        public void ShowUtility(Action<Action, bool> onGUIFocus) {
            started = true;
            this.onGUIFocus = onGUIFocus;
            ShowUtility();
        }

        public void ShowUtility(Action<Action> onGUI)
        {
            started = true;
            this.onGUI = onGUI;
            ShowUtility();
        }

        public void ShowUtility(String title, Action<Action, bool> onGUIFocus) {
            started = true;
            this.onGUIFocus = onGUIFocus;
            this.title = title;
            ShowUtility();
        }

        public void ShowUtility(String title, Action<Action> onGUI)
        {
            started = true;
            this.onGUI = onGUI;
            this.title = title;
            ShowUtility();
        }
    }
}
#endif