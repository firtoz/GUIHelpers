using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableMaterialDrawer : DisposableDrawerBase {
		private readonly Material material;
		private readonly Quaternion rotation;
		private readonly float scale;

		public DisposableMaterialDrawer(Material material, Quaternion rotation = default(Quaternion), float scale = 1f) {
			this.rotation = rotation;
			this.material = material;
			this.scale = scale;
		}

		public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
			DrawSquare(position, this.rotation*rotation, scale*size, material);
		}

		public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
			DrawSquare(position, rotation, size);
		}
	}
}