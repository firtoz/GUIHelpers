using System;
using System.Collections;
using toxicFork.GUIHelpers.Disposable;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace toxicFork.GUIHelpers {
    public class EditorGUIHelpers : GUIHelpers{
	    public static void RecordUndo(String action, params Object[] objects) {
            Undo.RecordObjects(objects, action);
        }

        public static T PropertyToEnum<T>(SerializedProperty property) {
            return (T) Enum.Parse(typeof (T), property.enumNames[property.enumValueIndex]);
        }

        public static bool RadialSlider(int controlID, Vector3 position, float size, Material material) {
            return false;
        }

        public static bool CustomHandleButton(int controlID, Vector3 buttonPosition, float buttonSize, Texture2D texture,
            Texture2D hotTexture = null) {
            return CustomHandleButton(controlID, buttonPosition, buttonSize, texture, hotTexture, Color.white);
        }

        public static bool CustomHandleButton(int controlID, Vector3 buttonPosition, float buttonSize, Texture2D texture,
            Color color) {
            return CustomHandleButton(controlID, buttonPosition, buttonSize, texture, null, color);
        }

        public static bool CustomHandleButton(int controlID, Vector3 buttonPosition, float buttonSize, Texture2D texture,
            Texture2D hotTexture, Color color) {
            float distance = HandleUtility.DistanceToRectangle(buttonPosition, Quaternion.identity, buttonSize*0.5f);
            HandleUtility.AddControl(controlID, distance);
            switch (Event.current.type) {
                case EventType.repaint:
                    using (DisposableGUITextureDrawer drawer = new DisposableGUITextureDrawer(texture, hotTexture)) {
                        using (
                            new DisposableMaterialProperty(drawer.Material, "_Hot",
                                GUIUtility.hotControl == controlID && distance <= 0 ? 1f : 0f)
                            ) {
                            using (new DisposableMaterialColor(drawer.Material, color)) {
                                drawer.DrawSquare(buttonPosition, Quaternion.identity, buttonSize);
                            }
                        }
                        HandleUtility.Repaint();
                    }
                    break;
                case EventType.mouseMove: {
                    break;
                }
                case EventType.mouseDown:
                    if (GUIUtility.hotControl == 0 && distance <= 0 && Event.current.button == 0) {
                        GUIUtility.hotControl = controlID;

                        Event.current.Use();
                        HandleUtility.Repaint();
                    }
                    break;
                case EventType.mouseUp:
                    if (GUIUtility.hotControl == controlID) {
                        GUIUtility.hotControl = 0;
                        if (distance <= 0) {
                            Event.current.Use();
                            HandleUtility.Repaint();
                            return true;
                        }
                    }
                    break;
            }
            return false;
            //throw new NotImplementedException();
        }

        public static void SetEditorCursor(MouseCursor cursor) {
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), cursor);
        }

        public static void SetEditorCursor(MouseCursor cursor, int controlID) {
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), cursor, controlID);
        }

        public class AngleSliderInfo {
            public int button;
            public float dragAngle;
        }

        public static float AngleSlider(int controlID, DisposableHandleDrawerBase drawer, Vector2 center, float angle,
            float distanceFromCenter, float handleSize) {
            Vector2 handlePosition = center + Rotated2DVector(angle)*distanceFromCenter;
            float distanceFromDrawer = drawer.GetDistance(handlePosition, handleSize, angle);
            HandleUtility.AddControl(controlID, distanceFromDrawer);

            AngleSliderInfo info = StateObject.Get<AngleSliderInfo>(controlID);

            Vector2 mousePosition = Event.current.mousePosition;
            Vector2 worldMousePosition = HandlePointToWorld(mousePosition);

            float mouseAngle = GetAngle(worldMousePosition - center);

            if (Event.current.type == EventType.repaint) {
                drawer.Draw(controlID, handlePosition, handleSize, angle);
            }

            if (GUIUtility.hotControl == controlID) {
                switch (Event.current.type)
                {
                    case EventType.mouseUp:
                        if(Event.current.button == info.button) {
                            Event.current.Use();
                            GUIUtility.hotControl = 0;
                        }
                        break;
                    case EventType.mouseMove:
                        Event.current.Use();
                        Debug.Log("mouse moved");
                        break;
                    case EventType.mouseDrag:
                        Event.current.Use();

                        angle += Mathf.DeltaAngle(info.dragAngle, mouseAngle);
                        info.dragAngle = mouseAngle;
                        
                        GUI.changed = true;
                        break;
                }
            }
            else if(GUIUtility.hotControl == 0) {
                switch (Event.current.type) {
                        case EventType.mouseDown:
                        if (distanceFromDrawer <= Mathf.Epsilon) {
                            info.button = Event.current.button;
                            info.dragAngle = mouseAngle;
                            Event.current.Use();
                            GUIUtility.hotControl = controlID;
                        }
                        break;
                }
            }

            return angle;
        }


        public static Vector3 HandlePointToWorld(Vector2 mousePosition)
        {
            return HandleUtility.GUIPointToWorldRay(mousePosition).origin;
        }
    }
}