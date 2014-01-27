using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableHandles {
	public abstract class HandleDrawerColorBase : HandleDrawerBase {
		protected readonly Color baseColor;
		protected readonly Color hoverColor;
		protected readonly Color activeColor;

		protected HandleDrawerColorBase() {
			baseColor = Color.white;
			activeColor = baseColor;
			hoverColor = baseColor;
		}

		protected HandleDrawerColorBase(Color baseColor) {
			this.baseColor = baseColor;
			activeColor = baseColor;
			hoverColor = baseColor;
		}

		protected HandleDrawerColorBase(Color baseColor, Color activeColor) {
			this.baseColor = baseColor;
			this.activeColor = activeColor;
			hoverColor = baseColor;
		}

		protected HandleDrawerColorBase(Color baseColor, Color activeColor, Color hoverColor) {
			this.baseColor = baseColor;
			this.activeColor = activeColor;
			this.hoverColor = hoverColor;
		}
	}
}