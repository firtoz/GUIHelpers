using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
    public class GUIMatrix : IDisposable {
        private readonly Matrix4x4 oldMatrix;

        public GUIMatrix() {
            oldMatrix = GUI.matrix;
        }

        public GUIMatrix(Matrix4x4 matrix, bool replace = false)
            : this() {
            GUI.matrix = replace ? matrix : GUI.matrix * matrix;
        }

        public void Dispose() {
            GUI.matrix = oldMatrix;
        }
    }
}