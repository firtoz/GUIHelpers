using UnityEngine;

namespace toxicFork.GUIHelpers {
	public static class Logger {
		public static void Log(params object[] args) {
			string[] strings = new string[args.Length];
			for (int i = 0; i < args.Length; i++) {
				object o = args[i];
				strings[i] = o + "";
			}
			Debug.Log(string.Join("   ", strings));
		}
	}
}