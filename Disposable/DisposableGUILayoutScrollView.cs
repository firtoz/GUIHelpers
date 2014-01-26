using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUILayoutScrollView : IDisposable {
		private readonly ScrollState scrollState = StateObject.Get<ScrollState>();

		public DisposableGUILayoutScrollView() {
			scrollState.position = GUILayout.BeginScrollView(scrollState.position);
		}

		public DisposableGUILayoutScrollView(GUIStyle style) {
			scrollState.position = GUILayout.BeginScrollView(scrollState.position, style);
		}

		public void Dispose() {
			GUILayout.EndScrollView();
		}
	}
}