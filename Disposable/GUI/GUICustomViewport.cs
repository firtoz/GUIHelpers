using toxicFork.GUIHelpers.Disposable;
using toxicFork.GUIHelpers.DisposableGUILayout;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGUI {
    public class GUICustomViewport : DisposableHelperBase {
        public GUICustomViewport(Rect area, Rect viewport) {
            disposables.Push(new GUILayoutArea(area));
            disposables.Push(new GUIGroup(new Rect(0, 0, viewport.width, viewport.height)));
            disposables.Push(new GUILayoutArea(new Rect(-viewport.x, -viewport.y, area.width, area.height)));
            disposables.Push(
                new GUIMatrix(Matrix4x4.TRS(new Vector3(viewport.x, viewport.y, 0),
                    Quaternion.identity,
                    Vector3.one)));
        }
    }
}