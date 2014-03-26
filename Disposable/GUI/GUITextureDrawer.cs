using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
	public class GUITextureDrawer : IDisposable {

		private readonly Texture2D texture;
		private readonly Texture2D hotTexture;
		private readonly Quaternion rotation;
		private readonly float scale;

	    public bool alwaysVisible = false;

		public GUITextureDrawer(Texture2D texture, Quaternion rotation = default(Quaternion), float scale = 1f) {
			this.rotation = rotation;
			this.texture = texture;
			this.scale = scale;
		}

		public GUITextureDrawer(Texture2D texture, Texture2D hotTexture,
			Quaternion rotation = default(Quaternion), float scale = 1f) {
			this.rotation = rotation;
			this.texture = texture;
			this.hotTexture = hotTexture;
			this.scale = scale;
		}

	    public Material Material {
	        get {
                Material material = alwaysVisible ? Helpers.AlwaysVisibleGUIMaterial : Helpers.GUIMaterial;
	            return material;
	        }
	    }

	    public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
            Material material = Material;
            material.SetTexture(0, texture);
            if (hotTexture != null && material.HasProperty("_HotTex"))
            {
                material.SetTexture("_HotTex", hotTexture);
			}
            Helpers.DrawSquare(position, this.rotation * rotation, scale * size, material);
		}

		public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
			DrawSquare(position, rotation, size);
		}

	    public void Dispose() {
	    }
	}
}