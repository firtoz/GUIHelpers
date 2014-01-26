using System;
using System.Collections;
using UnityEngine;

namespace toxicFork.GUIHelpers {
	public class GUIHelpers
	{
		public const int IndentMultiplier = 15;

		public static int EnumToIndex<T>(T value)
		{
			return new ArrayList(Enum.GetValues(typeof(T))).IndexOf(value);
		}

		/// <summary>
		/// Draws a square on the screen that is centered around the position.
		/// </summary>
		/// <param name="position">The center of the square on the screen</param>
		/// <param name="rotation">The rotation of the square. Does not work well with any rotation other than z.</param>
		/// <param name="size">The size of the square, in world coordinates.</param>
		public static void DrawSquare(Vector3 position, Quaternion rotation, float size)
		{
			float halfSize = size * .5f;
			Vector3 up = rotation * Vector3.up;
			Vector3 right = rotation * Vector3.right;
			Vector3 bottomLeft = position - up * halfSize - right * halfSize;
			GL.Begin(GL.QUADS);
			GL.TexCoord2(0, 0);
			GL.Vertex(bottomLeft);
			GL.TexCoord2(0, 1);
			GL.Vertex(bottomLeft + up * size);
			GL.TexCoord2(1, 1);
			GL.Vertex(bottomLeft + up * size + right * size);
			GL.TexCoord2(1, 0);
			GL.Vertex(bottomLeft + right * size);
			GL.End();
		}


		public static Vector2 Transform2DPoint(Transform transform, Vector2 point)
		{
			Vector2 scaledPoint = Vector2.Scale(point, transform.lossyScale);
			float angle = transform.rotation.eulerAngles.z;
			Vector2 rotatedScaledPoint = Quaternion.AngleAxis(angle, Vector3.forward) * scaledPoint;
			Vector2 translatedRotatedScaledPoint = (Vector2)transform.position + rotatedScaledPoint;
			return translatedRotatedScaledPoint;
		}

		public static Vector2 InverseTransform2DPoint(Transform transform, Vector2 translatedRotatedScaledPoint)
		{
			Vector2 rotatedScaledPoint = translatedRotatedScaledPoint - (Vector2)transform.position;
			float angle = transform.rotation.eulerAngles.z;
			Vector2 scaledPoint = Quaternion.AngleAxis(-angle, Vector3.forward) * rotatedScaledPoint;
			Vector2 point = Vector2.Scale(scaledPoint, new Vector2(1 / transform.lossyScale.x, 1 / transform.lossyScale.y));
			return point;
		}

		public static float GetAngle(Vector2 vector)
		{
			return Mathf.Rad2Deg * Mathf.Atan2(vector.y, vector.x);
		}

		public static Quaternion Rotate2D(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.forward);
		}

		public static Vector2 Rotated2DVector(float angle)
		{
			return Rotate2D(angle) * Vector3.right;
		}
	}

}