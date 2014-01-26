using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableMaterialProperty : IDisposable {
		private readonly System.Object value;
		private readonly Material material;
		private readonly string propertyName;

		private readonly Type type;


		public DisposableMaterialProperty() {}

		public DisposableMaterialProperty(Material material, string propertyName, float floatVal) {
			this.material = material;
			this.propertyName = propertyName;

			type = typeof (float);

			if (material.HasProperty(propertyName)) {
				value = material.GetFloat(propertyName);
				material.SetFloat(propertyName, floatVal);
			}
		}

		public void Dispose() {
			if (value != null) {
				if (type == typeof (float)) {
					material.SetFloat(propertyName, ((float?) value).Value);
				}
			}
		}
	}
}