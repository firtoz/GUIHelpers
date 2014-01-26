using UnityEngine;

namespace toxicFork.GUIHelpers {
	internal interface IGUIDrawer {
		void Draw(int controlID, Vector2 position, float size, float rotation);

		float GetDistance(Vector2 position, float size, float rotation);
	}
}