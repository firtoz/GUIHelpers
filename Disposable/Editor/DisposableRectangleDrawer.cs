using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableRectangleDrawer : DisposableHandleDrawerColorBase {
		public DisposableRectangleDrawer(Color baseColor)
			: base(baseColor) {}

		public DisposableRectangleDrawer(Color baseColor, Color activeColor)
			: base(baseColor, activeColor) {}

		public DisposableRectangleDrawer(Color baseColor, Color activeColor, Color hoverColor)
			: base(baseColor, activeColor, hoverColor) {}

		protected override void DoDraw(int controlID, Vector2 position, float size, float rotation) {
			Color color = GUIUtility.hotControl == controlID ? activeColor : baseColor;
			using (new DisposableHandleColor(color)) {
				Handles.RectangleCap(controlID, position, GUIHelpers.Rotate2D(rotation), size);
			}
		}

		public override float GetDistance(Vector2 position, float size, float rotation) {
			return HandleUtility.DistanceToRectangle(position, GUIHelpers.Rotate2D(rotation), size);
		}
	}
}