using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableMaterialColor : IDisposable {
		private readonly Material material;
		private readonly Color color;

		public DisposableMaterialColor(Material material, Color color) {
			this.material = material;
			this.color = material.color;
			material.color = color;
		}

		public void Dispose() {
			material.color = color;
		}
	}
}