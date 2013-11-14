using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers {
    public class GUIHelpers {
        public const int IndentMultiplier = 15;

        public static T PropertyToEnum<T>(SerializedProperty property) {
            return (T) Enum.Parse(typeof (T), property.enumNames[property.enumValueIndex]);
        }

        public static int EnumToIndex<T>(T value) {
            return new ArrayList(Enum.GetValues(typeof (T))).IndexOf(value);
        }

        public static bool RadialSlider(int controlID, Vector3 position, float size, Material material) {
            return false;
        }

        public static bool CustomHandleButton(int controlID, Vector3 buttonPosition, float buttonSize, Texture2D texture, Texture2D hotTexture = null) {
            return CustomHandleButton(controlID, buttonPosition, buttonSize, texture, hotTexture, Color.white);
        }

        public static bool CustomHandleButton(int controlID, Vector3 buttonPosition, float buttonSize, Texture2D texture, Color color) {
            return CustomHandleButton(controlID, buttonPosition, buttonSize, texture, null, color);
        }

        public static bool CustomHandleButton(int controlID, Vector3 buttonPosition, float buttonSize, Texture2D texture, Texture2D hotTexture, Color color)
        {
            float distance = HandleUtility.DistanceToRectangle(buttonPosition, Quaternion.identity, buttonSize*0.5f);
            HandleUtility.AddControl(controlID, distance);
            switch (Event.current.type) {
                case EventType.repaint:
                    using (DisposableGUITextureDrawer drawer = new DisposableGUITextureDrawer(texture, hotTexture))
                    {
                        
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

        /// <summary>
        /// Draws a square on the screen that is centered around the position.
        /// </summary>
        /// <param name="position">The center of the square on the screen</param>
        /// <param name="rotation">The rotation of the square. Does not work well with any rotation other than z.</param>
        /// <param name="size">The size of the square, in world coordinates.</param>
        public static void DrawSquare(Vector3 position, Quaternion rotation, float size) {
            float halfSize = size*.5f;
            Vector3 up = rotation*Vector3.up;
            Vector3 right = rotation*Vector3.right;
            Vector3 bottomLeft = position - up*halfSize - right*halfSize;
            GL.Begin(GL.QUADS);
            GL.TexCoord2(0, 0);
            GL.Vertex(bottomLeft);
            GL.TexCoord2(0, 1);
            GL.Vertex(bottomLeft + up*size);
            GL.TexCoord2(1, 1);
            GL.Vertex(bottomLeft + up*size + right*size);
            GL.TexCoord2(1, 0);
            GL.Vertex(bottomLeft + right*size);
            GL.End();
        }

        public static void SetEditorCursor(MouseCursor cursor) {
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), cursor);
        }

        public static void SetEditorCursor(MouseCursor cursor, int controlID) {
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), cursor, controlID);
        }
    }

    public class DisposableMaterialProperty : IDisposable {

        private readonly System.Object value;
        private readonly Material material;
        private readonly string propertyName;

        private readonly Type type;


        public DisposableMaterialProperty() {
            
        }

        public DisposableMaterialProperty(Material material, string propertyName, float floatVal) {
            this.material = material;
            this.propertyName = propertyName;

            type = typeof (float);

            if (material.HasProperty(propertyName)) {
                value = material.GetFloat(propertyName);
                material.SetFloat(propertyName, floatVal);
            }
        }

        public void Dispose() {
            if (value != null) {
                if (type == typeof (float)) {
                    material.SetFloat(propertyName, ((float?)value).Value);
                }
            }
        }
        
    }

    public class DisposableMaterialColor : IDisposable {
        private readonly Material material;
        private readonly Color color;

        public DisposableMaterialColor(Material material, Color color) {
            this.material = material;
            this.color = material.color;
            material.color = color;
        }

        public void Dispose() {
            material.color = color;
        }
    }

    public abstract class Drawer : IDisposable {
        public virtual void Dispose() {
        }

        public void DrawSquare(Vector3 position, Quaternion rotation, float size, Material material) {
            GL.PushMatrix();
            for (int i = 0; i < material.passCount; i++) {
                material.SetPass(i);
                GUIHelpers.DrawSquare(position, rotation, size);
            }
            GL.PopMatrix();
        }
    }

    public class DisposableGUITextureDrawer : Drawer {
        private static Material _guiMaterial;

        private static Material guiMaterial {
            get {
                return _guiMaterial ??
                       (_guiMaterial =
                        new Material(Shader.Find("GUI Helpers/GUI")) {hideFlags = HideFlags.HideAndDontSave});
            }
        }

        public Material Material {
            get {
                return guiMaterial;
            }
        }

        private readonly Texture2D texture;
        private readonly Texture2D hotTexture;
        private readonly Quaternion rotation;
        private readonly float scale;

        public DisposableGUITextureDrawer(Texture2D texture, Quaternion rotation = default(Quaternion), float scale = 1f) {
            this.rotation = rotation;
            this.texture = texture;
            this.scale = scale;
        }

        public DisposableGUITextureDrawer(Texture2D texture, Texture2D hotTexture, Quaternion rotation = default(Quaternion), float scale = 1f) {
            this.rotation = rotation;
            this.texture = texture;
            this.hotTexture = hotTexture;
            this.scale = scale;
        }

        public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
            guiMaterial.SetTexture(0, texture);
            if (hotTexture != null && guiMaterial.HasProperty("_HotTex"))
            {
                guiMaterial.SetTexture("_HotTex", hotTexture);
            }
            DrawSquare(position, this.rotation*rotation, scale*size, guiMaterial);
        }

        public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
            DrawSquare(position, rotation, size);
        }
    }

    public class DisposableMaterialDrawer : Drawer {
        private readonly Material material;
        private readonly Quaternion rotation;
        private readonly float scale;

        public DisposableMaterialDrawer(Material material, Quaternion rotation = default(Quaternion), float scale = 1f) {
            this.rotation = rotation;
            this.material = material;
            this.scale = scale;
        }

        public void DrawSquare(Vector3 position, Quaternion rotation, float size) {
            DrawSquare(position, this.rotation*rotation, scale*size, material);
        }

        public void DrawSquare(int controlID, Vector3 position, Quaternion rotation, float size) {
            DrawSquare(position, rotation, size);
        }
    }

    public class HorizontalGroup : IDisposable {
        public HorizontalGroup() {
            EditorGUILayout.BeginHorizontal();
        }

        public void Dispose() {
            EditorGUILayout.EndHorizontal();
        }
    }

    public class FixedWidthLabel : IDisposable {
        private readonly ZeroIndent indentReset;

        public FixedWidthLabel(string label)
            : this(new GUIContent(label)) {
        }

        public FixedWidthLabel(GUIContent label) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label,
                                       GUILayout.Width(GUI.skin.label.CalcSize(label).x +
                                                       GUIHelpers.IndentMultiplier*Mathf.Max(0, EditorGUI.indentLevel)));

            indentReset = new ZeroIndent();
        }

        public void Dispose() {
            indentReset.Dispose();
            EditorGUILayout.EndHorizontal();
        }
    }

    public class Indent : IDisposable {
        private readonly int offset;

        public Indent(int i = 1) {
            offset = i;
            EditorGUI.indentLevel += offset;
        }

        public void Dispose() {
            EditorGUI.indentLevel -= offset;
        }
    }

    public class DisposablePullout : DisposableHelper {
        private readonly bool oldGUIEnabled;

        private readonly bool wantsRepaint;
        private readonly PulloutState state;

        public DisposablePullout(Rect screenRect, float minWidth, bool scrollable = false, Color? backgroundColor = null) {
            float maxWidth = screenRect.width;

            minWidth = Mathf.Max(minWidth, 0);
            minWidth = Mathf.Min(maxWidth, minWidth);

            int controlID = GUIUtility.GetControlID(FocusType.Keyboard, screenRect);
            state = StateObject.Get<PulloutState>(controlID);
            state.Tick();

            if (backgroundColor != null) {
                state.Setup(backgroundColor.Value);
            }

            Vector2 mousePosition = Event.current.mousePosition;
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

            disposables.Push(new DisposableHandleGUI());
            disposables.Push(new GUILayoutArea(screenRect));

            //groupArea = new GroupArea(new Rect(screenRect.x, screenRect.y, screenRect.width, screenRect.height));


            float offset = maxWidth - state.edgePosition;

            //disposables.Push(new AreaGroup(new Rect(0, 0, maxWidth, 200)));

            disposables.Push(new CustomViewport(new Rect(0, 0, screenRect.width, screenRect.height),
                                                new Rect(offset, 0, screenRect.width - offset, screenRect.height)));

            disposables.Push(new CustomGUIMatrix(Matrix4x4.TRS(new Vector3(-offset, 0, 0), Quaternion.identity,
                                                               new Vector3(1, 1, 1))));

            using (new DisposableHandleColor(Color.red)) {
                Handles.DrawLine(new Vector3(maxWidth + 1, 0, 0),
                                 new Vector3(maxWidth + 1, screenRect.height, 0));
                Handles.DrawLine(new Vector3(0, 0, 0),
                                 new Vector3(maxWidth + 1, 0, 0));
                Handles.DrawLine(new Vector3(0, screenRect.height, 0),
                                 new Vector3(maxWidth + 1, screenRect.height, 0));
            }

            GUI.DrawTexture(new Rect(0, 0, maxWidth, screenRect.height),
                            state.background, ScaleMode.ScaleAndCrop);

            using (new DisposableHandleColor(Color.green)) {
                Handles.DrawLine(new Vector3(0, 0, 0),
                                 new Vector3(0, screenRect.height, 0));
            }

            if (scrollable) {
                disposables.Push(new EditorGUILayoutScrollView());
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
                            EditorApplication.update -= Update;
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
                double currentTime = EditorApplication.timeSinceStartup;
                float deltaTime = (float) (currentTime - time);
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

    public class DisposableGUIColor : IDisposable {
        private readonly Color previousColor;

        public DisposableGUIColor(Color color) {
            previousColor = GUI.color;
            GUI.color = color;
        }

        public void Dispose() {
            GUI.color = previousColor;
        }
    }

    public class DisposableHandleColor : IDisposable {
        private readonly Color previousColor;

        public DisposableHandleColor(Color color) {
            previousColor = GUI.color;
            Handles.color = color;
        }

        public DisposableHandleColor() {
            previousColor = GUI.color;
        }

        public void Dispose() {
            Handles.color = previousColor;
        }
    }

    public abstract class DisposableHelper : IDisposable {
        protected enum StackDisposeStrategy {
            BeforeCleanup,
            AfterCleanup
        }

        protected readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
        protected StackDisposeStrategy stackDisposeStrategy = StackDisposeStrategy.AfterCleanup;

        public void Dispose() {
            bool beforeCleanup = stackDisposeStrategy == StackDisposeStrategy.BeforeCleanup;
            if (beforeCleanup) {
                Cleanup();
            }
            while (disposables.Count > 0) {
                disposables.Pop().Dispose();
            }
            if (!beforeCleanup) {
                Cleanup();
            }
        }

        public virtual void Cleanup() {
        }
    }

    public class CustomViewport : DisposableHelper {
        public CustomViewport(Rect area, Rect viewport) {
            disposables.Push(new GUILayoutArea(area));
            disposables.Push(new GUIGroup(new Rect(0, 0, viewport.width, viewport.height)));
            disposables.Push(new GUILayoutArea(new Rect(-viewport.x, -viewport.y, area.width, area.height)));
            disposables.Push(
                             new CustomGUIMatrix(Matrix4x4.TRS(new Vector3(viewport.x, viewport.y, 0),
                                                               Quaternion.identity,
                                                               Vector3.one)));
        }
    }

    public class GUILayoutArea : IDisposable {
        public GUILayoutArea(Rect position) {
            GUILayout.BeginArea(position);
        }

        public void Dispose() {
            GUILayout.EndArea();
        }
    }

    public class GUIGroup : IDisposable {
        public GUIGroup(Rect position) {
            GUI.BeginGroup(position);
        }

        public void Dispose() {
            GUI.EndGroup();
        }
    }


    public class CustomGUIMatrix : IDisposable {
        private readonly Matrix4x4 oldMatrix;
        private readonly Matrix4x4 oldHandleMatrix;

        public CustomGUIMatrix() {
            oldMatrix = GUI.matrix;
            oldHandleMatrix = Handles.matrix;
        }

        public CustomGUIMatrix(Matrix4x4 matrix, bool replace = false)
            : this() {
            GUI.matrix = replace ? matrix : GUI.matrix*matrix;
        }

        public void Dispose() {
            Handles.matrix = oldHandleMatrix;
            GUI.matrix = oldMatrix;
        }
    }


    internal class ScrollState {
        public Vector2 position;
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

    public class StateObject {
        public static T Get<T>(FocusType focusType = FocusType.Passive) {
            return Get<T>(GUIUtility.GetControlID(focusType));
        }

        public static T Get<T>(int controlID) {
            return (T) GUIUtility.GetStateObject(typeof (T), controlID);
        }
    }

    public class GUILayoutScrollView : IDisposable {
        private readonly ScrollState scrollState = StateObject.Get<ScrollState>();

        public GUILayoutScrollView() {
            scrollState.position = GUILayout.BeginScrollView(scrollState.position);
        }

        public GUILayoutScrollView(GUIStyle style) {
            scrollState.position = GUILayout.BeginScrollView(scrollState.position, style);
        }

        public void Dispose() {
            GUILayout.EndScrollView();
        }
    }

    public class EditorGUILayoutScrollView : IDisposable {
        private readonly ScrollState scrollState = StateObject.Get<ScrollState>();

        public EditorGUILayoutScrollView() {
            scrollState.position = EditorGUILayout.BeginScrollView(scrollState.position);
        }

        public EditorGUILayoutScrollView(GUIStyle style) {
            scrollState.position = EditorGUILayout.BeginScrollView(scrollState.position, style);
        }

        public void Dispose() {
            EditorGUILayout.EndScrollView();
        }
    }

    public class DisposableHandleGUI : IDisposable {
        private static int _handleCount;

        public DisposableHandleGUI() {
            if (_handleCount == 0) {
                Handles.BeginGUI();
            }
            _handleCount++;
        }

        public void Dispose() {
            _handleCount--;

            if (_handleCount == 0) {
                Handles.EndGUI();
            }
        }
    }

    public static class Logger {
        public static void Log(params object[] args) {
            string[] strings = new string[args.Length];
            for (int i = 0; i < args.Length; i++) {
                object o = args[i];
                strings[i] = o + "";
            }
            Debug.Log(string.Join("   ", strings));
        }
    }

    public class RotatedGUI : DisposableHelper {
        private RotatedGUI(Vector2 position, Vector2 offset = new Vector2()) {
            disposables.Push(new CustomGUIMatrix(Matrix4x4.TRS(position + offset, Quaternion.identity, Vector3.one),
                                                 true));
        }

        public RotatedGUI(Vector2 position, float rotation, Vector2 offset = new Vector2())
            : this(position, offset) {
            GUIUtility.RotateAroundPivot(rotation, position);
        }

        public RotatedGUI(Vector2 position, Quaternion rotation, Vector2 offset = new Vector2())
            : this(position, offset) {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one)*GUI.matrix;
        }
    }

    public class MutableRect {
        public float x, y, width, height;
        public bool autoNewLine;

        public MutableRect(Rect r) {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public static implicit operator MutableRect(Rect r) {
            return new MutableRect(r);
        }

        public static implicit operator Rect(MutableRect r) {
            float y = r.y;
            if (r.autoNewLine) {
                r.y += r.height;
            }
            return new Rect(r.x, y, r.width, r.height);
        }
    }

    public class IndentRect : IDisposable {
        private readonly float offset;
        private readonly MutableRect rect;

        public IndentRect(MutableRect r, int i = 1) {
            rect = r;
            offset = i;
            offset = GUIHelpers.IndentMultiplier*i;
            rect.x += offset;
            rect.width -= offset;
        }

        public void Dispose() {
            rect.x -= offset;
            rect.width += offset;
        }
    }

    internal class ZeroIndent : IDisposable {
        private readonly int originalIndent;

        public ZeroIndent() {
            originalIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
        }

        public void Dispose() {
            EditorGUI.indentLevel = originalIndent;
        }
    }

    public class DisposableGUIEnabled : IDisposable {
        private readonly bool guiEnabled;

        public DisposableGUIEnabled(bool enabled) {
            guiEnabled = GUI.enabled;
            GUI.enabled = enabled;
        }

        public void Dispose() {
            GUI.enabled = guiEnabled;
        }
    }

    public class DisposableEditorGUIMixedValue : IDisposable {
        private readonly bool oldMixedValue;

        public DisposableEditorGUIMixedValue(bool mixedValue) {
            oldMixedValue = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = mixedValue;
        }

        public void Dispose() {
            EditorGUI.showMixedValue = oldMixedValue;
        }
    }
}
