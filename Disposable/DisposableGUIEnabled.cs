using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUIEnabled : IDisposable {
		private readonly bool guiEnabled;

		public DisposableGUIEnabled(bool enabled) {
			guiEnabled = GUI.enabled;
			GUI.enabled = enabled;
		}

		public void Dispose() {
			GUI.enabled = guiEnabled;
		}
	}
}