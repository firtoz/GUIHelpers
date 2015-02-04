using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.DisposableHandles {
	public class HandleGUI : IDisposable {
		private static int _handleCount;

		public HandleGUI() {
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
