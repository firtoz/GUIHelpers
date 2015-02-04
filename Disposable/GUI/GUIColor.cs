using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
    public class GUIColor : IDisposable {
        private readonly Color previousColor;

        public GUIColor(Color color) {
            previousColor = GUI.color;
            GUI.color = color;
        }

        public void Dispose() {
            GUI.color = previousColor;
        }
    }
}