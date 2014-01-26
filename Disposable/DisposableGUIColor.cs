using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUIColor : IDisposable {
		private readonly Color previousColor;

		public DisposableGUIColor(Color color) {
			previousColor = GUI.color;
			GUI.color = color;
		}

		public void Dispose() {
			GUI.color = previousColor;
		}
	}
}