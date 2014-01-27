using System;
using UnityEngine;

namespace toxicFork.GUIHelpers.DisposableGL
{
	class GLMatrix : IDisposable
	{
        public GLMatrix()
        {
            GL.PushMatrix();
	    }

	    public void Dispose() {
            GL.PopMatrix();
	    }
	}

}
