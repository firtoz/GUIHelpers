using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableGUITextureDrawer : DisposableDrawerBase {
		private static Material _guiMaterial;

		private static Material guiMaterial {
			get {
				return _guiMaterial ??
				       (_guiMaterial =
					       new Material(Shader.Find("GUI Helpers/GUI")) {hideFlags = HideFlags.HideAndDontSave});
			}
		}

		public Material Material {
			get { return guiMaterial; }
		}

		private readonly Texture2D texture;
		private readonly Texture2D hotTexture;
		private readonly Quaternion rotation;
		private readonly float scale;

		public DisposableGUITextureDrawer(Texture2D texture, Quaternion rotation = default(Quaternion), float scale = 1f) {
			this.rotation = rotation;
			this.texture = texture;
			this.scale = scale;
		}

		public DisposableGUITextureDrawer(Texture2D texture, Texture2D hotTexture,
			Quaternion rotation = default(Quaternion), float scale = 1f) {
			this.rotation = rotation;
			this.texture = texture;
			this.hotTexture = hotTexture;
			this.scale = scale;
		}

		public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
			guiMaterial.SetTexture(0, texture);
			if (hotTexture != null && guiMaterial.HasProperty("_HotTex")) {
				guiMaterial.SetTexture("_HotTex", hotTexture);
			}
			DrawSquare(position, this.rotation*rotation, scale*size, guiMaterial);
		}

		public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
			DrawSquare(position, rotation, size);
		}
	}
}