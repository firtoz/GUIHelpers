using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableHandleColor : IDisposable {
		private readonly Color previousColor;

		public DisposableHandleColor(Color color) {
			previousColor = Handles.color;
			Handles.color = color;
		}

		public DisposableHandleColor() {
			previousColor = Handles.color;
		}

		public void Dispose() {
			Handles.color = previousColor;
		}
	}
}