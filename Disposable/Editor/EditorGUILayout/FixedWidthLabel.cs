using System;
using toxicFork.GUIHelpers.DisposableEditorGUI;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableEditorGUILayout {

	public class FixedWidthLabel : IDisposable {
        private readonly ZeroIndent indentReset;

		public FixedWidthLabel(string label)
			: this(new GUIContent(label)) {}

		public FixedWidthLabel(GUIContent label) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label,
				GUILayout.Width(GUI.skin.label.CalcSize(label).x +
				                Helpers.IndentMultiplier*Mathf.Max(0, EditorGUI.indentLevel)));

			indentReset = new ZeroIndent();
		}

		public void Dispose() {
			indentReset.Dispose();
            EditorGUILayout.EndHorizontal();
		}
	}
}
