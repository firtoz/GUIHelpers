using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers {
    public class HelperPopupWindow : EditorWindow {
        private Action<Action> onGUI;
        private Action<Action, bool> onGUIFocus;

        private readonly Action closeAction;
        private bool started;
        private bool closing;

        public HelperPopupWindow() {
            closeAction = () => {
                CleanClose();
            };
        }

        public void OnGUI()
        {
            if (closing) {
                return;
            }
            if (!started || EditorApplication.isCompiling) {
                CleanClose();
                return;
            }

            if (onGUIFocus != null && closeAction != null)
            {
                onGUIFocus(closeAction, focusedWindow == this);
            } else if (onGUI != null && closeAction != null) {
                onGUI(closeAction);
            }
            else {
                CleanClose();
            }
        }

        private void CleanClose() { 
            if (!closing) {
                closing = true;
                EditorApplication.delayCall += () => {
                    if (!disabled) {
                        Close();
                    }
                };
            }
        }

        [SerializeField]
        private bool disabled = true;

        public void OnEnable() {
            disabled = false;
        }

        public void OnDisable() {
            disabled = true;
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
            titleContent = new GUIContent(title);
            ShowUtility();
        }

        public void ShowUtility(String title, Action<Action> onGUI)
        {
            started = true;
            this.onGUI = onGUI;
            titleContent = new GUIContent(title);
            ShowUtility();
        }
    }
}