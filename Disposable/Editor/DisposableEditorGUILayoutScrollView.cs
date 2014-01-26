using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableEditorGUILayoutScrollView : IDisposable {
		private readonly ScrollState scrollState = StateObject.Get<ScrollState>();

		public DisposableEditorGUILayoutScrollView() {
			scrollState.position = EditorGUILayout.BeginScrollView(scrollState.position);
		}

		public DisposableEditorGUILayoutScrollView(GUIStyle style) {
			scrollState.position = EditorGUILayout.BeginScrollView(scrollState.position, style);
		}

		public void Dispose() {
			EditorGUILayout.EndScrollView();
		}
	}
}