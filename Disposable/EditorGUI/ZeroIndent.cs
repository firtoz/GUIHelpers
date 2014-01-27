#if UNITY_EDITOR
using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.DisposableEditorGUI
{
	internal class ZeroIndent : IDisposable {
		private readonly int originalIndent;

		public ZeroIndent() {
			originalIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
		}

		public void Dispose() {
			EditorGUI.indentLevel = originalIndent;
		}
	}
}
#endif