#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using toxicFork.GUIHelpers.DisposableEditorGUI;
using UnityEditor;
using UnityEngine;

public class FoldoutHelper {
    private readonly Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();

    public void Foldout(string name, GUIContent content, Action action) {
        bool currentStatus = foldoutStates.ContainsKey(name) && foldoutStates[name];

        currentStatus = EditorGUILayout.Foldout(currentStatus, content);

        if (currentStatus) {
            using (new Indent()) {
                action();
            }  
        }

        foldoutStates[name] = currentStatus;
    }
}
#endif