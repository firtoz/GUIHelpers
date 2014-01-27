using System;

namespace toxicFork.GUIHelpers {
	public class IndentRect : IDisposable {
		private readonly float offset;
		private readonly MutableRect rect;

		public IndentRect(MutableRect r, int i = 1) {
			rect = r;
			offset = i;
			offset = Helpers.IndentMultiplier*i;
			rect.x += offset;
			rect.width -= offset;
		}

		public void Dispose() {
			rect.x -= offset;
			rect.width += offset;
		}
	}
}