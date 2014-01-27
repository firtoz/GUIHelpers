using System;
using toxicFork.GUIHelpers.Disposable;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUILayout {
	public class GUILayoutScrollView : IDisposable {
		private readonly ScrollState scrollState = StateObject.Get<ScrollState>();

		public GUILayoutScrollView() {
			scrollState.position = GUILayout.BeginScrollView(scrollState.position);
		}

		public GUILayoutScrollView(GUIStyle style) {
			scrollState.position = GUILayout.BeginScrollView(scrollState.position, style);
		}

		public void Dispose() {
			GUILayout.EndScrollView();
		}
	}
}