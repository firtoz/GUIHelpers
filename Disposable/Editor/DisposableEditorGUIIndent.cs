using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableEditorGUIIndent : IDisposable {
		private readonly int offset;

		public DisposableEditorGUIIndent(int i = 1) {
			offset = i;
			EditorGUI.indentLevel += offset;
		}

		public void Dispose() {
			EditorGUI.indentLevel -= offset;
		}
	}
}