using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers {
    public class HelperPopupWindow : EditorWindow {
        private Action<Action> onGUI;
        private Action<Action, bool> onGUIFocus;

        private readonly Action closeAction;

        public HelperPopupWindow() {
            closeAction = () => { Close(); };
        }

        public void OnGUI()
        {
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
            this.onGUI = onGUI;
            ShowAsDropDown(windowRect, windowRect.size);
        }

        public void ShowAsDropDown(Action<Action, bool> onGUIFocus, Rect windowRect)
        {
            this.onGUIFocus = onGUIFocus;
            ShowAsDropDown(windowRect, windowRect.size);
        }
    }
}