using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public abstract class DisposableDrawerBase : IDisposable {
		public virtual void Dispose() {}

		public void DrawSquare(Vector3 position, Quaternion rotation, float size, Material material) {
			GL.PushMatrix();
			for (int i = 0; i < material.passCount; i++) {
				material.SetPass(i);
				GUIHelpers.DrawSquare(position, rotation, size);
			}
			GL.PopMatrix();
		}
	}
}