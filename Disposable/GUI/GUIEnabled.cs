using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
    public class GUIEnabled : IDisposable {
        private readonly bool guiEnabled;

        public GUIEnabled(bool enabled) {
            guiEnabled = GUI.enabled;
            GUI.enabled = enabled;
        }

        public void Dispose() {
            GUI.enabled = guiEnabled;
        }
    }
}