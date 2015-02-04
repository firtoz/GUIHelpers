#if UNITY_EDITOR
using UnityEditor;
using System;

namespace toxicFork.GUIHelpers.DisposableEditorGUI
{
	public class Indent : IDisposable {
		private readonly int offset;

		public Indent(int i = 1) {
			offset = i;
			EditorGUI.indentLevel += offset;
		}

		public void Dispose() {
			EditorGUI.indentLevel -= offset;
		}
	}
}
#endif