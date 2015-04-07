#if UNITY_EDITOR
using System;
using toxicFork.GUIHelpers.Disposable;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableEditorGUILayout {

	public class ScrollView : IDisposable {
		private readonly ScrollState scrollState = StateObject.Get<ScrollState>();

		public ScrollView() {
			scrollState.position = EditorGUILayout.BeginScrollView(scrollState.position);
		}

		public ScrollView(GUIStyle style) {
			scrollState.position = EditorGUILayout.BeginScrollView(scrollState.position, style);
		}

		public void Dispose() {
			EditorGUILayout.EndScrollView();
		}
	}
}
#endif