using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableEditorGUIMixedValue : IDisposable {
		private readonly bool oldMixedValue;

		public DisposableEditorGUIMixedValue(bool mixedValue) {
			oldMixedValue = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = mixedValue;
		}

		public void Dispose() {
			EditorGUI.showMixedValue = oldMixedValue;
		}
	}
}