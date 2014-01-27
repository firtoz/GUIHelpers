using toxicFork.GUIHelpers.DisposableHandles;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class HandleRectangleDrawer : HandleDrawerColorBase {
		public HandleRectangleDrawer(Color baseColor)
			: base(baseColor) {}

		public HandleRectangleDrawer(Color baseColor, Color activeColor)
			: base(baseColor, activeColor) {}

		public HandleRectangleDrawer(Color baseColor, Color activeColor, Color hoverColor)
			: base(baseColor, activeColor, hoverColor) {}

		protected override void DoDraw(int controlID, Vector2 position, float size, float rotation) {
			Color color = GUIUtility.hotControl == controlID ? activeColor : baseColor;
			using (new HandleColor(color)) {
				Handles.RectangleCap(controlID, position, Helpers.Rotate2D(rotation), size);
			}
		}

		public override float GetDistance(Vector2 position, float size, float rotation) {
			return HandleUtility.DistanceToRectangle(position, Helpers.Rotate2D(rotation), size);
		}
	}
}
#endif