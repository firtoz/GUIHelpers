using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
    public class GUITextureDrawer : IDisposable {
        private readonly Texture2D texture;
        private readonly Texture2D hotTexture;
        private readonly Quaternion rotation;
        private readonly float scale;
        private readonly float alpha;

        public bool alwaysVisible = false;

        public GUITextureDrawer(Texture2D texture, Quaternion rotation = default(Quaternion), float scale = 1f,
            float alpha = 1f) {
            this.rotation = rotation;
            this.texture = texture;
            this.scale = scale;
            this.alpha = alpha;
        }

        public GUITextureDrawer(Texture2D texture, Texture2D hotTexture, Quaternion rotation = default(Quaternion),
            float scale = 1f, float alpha = 1f) {
            this.rotation = rotation;
            this.texture = texture;
            this.hotTexture = hotTexture;
            this.scale = scale;
            this.alpha = alpha;
        }

        public Material Material {
            get {
                var material = alwaysVisible ? Helpers.AlwaysVisibleGUIMaterial : Helpers.GUIMaterial;
                return material;
            }
        }

        public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
            var material = Material;
            material.SetTexture(0, texture);
            if (hotTexture != null && material.HasProperty("_HotTex")) {
                material.SetTexture("_HotTex", hotTexture);
            }

            var oldColor = Helpers.color;
            Helpers.color = new Color(1, 1, 1, alpha);
            Helpers.DrawSquare(position, this.rotation * rotation, scale * size, material);
            Helpers.color = oldColor;
        }

        public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
            DrawSquare(position, rotation, size);
        }

        public void Dispose() {}
    }
}