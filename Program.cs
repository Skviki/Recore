using Gtk;
using static Recor.Logic;

namespace Recor;

class Program
{
    public static Window root = new("Recore");
    
    static void Main(string[] args)
    {
        Application.Init();
        root.SetPosition(WindowPosition.Center);
        root.SetSizeRequest(600, 250);
        root.DeleteEvent += (o, eventArgs) => Application.Quit();
        root.SetIconFromFile(FastPath("icon.svg"));
        root.Resizable = false;

        var content = new VBox();
        content.Margin = 15;
        var stack = new Stack();
        
        stack.AddTitled(RecordGui.content,"video","Video");
        stack.AddTitled(ScreenshotGui.content,"screenshot","Screenshot");
        stack.AddTitled(AboutGui.content,"about","About");

        stack.TransitionDuration = 100;
        stack.TransitionType = StackTransitionType.Crossfade;
        
        var switcher = new StackSwitcher();
        switcher.Stack = stack;
        switcher.Halign = Align.Fill;
        switcher.MarginBottom = 10;
        
        content.PackStart(switcher,false,true,0);
        content.Add(stack);
        
        RecordGui.Start();
        ScreenshotGui.Start();
        AboutGui.Start();
        
        root.Add(content);
        root.ShowAll();
        Application.Run();
    }
}