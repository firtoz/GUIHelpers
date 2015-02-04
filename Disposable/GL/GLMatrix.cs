using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGL {
    public class GLMatrix : IDisposable {
        public GLMatrix() {
            GL.PushMatrix();
        }

        public void Dispose() {
            GL.PopMatrix();
        }
    }
}