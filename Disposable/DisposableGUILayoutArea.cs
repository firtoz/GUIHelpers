using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUILayoutArea : IDisposable {
		public DisposableGUILayoutArea(Rect position) {
			GUILayout.BeginArea(position);
		}

		public void Dispose() {
			GUILayout.EndArea();
		}
	}
}