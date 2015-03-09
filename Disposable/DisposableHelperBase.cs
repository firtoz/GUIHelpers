using System;
using System.Collections.Generic;

namespace toxicFork.GUIHelpers.Disposable {
	public abstract class DisposableHelperBase : IDisposable {
		protected enum StackDisposeStrategy {
			BeforeCleanup,
			AfterCleanup
		}

		protected readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
		protected StackDisposeStrategy stackDisposeStrategy = StackDisposeStrategy.AfterCleanup;

		public void Dispose() {
			var beforeCleanup = stackDisposeStrategy == StackDisposeStrategy.BeforeCleanup;
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

		public virtual void Cleanup() {}
	}
}