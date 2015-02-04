#if UNITY_EDITOR
using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.DisposableEditorGUI
{
	public class MixedValue : IDisposable {
		private readonly bool oldMixedValue;

		public MixedValue(bool mixedValue) {
			oldMixedValue = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = mixedValue;
		}

		public void Dispose() {
			EditorGUI.showMixedValue = oldMixedValue;
		}
	}
}
#endif