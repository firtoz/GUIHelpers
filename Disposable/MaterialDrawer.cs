using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class MaterialDrawer : IDisposable {
		private readonly Material material;
		private readonly Quaternion rotation;
		private readonly float scale;

		public MaterialDrawer(Material material, Quaternion rotation = default(Quaternion), float scale = 1f) {
			this.rotation = rotation;
			this.material = material;
			this.scale = scale;
		}

		public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
			Helpers.DrawSquare(position, this.rotation*rotation, scale*size, material);
		}

		public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
			DrawSquare(position, rotation, size);
		}

	    public void Dispose() {
	    }
	}
}