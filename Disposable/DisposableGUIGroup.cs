using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUIGroup : IDisposable {
		public DisposableGUIGroup(Rect position) {
			GUI.BeginGroup(position);
		}

		public void Dispose() {
			GUI.EndGroup();
		}
	}
}