using Gtk;

namespace Recor;

public class AboutGui
{
    public static Box content = new(Orientation.Vertical,10);

    public static void Start()
    {
        var header = new Label("About");
        var authors = new Label("Author : Skvik360");
        var usingText = new Label("Used : GTK.Sharp, Ffmpeg, C#");
        var about = new Label("A simple application for recording videos and taking screenshots. Designed specifically for quick startup and easy recording.");
        var version = new Label($"Version : {Logic.version}");
        content.PackStart(header,false,true,5);
        content.PackStart(authors,false,true,0);
        content.PackStart(usingText,false,true,0);
        content.PackStart(about,true,true,5);
        content.PackStart(version,false,true,5);
        header.Xalign = 0;
        authors.Xalign = 0;
        usingText.Xalign = 0;
        about.Xalign = 0;
        about.Valign = (Align)1;
        version.Xalign = 0;
        about.LineWrap = true;
        about.MaxWidthChars = 10;
        header.Markup = "<span font-size=\"15000\"><b>About</b></span>";
    }
}