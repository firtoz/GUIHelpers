using System;
using toxicFork.GUIHelpers.DisposableEditorGUILayout;
using toxicFork.GUIHelpers.DisposableGUI;
using toxicFork.GUIHelpers.DisposableGUILayout;
using toxicFork.GUIHelpers.DisposableHandles;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class Pullout : DisposableHelperBase {
		private readonly bool oldGUIEnabled;

		private readonly bool wantsRepaint;
		private readonly PulloutState state;

		public Pullout(Rect screenRect, float minWidth, bool scrollable = false, Color? backgroundColor = null) {
			var maxWidth = screenRect.width;

			minWidth = Mathf.Max(minWidth, 0);
			minWidth = Mathf.Min(maxWidth, minWidth);

			var controlID = GUIUtility.GetControlID(FocusType.Keyboard, screenRect);
			state = StateObject.Get<PulloutState>(controlID);
			state.Tick();

			if (backgroundColor != null) {
				state.Setup(backgroundColor.Value);
			}

			var mousePosition = Event.current.mousePosition;
			if (screenRect.Contains(mousePosition) && mousePosition.x - screenRect.x < state.edgePosition) {
				if (!state.mouseIn && (GUIUtility.hotControl == 0 || GUIUtility.hotControl == controlID)) {
					state.mouseIn = true;
					wantsRepaint = true;
				}

				if (state.mouseIn) {
					state.wantedEdgePosition = maxWidth;
				}

				if (GUIUtility.keyboardControl == controlID) {
					GUIUtility.hotControl = controlID;
				}

				HandleUtility.AddDefaultControl(controlID);
			}
			else {
				if (state.mouseIn) {
					state.mouseIn = false;
					wantsRepaint = true;
				}
				state.wantedEdgePosition = minWidth;
			}

			disposables.Push(new HandleGUI());
			disposables.Push(new GUILayoutArea(screenRect));

			//groupArea = new GroupArea(new Rect(screenRect.x, screenRect.y, screenRect.width, screenRect.height));


			var offset = maxWidth - state.edgePosition;

			disposables.Push(new GUICustomViewport(new Rect(0, 0, screenRect.width, screenRect.height),
				new Rect(offset, 0, screenRect.width - offset, screenRect.height)));

			disposables.Push(new GUIMatrix(Matrix4x4.TRS(new Vector3(-offset, 0, 0), Quaternion.identity,
				new Vector3(1, 1, 1))));

			using (new HandleColor(Color.red)) {
				Handles.DrawLine(new Vector3(maxWidth + 1, 0, 0),
					new Vector3(maxWidth + 1, screenRect.height, 0));
				Handles.DrawLine(new Vector3(0, 0, 0),
					new Vector3(maxWidth + 1, 0, 0));
				Handles.DrawLine(new Vector3(0, screenRect.height, 0),
					new Vector3(maxWidth + 1, screenRect.height, 0));
			}

			GUI.DrawTexture(new Rect(0, 0, maxWidth, screenRect.height),
				state.background, ScaleMode.ScaleAndCrop);

			using (new HandleColor(Color.green)) {
				Handles.DrawLine(new Vector3(0, 0, 0),
					new Vector3(0, screenRect.height, 0));
			}

			if (scrollable) {
				disposables.Push(new ScrollView());
			}

			oldGUIEnabled = GUI.enabled;
			GUI.enabled = state.mouseIn && state.Opened(maxWidth);

			if (Math.Abs(state.edgePosition - maxWidth) > 0.25f) {
				wantsRepaint = true;
			}
		}

		public override void Cleanup() {
			GUI.enabled = oldGUIEnabled;

			if (state.mouseIn) {
				EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), MouseCursor.Arrow);
				switch (Event.current.type) {
					case EventType.layout:
						break;
					case EventType.repaint:
						Event.current.Use();
						break;
					case EventType.ignore:
					case EventType.used:
						break;
					case EventType.mouseMove:
					case EventType.mouseDown:
					case EventType.mouseDrag:
					case EventType.mouseUp:
					case EventType.keyDown:
					case EventType.keyUp:
						Event.current.Use();
						break;
					default:
						Debug.Log(Event.current.type);
						break;
				}
			}
			if (wantsRepaint) {
				HandleUtility.Repaint();
			}
		}

		internal class PulloutState : IDisposable {
			public float edgePosition, wantedEdgePosition;
			public bool mouseIn;
			private double time;


			private static Texture2D _pulloutBackground;

			private static Texture2D pulloutBackground {
				get {
					if (_pulloutBackground == null) {
						_pulloutBackground = new Texture2D(1, 1) {
							hideFlags = HideFlags.HideAndDontSave
						};
						_pulloutBackground.SetPixel(0, 0, new Color(194/255f, 194/255f, 194/255f));
						_pulloutBackground.Apply(false, true);
					}
					return _pulloutBackground;
				}
			}

			private bool setup;

			private void SetBackground(Color color) {
				background = new Texture2D(1, 1) {
					hideFlags = HideFlags.HideAndDontSave
				};
				background.SetPixel(0, 0, color);
				background.Apply(false, true);
			}

			public Texture2D background = pulloutBackground;

			private double lastTick;

			private bool updating_;

			private bool updating {
				get { return updating_; }
				set {
					if (value != updating) {
						if (value) {
							EditorApplication.update += Update;
						}
						else {
// ReSharper disable DelegateSubtraction
							EditorApplication.update -= Update;
// ReSharper restore DelegateSubtraction
						}
					}
					updating_ = value;
				}
			}

			public PulloutState() {
				time = EditorApplication.timeSinceStartup;
				lastTick = time;

				updating = true;
			}

			public void Tick() {
				lastTick = EditorApplication.timeSinceStartup;
				if (!updating) {
					updating = true;
				}
			}

			private void Update() {
				var currentTime = EditorApplication.timeSinceStartup;
				var deltaTime = (float) (currentTime - time);
				edgePosition = Mathf.Lerp(edgePosition, wantedEdgePosition, Mathf.Clamp(10f*deltaTime, 0f, 1f));
				time = currentTime;

				if (currentTime - lastTick > 1) {
					updating = false;
				}
			}

			public bool Opened(float maxWidth) {
				return (Mathf.Abs(edgePosition - maxWidth) < 5.0f);
			}

			// ReSharper disable DelegateSubtraction
			public void Dispose() {
				EditorApplication.update -= Update;
			}

			~PulloutState() {
				Debug.Log("?!??");
			}

			// ReSharper restore DelegateSubtraction

			public void Setup(Color color) {
				if (!setup) {
					setup = true;
					SetBackground(color);
				}
			}
		}
	}
}