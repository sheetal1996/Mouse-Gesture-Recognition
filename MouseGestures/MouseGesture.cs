using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace MouseGestures {

  /// <summary>
  /// Enum defining gesture directions.
  /// </summary>
  public enum MouseGestureDirection {
    Unknown,
    Up,
    Right,
    Down,
    Left
  }

  /// <summary>
  /// A MouseGesture is a sequence of movements
  /// </summary>
  public class MouseGesture
  {
    Point start;
    List<MouseGestureDirection> directions;

    /// <summary>
    /// Create a empty mouse gesture
    /// </summary>
    /// <param name="start">The point where the gesture started</param>
    public MouseGesture(Point point) {
      start = point;
      directions = new List<MouseGestureDirection>();
    }

    /// <summary>
    /// The point where the gesture started
    /// </summary>
    public Point Start
    {
      get {
        return start;
      }
    }

    /// <summary>
    /// Append a motion to the gesture
    /// </summary>
    /// <remarks>
    /// Duplicate and unknown motions will be filtered out.
    /// </remarks>
    public void AddSegment(MouseGestureDirection direction)
    {
      if (direction != MouseGestureDirection.Unknown &&
        ( directions.Count == 0 ||
          direction != directions[directions.Count - 1] ) )
        directions.Add(direction);
    }

    /// <summary>
    /// The number of motions in the gesture
    /// </summary>
    public int Count
    {
      get {
        return directions.Count;
      }
    }

		/// <summary>
		/// Gets the performed gesture
		/// </summary>
		public string Motions {
			get {
				return this.ToString();
			}
		}

    /// <summary>
    /// A string representation of the gesture
    /// </summary>
    public override string ToString()
    {
      string s = string.Empty;
      foreach (MouseGestureDirection d in directions)  {
        switch (d)  {
          case MouseGestureDirection.Left:
            s += 'L'; break;
          case MouseGestureDirection.Right:
            s += 'R'; break;
          case MouseGestureDirection.Up:
            s += 'U'; break;
          case MouseGestureDirection.Down:
            s += 'D'; break;
          case MouseGestureDirection.Unknown:
            break;
        }
      }
      return s;
    }
  }
}
