using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public abstract class DisposableHandleDrawerBase : IGUIDrawer, IDisposable {
		protected abstract void DoDraw(int controlID, Vector2 position, float size, float rotation);

		public void Draw(int controlID, Vector2 position, float size, float rotation) {
			if (Event.current.type == EventType.repaint) {
				DoDraw(controlID, position, size, rotation);
			}
		}

		public abstract float GetDistance(Vector2 position, float size, float rotation);

		public void Dispose() {}
	}
}