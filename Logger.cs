using UnityEngine;

namespace toxicFork.GUIHelpers {
	public static class Logger {
		public static void Log(params object[] args) {
			var strings = new string[args.Length];
			for (var i = 0; i < args.Length; i++) {
				var o = args[i];
				strings[i] = o + "";
			}
			Debug.Log(string.Join("   ", strings));
		}
	}
}