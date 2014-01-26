using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public abstract class DisposableHandleDrawerColorBase : DisposableHandleDrawerBase {
		protected readonly Color baseColor;
		protected readonly Color hoverColor;
		protected readonly Color activeColor;

		protected DisposableHandleDrawerColorBase() {
			baseColor = Color.white;
			activeColor = baseColor;
			hoverColor = baseColor;
		}

		protected DisposableHandleDrawerColorBase(Color baseColor) {
			this.baseColor = baseColor;
			activeColor = baseColor;
			hoverColor = baseColor;
		}

		protected DisposableHandleDrawerColorBase(Color baseColor, Color activeColor) {
			this.baseColor = baseColor;
			this.activeColor = activeColor;
			hoverColor = baseColor;
		}

		protected DisposableHandleDrawerColorBase(Color baseColor, Color activeColor, Color hoverColor) {
			this.baseColor = baseColor;
			this.activeColor = activeColor;
			this.hoverColor = hoverColor;
		}
	}
}