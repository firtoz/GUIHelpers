using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace toxicFork.GUIHelpers.DisposableEditor
{
    public class Modification : IDisposable {
        private readonly Object[] objects;

        public Modification(string action, params Object[] objects) {
            this.objects = objects;
            EditorHelpers.RecordUndo(action, objects);
        }
        public void Dispose() {
            foreach (var o in objects) {
                EditorUtility.SetDirty(o);
            }
        }
    }
}