using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableRotatedGUI : DisposableHelper {
		private DisposableRotatedGUI(Vector2 position, Vector2 offset = new Vector2()) {
			disposables.Push(new DisposableGUIMatrix(Matrix4x4.TRS(position + offset, Quaternion.identity, Vector3.one),
				true));
		}

		public DisposableRotatedGUI(Vector2 position, float rotation, Vector2 offset = new Vector2())
			: this(position, offset) {
			GUIUtility.RotateAroundPivot(rotation, position);
		}

		public DisposableRotatedGUI(Vector2 position, Quaternion rotation, Vector2 offset = new Vector2())
			: this(position, offset) {
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one)*GUI.matrix;
		}
	}
}