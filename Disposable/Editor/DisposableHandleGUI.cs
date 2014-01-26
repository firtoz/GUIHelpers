using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableHandleGUI : IDisposable {
		private static int _handleCount;

		public DisposableHandleGUI() {
			if (_handleCount == 0) {
				Handles.BeginGUI();
			}
			_handleCount++;
		}

		public void Dispose() {
			_handleCount--;

			if (_handleCount == 0) {
				Handles.EndGUI();
			}
		}
	}
}