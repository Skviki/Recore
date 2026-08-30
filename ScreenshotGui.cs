using Gtk;

namespace Recor;

public class ScreenshotGui
{
    public static Box content = new(Orientation.Vertical, 10);
    
    public static void Start()
    {
        var qualCont = new HBox();
        ComboBoxText qual = new ComboBoxText();
        var qualText = new Label("Quality");
        qualText.Xalign = 0;
        qual.AppendText("1080");
        qual.AppendText("720");
        qual.AppendText("480");
        qual.Active = 0;
        qualCont.Add(qualText);
        qualCont.Add(qual);
        
        var btnShot = new Button("Screenshot");

        btnShot.Clicked += async (s, e) =>
        {
            Program.root.Hide();
            await Task.Delay(1000);
            Logic.Screenshot(int.Parse(qual.ActiveText));
            Program.root.Show();
        };
        
        content.Add(qualCont);
        content.Add(btnShot);
    }
}