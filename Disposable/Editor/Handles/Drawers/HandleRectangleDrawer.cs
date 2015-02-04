using toxicFork.GUIHelpers.DisposableHandles;
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


		public override float GetDistance(Vector2 position, float size, float rotation) {
			return HandleUtility.DistanceToRectangle(position, Helpers2D.Rotate(rotation), size);
		}

	    protected override void DrawShape(int controlID, Vector2 position, float size, float rotation) {
            Handles.RectangleCap(controlID, position, Helpers2D.Rotate(rotation), size);
	    }
	}
}
