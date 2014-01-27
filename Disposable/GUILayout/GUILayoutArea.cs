using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUILayout {
	public class GUILayoutArea : IDisposable {
		public GUILayoutArea(Rect position) {
			GUILayout.BeginArea(position);
		}

		public void Dispose() {
			GUILayout.EndArea();
		}
	}
}