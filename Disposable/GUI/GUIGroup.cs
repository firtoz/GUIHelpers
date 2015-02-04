using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
    public class GUIGroup : IDisposable {
        public GUIGroup(Rect position) {
            GUI.BeginGroup(position);
        }

        public void Dispose() {
            GUI.EndGroup();
        }
    }
}