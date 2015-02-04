using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableHandles {
    public abstract class HandleDrawerStateObjectBase {
        public int controlID;
        public bool hovering;
        public float size;
        public Vector2 position;
        public float rotation;
    }

    public abstract class HandleDrawerBase : IHandleDrawer, IDisposable {
        protected abstract void DoDraw(int controlID, Vector2 position, float size, float rotation, bool hovering);

        protected virtual void DoDraw(HandleDrawerStateObjectBase stateObject) {
            DoDraw(stateObject.controlID, stateObject.position, stateObject.size, stateObject.rotation, stateObject.hovering);
        }

		public void Draw(int controlID, Vector2 position, float size, float rotation) {
		    Draw(controlID, position, size, rotation, false);
		}

		public void Draw(int controlID, Vector2 position, float size, float rotation, bool hovering) {
			if (Event.current.type == EventType.repaint) {
				DoDraw(controlID, position, size, rotation, hovering);
			}
		}

	    public void Draw(HandleDrawerStateObjectBase stateObject) {
            if (Event.current.type == EventType.repaint)
            {
                DoDraw(stateObject);
            }
	    }

		public abstract float GetDistance(Vector2 position, float size, float rotation);

		public void Dispose() {}
	}
}