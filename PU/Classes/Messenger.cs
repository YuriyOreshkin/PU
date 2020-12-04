using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PU.Classes
{
    public enum AlertType
    {
        Success,
        Info,
        Error
    }

    public static class Messenger
    {
        public static void showAlert(AlertType type, string CaptionText, string ContentText, string ThemeName, int Height = 150)
        {
            Telerik.WinControls.UI.RadDesktopAlert alert = new Telerik.WinControls.UI.RadDesktopAlert { ThemeName = ThemeName, FadeAnimationFrames = 60, PopupAnimationFrames = 30, Opacity = 0.9F, PopupAnimationDirection = Telerik.WinControls.UI.RadDirection.Up, ShowOptionsButton = false, FixedSize = new Size(350, Height) };
            //alert.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.Red;
            alert.CaptionText = CaptionText;
            alert.ContentText = ContentText;
            alert.Show();
        }
    }
}
