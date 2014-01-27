#if UNITY_EDITOR
using UnityEditor;
using System;

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