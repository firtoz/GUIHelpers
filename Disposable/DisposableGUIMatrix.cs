using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUIMatrix : IDisposable {
		private readonly Matrix4x4 oldMatrix;

		public DisposableGUIMatrix() {
			oldMatrix = GUI.matrix;
		}

		public DisposableGUIMatrix(Matrix4x4 matrix, bool replace = false)
			: this() {
			GUI.matrix = replace ? matrix : GUI.matrix*matrix;
		}

		public void Dispose() {
			GUI.matrix = oldMatrix;
		}
	}
}