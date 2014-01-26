using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableEditorGUILayoutHorizontal : IDisposable {
		public DisposableEditorGUILayoutHorizontal() {
			EditorGUILayout.BeginHorizontal();
		}

		public void Dispose() {
			EditorGUILayout.EndHorizontal();
		}
	}
}