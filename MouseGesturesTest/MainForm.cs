using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MouseGestures;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace MouseGesturesTest {
  public partial class MainForm : Form {
    private PictureBox[] mouseGestureDisplay;

    public MainForm() {
      InitializeComponent();

      mouseGestureDisplay = new PictureBox[18];

			pbUp.Tag = "U";
			pbRight.Tag = "R";
			pbDown.Tag = "D";
			pbLeft.Tag = "L";
			pbUpRight.Tag = "UR";
			pbUpDownUp.Tag = "UDU";
			pbUpLeft.Tag = "UL";
			pbRightUp.Tag = "RU";
			pbRightDown.Tag = "RD";
			pbUpRightDownLeft.Tag = "URDL";
			pbDownUp.Tag = "DU";
			pbDownRight.Tag = "DR";
			pbDownLeft.Tag = "DL";
			pbLeftUp.Tag = "LU";
			pbUpDownUpDown.Tag = "UDUD";
			pbLeftDown.Tag = "LD";
            pbRightDownRight.Tag = "RDR";
            pbDownUpDownUp.Tag = "DUDU";

      mouseGestureDisplay[0] = pbUp;
      mouseGestureDisplay[1] = pbRight;
      mouseGestureDisplay[2] = pbDown;
      mouseGestureDisplay[3] = pbLeft;

      mouseGestureDisplay[4] = pbUpRight;
      mouseGestureDisplay[5] = pbUpDownUp;
      mouseGestureDisplay[6] = pbUpLeft;

      mouseGestureDisplay[7] = pbRightUp;
      mouseGestureDisplay[8] = pbRightDown;
      mouseGestureDisplay[9] = pbUpRightDownLeft;

      mouseGestureDisplay[10] = pbDownUp;
      mouseGestureDisplay[11] = pbDownRight;
      mouseGestureDisplay[12] = pbDownLeft;

      mouseGestureDisplay[13] = pbLeftUp;
      mouseGestureDisplay[14] = pbUpDownUpDown;
      mouseGestureDisplay[15] = pbLeftDown;
      mouseGestureDisplay[16] = pbRightDownRight;
      mouseGestureDisplay[17] = pbDownUpDownUp;
        }

    private void timerReset_Tick(object sender, EventArgs e) {
      //resets images to normal
      ResetImages();
      timerReset.Stop();
    }

    private void ResetImages() {
      foreach ( PictureBox pb in mouseGestureDisplay ) {
                if (Assembly.GetExecutingAssembly().GetManifestResourceStream(
          getResourceName((string)pb.Tag, false)) != null)
                {
                    pb.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                      getResourceName((string)pb.Tag, false)));
                }
      }
    }

    //Build full name of the resource
		private string getResourceName(string gestureName, bool activeImage) {
      string active = "";
      if ( activeImage )
        active = "A";

      return "MouseGesturesTest.Images." + gestureName + active + ".png";
    }

    private void mouseGesturesTest_Gesture(object sender, MouseGestureEventArgs e) {
      foreach ( PictureBox pb in mouseGestureDisplay ) {
        if ( ( string )pb.Tag == e.Gesture.ToString() ) {
          //load image from the resources
          pb.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(
    getResourceName(e.Gesture.Motions, true)));
          timerReset.Start();
          string move = (string)pb.Tag;
          switch(move)
          {
                        case "U":
                            Process.Start("https://www.google.co.in/search?rlz=1C1CHBF_enIN725IN725&q=weather&oq=weather&gs_l=serp.3..0i67k1l10.30821.32822.0.35836.7.7.0.0.0.0.374.1235.0j5j1j1.7.0....0...1.1.64.serp..0.7.1231...0j0i131k1.45U3X1pvxY0");
                            break;
                        case "D":
                            Process.Start("C:/Program Files/Android/Android Studio/bin/studio64.exe");
                            break;
                        case "RDR":
                            Process.Start("https://www.zomato.com/india");
                            break;
                        case "R":
                            Process.Start("C:/Program Files (x86)/Git/cmd/git-gui.exe");
                            break;
                        case "L":
                            Process.Start("wmplayer.exe");
                            break;
                        case "UDU":
                            Process.Start("C:/Program Files (x86)/Notepad++/notepad++.exe");
                            break;
                        case "UDUD":
                            Process.Start("C:/Program Files (x86)/Mission Planner/MissionPlanner.exe");
                            break;
                        case "DUDU":
                            Process.Start("C:/Program Files/Windows NT/Accessories/wordpad.exe");
                            break;
                        case "DU":
                            Process.Start("C:/Program Files (x86)/Microsoft Visual Studio/2017/Community/Common7/IDE/devenv.exe");
                            break;
                        case "URDL":
                            Process.Start("mspaint.exe");
                            break;
                        case "UR":
                            Process.Start("control.exe");
                            break;
                        case "UL":
                            Process.Start("https://mail.google.com/");
                            break;
                        case "RU":
                            Process.Start("https://www.youtube.com/");
                            break;
                        case "LU":
                            Process.Start("https://auth.udacity.com/sign-in?next=https%3A%2F%2Fclassroom.udacity.com%2Fauthenticated");
                            break;
                        case "RD":
                            Process.Start("https://github.com/sheetal1996");
                            break;
                        case "LD":
                            Process.Start("explorer.exe");
                            break;
                        case "DR":
                            Process.Start("https://in.linkedin.com/");
                            break;
                        case "DL":
                            Process.Start("calc.exe");
                            break;
                        default:
                            break;

          }

          return;
        }
      }
    }

    private void MainForm_Load(object sender, EventArgs e) {
      ResetImages();
    }


  }
}