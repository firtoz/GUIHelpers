using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableCustomViewport : DisposableHelper {
		public DisposableCustomViewport(Rect area, Rect viewport) {
			disposables.Push(new DisposableGUILayoutArea(area));
			disposables.Push(new DisposableGUIGroup(new Rect(0, 0, viewport.width, viewport.height)));
			disposables.Push(new DisposableGUILayoutArea(new Rect(-viewport.x, -viewport.y, area.width, area.height)));
			disposables.Push(
				new DisposableGUIMatrix(Matrix4x4.TRS(new Vector3(viewport.x, viewport.y, 0),
					Quaternion.identity,
					Vector3.one)));
		}
	}
}