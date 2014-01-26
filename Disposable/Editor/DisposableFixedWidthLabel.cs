using System;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.Disposable {
	public class DisposableFixedWidthLabel : IDisposable {
		private readonly DisposableZeroIndent indentReset;

		public DisposableFixedWidthLabel(string label)
			: this(new GUIContent(label)) {}

		public DisposableFixedWidthLabel(GUIContent label) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label,
				GUILayout.Width(GUI.skin.label.CalcSize(label).x +
				                GUIHelpers.IndentMultiplier*Mathf.Max(0, EditorGUI.indentLevel)));

			indentReset = new DisposableZeroIndent();
		}

		public void Dispose() {
			indentReset.Dispose();
			EditorGUILayout.EndHorizontal();
		}
	}
}