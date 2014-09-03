using System;
using System.Collections;
using toxicFork.GUIHelpers.DisposableGL;
using UnityEngine;

namespace toxicFork.GUIHelpers {
    public class Helpers {
        public const int IndentMultiplier = 15;

        public static int EnumToIndex<T>(T value) {
            return new ArrayList(Enum.GetValues(typeof (T))).IndexOf(value);
        }

        public static void DrawSquare(Vector3 position, Quaternion rotation, float size, Material material)
        {
            using (new GLMatrix())
            {
                for (int i = 0; i < material.passCount; i++)
                {
                    material.SetPass(i);
                    DrawSquare(position, rotation, size);
                }
            }
        }

        //https://harthur.github.io/brain/
        public static Color YIQ(Color color) {
            float r = color.r *255;
            float g = color.g*255;
            float b = color.b*255;
            float yiq = (r*299 + g*587 + b*114) / 1000f;
            return (yiq >= 128) ? Color.black : Color.white;
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
            GL.Color(Color.white);
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


        private static Material _guiMaterial;

        public static Material GUIMaterial
        {
            get
            {
                return _guiMaterial ??
                       (_guiMaterial =
                           new Material(Shader.Find("GUI Helpers/GUI")) { hideFlags = HideFlags.HideAndDontSave });
            }
        }
        private static Material _vertexGUIMaterial;

        public static Material VertexGUIMaterial
        {
            get
            {
                return _vertexGUIMaterial ??
                       (_vertexGUIMaterial =
                           new Material(Shader.Find("GUI Helpers/GUI (Vertex)")) { hideFlags = HideFlags.HideAndDontSave });
            }
        }
        private static Material _alwaysVisiblevertexGUIMaterial;

        public static Material AlwaysVisibleVertexGUIMaterial
        {
            get
            {
                return _alwaysVisiblevertexGUIMaterial ??
                       (_alwaysVisiblevertexGUIMaterial =
                           new Material(Shader.Find("GUI Helpers/GUI (Vertex, Always Visible)")) { hideFlags = HideFlags.HideAndDontSave });
            }
        }

        private static Material _alwaysVisibleGUIMaterial;

        public static Material AlwaysVisibleGUIMaterial
        {
            get
            {
                return _alwaysVisibleGUIMaterial ??
                       (_alwaysVisibleGUIMaterial =
                           new Material(Shader.Find("GUI Helpers/GUI (Always Visible)")) { hideFlags = HideFlags.HideAndDontSave });
            }
        }
    }
}