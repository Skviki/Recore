using Gtk;
using static Recor.Logic;

namespace Recor;

public class UpdateGui
{
    public static Window wind = new Window("Update");
    public static Stack stack = new Stack();
    public static Box boxNoUpdate = new Box(Orientation.Vertical,10);
    public static Box boxUpdate = new Box(Orientation.Vertical,10);
    public static void Start()
    {
        
        wind.Decorated = false;
        wind.SetSizeRequest(200, 200);
        wind.SetPosition(WindowPosition.Center);
        wind.KeepAbove = true;
        wind.SetIconFromFile(FastPath("icon-update.svg"));
        
        stack.Margin = 15;
        
        var boxLoad = new Box(Orientation.Vertical,10);
        
        stack.Add(boxLoad);
        stack.Add(boxNoUpdate);
        stack.Add(boxUpdate);
        
        var spinner = new Spinner();
        spinner.Start();
        var text = new Label("Check...");
        
        boxLoad.PackStart(spinner,true,true,0);
        boxLoad.PackStart(text,false,true,0);

        var errorImg = new Image();
        errorImg.SetFromIconName("dialog-error", IconSize.Dialog);
        var textNoUpdate = new Label("No updates available");
        boxNoUpdate.PackStart(errorImg,true,true,0);
        boxNoUpdate.PackStart(textNoUpdate,false,true,0);
        
        var updateImg = new Image();
        updateImg.SetFromIconName("view-refresh", IconSize.Dialog);
        var textUpdate = new Label("Update available!");
        var btnGitHub = new Button("GitHub");
        boxUpdate.PackStart(updateImg,true,true,0);
        boxUpdate.PackStart(textUpdate,false,true,0);
        boxUpdate.PackStart(btnGitHub,false,true,0);
        
        
        wind.Add(stack);
        
        wind.ShowAll();

        LogicNet();
    }

    static async void LogicNet()
    {
        var client = new HttpClient();
        string vers = await client.GetStringAsync(new Uri("https://github.com/Skviki/Recore/raw/refs/heads/main/version"));
        if (vers == version)
        {
            stack.VisibleChild = boxNoUpdate;
        }
        else
        {
            stack.VisibleChild = boxUpdate;
        }
        wind.Decorated = true;
        wind.SetIconFromFile(FastPath("icon.svg"));
    }
}