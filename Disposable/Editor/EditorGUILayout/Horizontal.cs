#if UNITY_EDITOR
using System;
using UnityEditor;

namespace toxicFork.GUIHelpers.DisposableEditorGUILayout
{
	public class Horizontal : IDisposable {
		public Horizontal() {
			EditorGUILayout.BeginHorizontal();
		}

		public void Dispose() {
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif