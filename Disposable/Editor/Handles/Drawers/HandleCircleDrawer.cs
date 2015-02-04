#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableHandles {
	public class HandleCircleDrawer : HandleDrawerColorBase {
		public HandleCircleDrawer(Color baseColor)
			: base(baseColor) {}

		public HandleCircleDrawer(Color baseColor, Color activeColor)
			: base(baseColor, activeColor) {}

		public HandleCircleDrawer(Color baseColor, Color activeColor, Color hoverColor)
			: base(baseColor, activeColor, hoverColor) {}

		

		public override float GetDistance(Vector2 position, float size, float rotation) {
			return HandleUtility.DistanceToCircle(position, size);
		}

	    protected override void DrawShape(int controlID, Vector2 position, float size, float rotation) {
            Handles.CircleCap(controlID, position, Quaternion.identity, size);
	    }
	}
}

#endif