using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableHandles {
	public class HandleColor : IDisposable {
		private readonly Color previousColor;

		public HandleColor(Color color) {
			previousColor = Handles.color;
			Handles.color = color;
		}

		public HandleColor() {
			previousColor = Handles.color;
		}

		public void Dispose() {
			Handles.color = previousColor;
		}
	}
}
