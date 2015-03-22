using System;
using System.Collections.Generic;
using toxicFork.GUIHelpers.DisposableEditorGUI;
using UnityEditor;
using UnityEngine;

public class PersistentFoldoutHelper {
    private readonly Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
    private readonly string keyPrefix;

    public PersistentFoldoutHelper(string keyPrefix) {
        this.keyPrefix = keyPrefix;
    }

    public void Foldout(string name, GUIContent content, Action action) {
        bool currentStatus;
        var keyName = keyPrefix + "." + name;
        if (!foldoutStates.ContainsKey(name)) {
            foldoutStates[name] = EditorPrefs.GetBool(keyName, false);
        }

        currentStatus = foldoutStates[name];

        EditorGUI.BeginChangeCheck();
        currentStatus = EditorGUILayout.Foldout(currentStatus, content);
        if (EditorGUI.EndChangeCheck()) {
            foldoutStates[name] = currentStatus;
            EditorPrefs.SetBool(keyName, currentStatus);
        }

        if (currentStatus) {
            using (new Indent()) {
                action();
            }  
        }
    }
}