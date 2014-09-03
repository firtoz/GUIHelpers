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

		protected override void DoDraw(int controlID, Vector2 position, float size, float rotation) {
			Color color = GUIUtility.hotControl == controlID ? activeColor : baseColor;
			using (new HandleColor(color)) {
				Handles.CircleCap(controlID, position, Quaternion.identity, size);
			}
		}

		protected override void DoDraw(int controlID, Vector2 position, float size, float rotation, bool hovering) {
            Color color;
		    if (GUIUtility.hotControl == controlID) {
		        color = activeColor;
		    }
		    else {
		        if (hovering) {
		            color = hoverColor;
		        }
		        else {
		            color = baseColor;
		        }
		    }
		    using (new HandleColor(color)) {
				Handles.CircleCap(controlID, position, Quaternion.identity, size);
			}
		}

		public override float GetDistance(Vector2 position, float size, float rotation) {
			return HandleUtility.DistanceToCircle(position, size);
		}
	}
}

#endif