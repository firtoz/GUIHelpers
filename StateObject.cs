using UnityEngine;

namespace toxicFork.GUIHelpers {
	public class StateObject {
		public static T Get<T>(FocusType focusType = FocusType.Passive) {
			return Get<T>(GUIUtility.GetControlID(focusType));
		}

		public static T Get<T>(int controlID) {
			return (T) GUIUtility.GetStateObject(typeof (T), controlID);
		}
	}

	public class StateObject<T> {
		private readonly T contents;

		public StateObject(int controlID) {
			contents = (T) GUIUtility.GetStateObject(typeof (T), controlID);
		}

		public static implicit operator T(StateObject<T> state) {
			return state.contents;
		}
	}
}