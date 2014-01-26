using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableCircleDrawer : DisposableHandleDrawerColorBase {
		public DisposableCircleDrawer(Color baseColor)
			: base(baseColor) {}

		public DisposableCircleDrawer(Color baseColor, Color activeColor)
			: base(baseColor, activeColor) {}

		public DisposableCircleDrawer(Color baseColor, Color activeColor, Color hoverColor)
			: base(baseColor, activeColor, hoverColor) {}

		protected override void DoDraw(int controlID, Vector2 position, float size, float rotation) {
			Color color = GUIUtility.hotControl == controlID ? activeColor : baseColor;
			using (new DisposableHandleColor(color)) {
				Handles.CircleCap(controlID, position, Quaternion.identity, size);
			}
		}

		public override float GetDistance(Vector2 position, float size, float rotation) {
			return HandleUtility.DistanceToCircle(position, size);
		}
	}
}