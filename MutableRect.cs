using UnityEngine;

namespace toxicFork.GUIHelpers {
	public class MutableRect {
		public float x, y, width, height;
		public bool autoNewLine;

		public MutableRect(Rect r) {
			x = r.x;
			y = r.y;
			width = r.width;
			height = r.height;
		}

		public static implicit operator MutableRect(Rect r) {
			return new MutableRect(r);
		}

		public static implicit operator Rect(MutableRect r) {
			var y = r.y;
			if (r.autoNewLine) {
				r.y += r.height;
			}
			return new Rect(r.x, y, r.width, r.height);
		}
	}
}