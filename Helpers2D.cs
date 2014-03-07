using UnityEditor;
using UnityEngine;

namespace toxicFork.GUIHelpers {
    public class Helpers2D : Helpers {
        /// <summary>
        /// Transforms a 2D point into the selected object's local space.
        /// </summary>
        /// <param name="transform">The transform of the selected object.</param>
        /// <param name="point">The world coordinates of the point.</param>
        /// <returns>The point in the object's local space.</returns>
        public static Vector2 Transform2DPoint(Transform transform, Vector2 point)
        {
            Vector2 rotatedScaledPoint = Transform2DVector(transform, point);
            Vector2 translatedRotatedScaledPoint = (Vector2)transform.position + rotatedScaledPoint;
            return translatedRotatedScaledPoint;
        }

        /// <summary>
        /// Transforms a 2D vector into the selected object's local space.
        /// </summary>
        /// <param name="transform">The transform of the selected object.</param>
        /// <param name="vector">The world coordinates of the point.</param>
        /// <returns>The point in the object's local space.</returns>
        public static Vector2 Transform2DVector(Transform transform, Vector2 vector)
        {
            Vector2 scaledPoint = Vector2.Scale(vector, transform.lossyScale);
            float angle = transform.rotation.eulerAngles.z;
            Vector2 rotatedScaledPoint = Quaternion.AngleAxis(angle, Vector3.forward) * scaledPoint;
            return rotatedScaledPoint;
        }

        public static Vector2 InverseTransform2DPoint(Transform transform, Vector2 translatedRotatedScaledPoint)
        {
            Vector2 rotatedScaledVector = translatedRotatedScaledPoint - (Vector2)transform.position;
            return InverseTransform2DVector(transform, rotatedScaledVector);
        }

        public static Vector2 InverseTransform2DVector(Transform transform, Vector2 rotatedScaledVector)
        {
            float angle = transform.rotation.eulerAngles.z;
            Vector2 scaledPoint = Quaternion.AngleAxis(-angle, Vector3.forward) * rotatedScaledVector;
            Vector2 point = Vector2.Scale(scaledPoint, new Vector2(1 / transform.lossyScale.x, 1 / transform.lossyScale.y));
            return point;
        }

        public static float GetAngle2D(Vector2 vector)
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

        public static float DistanceToLine(Ray ray, Vector3 point)
        {
            return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
        }

        public static Vector2 Intersect2DPlane(Ray ray)
        {
            float d = Vector3.Dot(-ray.origin, Vector3.forward) / Vector3.Dot(ray.direction, Vector3.forward);
            return ray.GetPoint(d);
        }

        public static Vector2 GUIPointTo2DPosition(Vector2 position) {
            return Intersect2DPlane(HandleUtility.GUIPointToWorldRay(position));
        }
    }
}