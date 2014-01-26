using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.Disposable {
	internal class DisposableZeroIndent : IDisposable {
		private readonly int originalIndent;

		public DisposableZeroIndent() {
			originalIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
		}

		public void Dispose() {
			EditorGUI.indentLevel = originalIndent;
		}
	}
}