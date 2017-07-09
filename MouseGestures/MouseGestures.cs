using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseGestures {
  [ToolboxBitmap(typeof(MouseGestures), "ComponentIcon")]
  public partial class MouseGestures : Component {
    
    /// <summary>
    /// Maximal ratio of unknown MouseMuveSegments it the gesture
    /// </summary>
    //TODO: consider moving the hardcoded value to a property
    private const float maxUnknownSkipRatio = 0.35F;
    
    /// <summary>
    /// Minimal length of MouseMoveSegment
    /// </summary>
    //TODO consider moving the hardcoded value to a property
    private const uint mouseMoveSegmentLength = 8;

    /// <summary>
    /// Defines maximum angle error in degrees. If the angle error is greater then
    /// maxAngleError then the direction is not recognized.
    /// </summary>
    /// <remarks>
    /// It must be positive number lesser then 45
    /// </remarks>
    //TODO: consider moving the hardcoded value to a propery
    private const double maxAngleError = 30;

    private IMouseMessageFilter mf;
    private MouseGesture gesture;
    private double distance;

    private Point lastPoint;

    #region Properties

    #region Enabled
    /// <summary>
    /// Gets or sets propery indicating whether component is enabled and
    /// will recognize mouse gestures
    /// </summary>
    [DefaultValue(true)] 
    public bool Enabled {
      get {
        return enabled;
      }
      set {
        enabled = value;
        mf.Enabled = value;
      }
    }
    private bool enabled = true;
    #endregion

    #region Working
    /// <summary>
    /// Gets value indicating whether component is capturing and recognizing
    /// mouse gesture
    /// </summary>

    [Category("Mouse Gestures")]
    public bool Working {
      get {
        return gesture != null;
      }
    }
    #endregion

    #region MinGestureSize
    /// <summary>
    /// Gets or sets minimal gesture size in pixels
    /// </summary>
    [DefaultValue(30),
    Category("Mouse Gestures")]
    public int MinGestureSize {
      get {
        return minGestureSize;
      }
      set {
        if(value >0)
        minGestureSize = value;
      }
    }
    private int minGestureSize = 30;
    #endregion

    #endregion

    #region Constructors
    /// <summary>
    /// Creates MouseGestures component
    /// </summary>
    /// <param name="useLLMessageFilter">Specifies whether use LLMessageFilter</param>
    public MouseGestures(bool useLLMessageFilter) {
      InitializeComponent();
      InitializeMessageFilter(useLLMessageFilter);
    }
    

    /// <summary>
    /// Generic constructor used by Visual Studio
    /// </summary>
    /// <param name="container"></param>
    public MouseGestures(IContainer container) {
      container.Add(this);

      InitializeComponent();

      //for standard forms alway use ManagedMouseFilter
      InitializeMessageFilter(false);   
    }
    
    #endregion

    /// <summary>
    /// Installs MouseMessageFilter and hooks it's events
    /// </summary>
    private void InitializeMessageFilter(bool useLLMessageFilter) {
      if ( useLLMessageFilter ) {
        mf = new LLMouseFilter();
      }
      else {
        mf = new ManagedMouseFilter();
      }

      mf.Enabled = enabled;
      mf.RightButtonDown += new MouseFilterEventHandler(BeginGesture);
      mf.MouseMove += new MouseFilterEventHandler(AddToGesture);
      mf.RightButtonUp += new MouseFilterEventHandler(EndGesture);
    }

    #region Destructors
    ~MouseGestures() {
      
    }
    #endregion

    #region Helper Functions
    /// <summary>
    /// Calculates distance between 2 points
    /// </summary>
    /// <param name="p1">First point</param>
    /// <param name="p2">Second point</param>
    /// <returns>Distance between two points</returns>
    private static double GetDistance(Point p1, Point p2) {
      int dx = p1.X - p2.X;
      int dy = p1.Y - p2.Y;

      return Math.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Recognizes direction between two points
    /// </summary>
    /// <param name="deltaX">Lenght of movement in the horizontal direction</param>
    /// <param name="deltaY">Lenght of movement in the vertical direction</param>
    /// <returns>Gesture direction, if fails returns MouseGestureDirection.Unknown</returns>
    private static MouseGestureDirection GetDirection(Point start, Point end)
    {
        int deltaX = end.X - start.X;
        int deltaY = end.Y - start.Y;

        double length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        double sin = deltaX / length;
        double cos = deltaY / length;

        double angle = Math.Asin(Math.Abs(sin)) * 180 / Math.PI;

        if ((sin >= 0) && (cos < 0))
            angle = 180 - angle;
        else if ((sin < 0) && (cos < 0))
            angle = angle + 180;
        else if ((sin < 0) && (cos >= 0))
            angle = 360 - angle;

        //direction recognition
        if ((angle > 360 - maxAngleError) || (angle < 0 + maxAngleError))
            return MouseGestureDirection.Down;
        else if ((angle > 90 - maxAngleError) && (angle < 90 + maxAngleError))
            return MouseGestureDirection.Right;
        else if ((angle > 180 - maxAngleError) && (angle < 180 + maxAngleError))
            return MouseGestureDirection.Up;
        else if ((angle > 270 - maxAngleError) && (angle < 270 + maxAngleError))
            return MouseGestureDirection.Left;
        else return MouseGestureDirection.Unknown;
    }
    #endregion


    #region Mouse Events Handling
    /// <summary>
    /// Starts new mouse gesture
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Mouse event data</param>
    /// <remarks>
    /// Functions is called on the RightButtonDown event of MouseMessageFilter.
    /// </remarks>
    public void BeginGesture(object sender, EventArgs e) {
      lastPoint = Cursor.Position;
      gesture = new MouseGesture(lastPoint);
      distance = 0;
    }

    /// <summary>
    /// Adds MouseMoveSegment to the current gesture.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Mouse event data</param>
    /// <remarks>
    /// Function is called on the MouseMoveSegment of MouseMessageFilter
    /// The segment is added only when segment length is greater then 
    /// mouseMoveSegmentLength
    /// </remarks>
    public void AddToGesture(object sender, EventArgs e) {
      if ( gesture != null ) {
        Point point = Cursor.Position;
        double segmentDistance = GetDistance(lastPoint, point);
        if ( segmentDistance >= mouseMoveSegmentLength ) {
          gesture.AddSegment(GetDirection(lastPoint, point));
          distance += segmentDistance;
          lastPoint = point;
        }
      }
    }

    /// <summary>
    /// Stops mouse gesture recording and tries to recognize the gesture
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Mouse event data</param>
    public void EndGesture(object sender,  EventArgs e) {
      //check minimal length
      //TODO change minimal length checking  - does not work for gesture LeftRight, etc...
      if ( distance < minGestureSize || gesture.Count == 0 ) {
        //too short for mouse gesture - send regular right mouse click
        mf.Enabled = false;
        WinAPI.MouseInputEmulation.SendRightMouseClick();
        Application.DoEvents();
        mf.Enabled = true;
      } else {
        GestureHandler temp = Gesture;
        if ( temp != null ) {
          MouseGestureEventArgs args = new MouseGestureEventArgs(gesture);
          temp(this, args);
        }
      }
      gesture = null;
    }
    #endregion

    #region MouseGesture Event

    /// <summary>
    /// Represents the method that will handle MouseGesture events.
    /// </summary>
    /// <param name="sender">The source of event.</param>
    /// <param name="start">A MouseGestureEventArgs that contains event data.</param>
    public delegate void GestureHandler(object sender, MouseGestureEventArgs e);

    /// <summary>
    /// Occurs whether valid mouse gesture is performed.
    /// </summary>
    public event GestureHandler Gesture;

    #endregion

  }
}
