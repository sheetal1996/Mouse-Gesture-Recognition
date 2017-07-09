using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace MouseGestures {
  /// <summary>
  /// Provides data for MouseGesture events
  /// </summary>
  public class MouseGestureEventArgs:EventArgs {
    private MouseGesture gesture;

    /// <summary>
    /// Initializes new instance of MouseGestureEventArgs
    /// </summary>
    /// <param name="gesture">The gesture performed.</param>
    public MouseGestureEventArgs(MouseGesture mouseGesture):base() {
      gesture = mouseGesture;
    }

    /// <summary>
    /// The gesture performed.
    /// </summary>
    public MouseGesture Gesture {
      get {
          return gesture;
      }
    }
  }
}
