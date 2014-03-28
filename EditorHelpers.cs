#if UNITY_EDITOR
using toxicFork.GUIHelpers.Disposable;
using toxicFork.GUIHelpers.DisposableGL;
using MaterialProperty = toxicFork.GUIHelpers.Disposable.MaterialProperty;
using System;
using toxicFork.GUIHelpers.DisposableGUI;
using toxicFork.GUIHelpers.DisposableHandles;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace toxicFork.GUIHelpers {
    public class EditorHelpers : Helpers {
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
            switch (Event.current.type) {
                case EventType.layout:
                    HandleUtility.AddControl(controlID, distance);
                    break;
                case EventType.repaint:
                    using (GUITextureDrawer drawer = new GUITextureDrawer(texture, hotTexture)) {
                        drawer.alwaysVisible = true;
                        using (
                            new MaterialProperty(drawer.Material, "_Hot",
                                GUIUtility.hotControl == controlID && distance <= 0 ? 1f : 0f)
                            ) {
                            using (new MaterialColor(drawer.Material, color)) {
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
            public float mouseAngle;
            public Vector2 mousePosition;
            public float angle;
        }

        public static float AngleSlider(int controlID, HandleDrawerBase drawer, Vector2 center, float angle,
            float distanceFromCenter, float handleSize, float snap = 0) {
            AngleSliderInfo info = StateObject.Get<AngleSliderInfo>(controlID);

            Event current = Event.current;

            if (GUIUtility.hotControl == controlID) {
                angle = info.angle;
            }
            Vector2 handlePosition = center + Helpers2D.GetDirection(angle)*distanceFromCenter;

            EventType typeForControl = current.GetTypeForControl(controlID);

            switch (typeForControl) {
                case EventType.layout:
                    float distanceFromDrawer = drawer.GetDistance(handlePosition, handleSize, angle);
                    HandleUtility.AddControl(controlID, distanceFromDrawer);
                    break;
            }

            if (GUIUtility.hotControl == controlID) {
                switch (typeForControl) {
                    case EventType.mouseUp:
                        if (current.button == info.button) {
                            current.Use();
                            GUIUtility.hotControl = 0;
                        }
                        break;
                    case EventType.mouseDrag:
                        current.Use();

                        info.mousePosition += new Vector2(current.delta.x, current.delta.y);
                        Vector2 worldMousePosition = HandlePointToWorld(info.mousePosition);

                        float mouseAngle = Helpers2D.GetAngle(worldMousePosition - center);

                        info.angle += Mathf.DeltaAngle(info.mouseAngle, mouseAngle);
                        info.mouseAngle = mouseAngle;

                        angle = Handles.SnapValue(info.angle, snap);

                        GUI.changed = true;
                        break;
                }
            }
            else {
                if (GUIUtility.hotControl == 0) {
                    switch (typeForControl) {
                        case EventType.mouseDown:
                            if (HandleUtility.nearestControl == controlID) {
                                info.button = current.button;
                                info.mousePosition = current.mousePosition;

                                Vector2 worldMousePosition = HandlePointToWorld(info.mousePosition);

                                float mouseAngle = Helpers2D.GetAngle(worldMousePosition - center);
                                info.mouseAngle = mouseAngle;
                                info.angle = angle;
                                current.Use();
                                GUIUtility.hotControl = controlID;
                            }
                            break;
                    }
                }
            }

            if (typeForControl == EventType.repaint) {
                drawer.Draw(controlID, handlePosition, handleSize, angle);
            }

            return angle;
        }


        public static Vector3 HandlePointToWorld(Vector2 mousePosition) {
            return HandleUtility.GUIPointToWorldRay(mousePosition).origin;
        }

        public static void DrawThickLine(Vector3 p1, Vector3 p2, float thickness, bool alwaysVisible = false) {
            Camera current = Camera.current;
            if (!current || Event.current.type != EventType.Repaint) {
                return;
            }
            Color c = Handles.color*new Color(1f, 1f, 1f, 0.75f);
            if (alwaysVisible) {
                AlwaysVisibleVertexGUIMaterial.SetPass(0);
            } else {
                VertexGUIMaterial.SetPass(0);
            }

            using (new GLMatrix()) {
                GL.MultMatrix(Handles.matrix);

                Vector3 screenPoint1 = current.WorldToScreenPoint(p1);
                Vector3 screenPoint2 = current.WorldToScreenPoint(p2);

                Vector3 dir = (screenPoint2 - screenPoint1).normalized;
                Vector3 perpendicular = new Vector2(-dir.y, dir.x).normalized*thickness*0.5f;
                dir *= (thickness*0.5f);

                GL.Begin(GL.QUADS);
                GL.Color(c);
                GL.Vertex(current.ScreenToWorldPoint(screenPoint1 - dir - perpendicular));
                GL.Vertex(current.ScreenToWorldPoint(screenPoint1 - dir + perpendicular));
                GL.Vertex(current.ScreenToWorldPoint(screenPoint2 + dir + perpendicular));
                GL.Vertex(current.ScreenToWorldPoint(screenPoint2 + dir - perpendicular));
                GL.End();
            }
        }

        public static void DrawThickLineWithOutline(Vector3 a, Vector3 b, float mainThickness, float outlineThickness,
            bool alwaysVisible = false) {
            Color bg = Color.black;
            bg.a = Handles.color.a;
            using (new HandleColor(bg)) {
                DrawThickLine(a, b, mainThickness + outlineThickness, alwaysVisible);
            }

            Vector3 cameraVectorA = HandleToCameraPoint(a);
            Vector3 cameraVectorB = HandleToCameraPoint(b);

            cameraVectorA.z -= 0.01f;
            cameraVectorB.z -= 0.01f;

            a = CameraToHandlePoint(cameraVectorA);
            b = CameraToHandlePoint(cameraVectorB);

            DrawThickLine(a, b, mainThickness, alwaysVisible);
        }

        public static Vector3 CameraToHandlePoint(Vector3 cameraVectorA) {
            return Handles.inverseMatrix.MultiplyPoint(Camera.current.ScreenToWorldPoint(cameraVectorA));
        }

        public static Vector3 HandleToCameraPoint(Vector3 a) {
            return Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(a));
        }

        public static float LineSlider(int controlID, Vector2 center, float distance, float angle,
            float handleScale = 1f, bool alwaysVisible = false) {
            HoverState state = StateObject.Get<HoverState>(controlID);
            Vector2 direction = Helpers2D.GetDirection(angle);
            Vector2 wantedPosition = center + direction*distance;

            Vector2 normal = new Vector2(-direction.y, direction.x)*
                             HandleUtility.GetHandleSize(wantedPosition)*handleScale;

            EditorGUI.BeginChangeCheck();

            wantedPosition = Handles.Slider2D(controlID,
                wantedPosition,
                Vector3.forward,
                Vector3.up,
                Vector3.right,
                normal.magnitude*2,
                (id, position, rotation, size) => { },
                Vector2.zero);

            if (EditorGUI.EndChangeCheck()) {
                distance = Helpers2D.DistanceAlongLine(new Ray(center, direction), wantedPosition);
            }

            Event current = Event.current;

            float drawScale = 1;
            switch (current.GetTypeForControl(controlID)) {
                case EventType.mouseMove:
                    bool hovering = HandleUtility.nearestControl == controlID;
                    if (state.hovering != hovering) {
                        current.Use();
                        state.hovering = hovering;
                    }
                    break;
                case EventType.repaint:
                    Color color = Handles.color;

                    if (GUIUtility.hotControl == controlID || state.hovering) {
                        color = GUIUtility.hotControl == controlID ? Color.red : Color.yellow;

                        MouseCursor cursor = RotatedResizeCursor(direction);

                        SetEditorCursor(cursor, controlID);
                        drawScale = 2;
                    }
                    else if (GUIUtility.hotControl != 0) {
                        color.a = 0.5f;
                    }

                    Vector2 drawNormal = normal*drawScale;
                    using (new HandleColor(color)) {
                        Vector3 a = wantedPosition - drawNormal;
                        Vector3 b = wantedPosition + drawNormal;
                        if (GUIUtility.hotControl == controlID) {
                            Vector3 cameraVectorA = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(a));
                            Vector3 cameraVectorB = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(b));

                            cameraVectorA.z -= 0.01f;
                            cameraVectorB.z -= 0.01f;

                            a = Handles.inverseMatrix.MultiplyPoint(Camera.current.ScreenToWorldPoint(cameraVectorA));
                            b = Handles.inverseMatrix.MultiplyPoint(Camera.current.ScreenToWorldPoint(cameraVectorB));
                        }
                        DrawThickLineWithOutline(a, b, 2, 2, alwaysVisible);
                    }
                    break;
            }
            return distance;
        }

        public static MouseCursor RotatedResizeCursor(Vector2 direction) {
            Vector3 cameraDirection = GetCameraDirection(direction);

            float angle = Helpers2D.GetAngle(cameraDirection);
            MouseCursor cursor;
            if (Mathf.Abs(Mathf.DeltaAngle(angle, 0)) <= 22.5f || Mathf.Abs(Mathf.DeltaAngle(angle, 180)) <= 22.5f) {
                cursor = MouseCursor.ResizeHorizontal;
            }
            else if (Mathf.Abs(Mathf.DeltaAngle(angle, 45)) <= 22.5f
                     || Mathf.Abs(Mathf.DeltaAngle(angle, 225)) <= 22.5f) {
                cursor = MouseCursor.ResizeUpRight;
            }
            else if (Mathf.Abs(Mathf.DeltaAngle(angle, 135)) <= 22.5f
                     || Mathf.Abs(Mathf.DeltaAngle(angle, 315)) <= 22.5f) {
                cursor = MouseCursor.ResizeUpLeft;
            }
            else {
                cursor = MouseCursor.ResizeVertical;
            }
            return cursor;
        }

        private static Vector3 GetCameraDirection(Vector2 direction) {
            Vector3 worldDirection = Handles.matrix.MultiplyVector(direction);
            Vector3 cameraDirection = Camera.current
                ? Camera.current.worldToCameraMatrix.MultiplyVector(worldDirection)
                : worldDirection;
            return cameraDirection;
        }
    }
}

#endif