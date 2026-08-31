using Gtk;

namespace Recor;

public class RecordGui
{
    public static Box content = new Box(Orientation.Horizontal,10);
    
    public static void Start()
    {
        var btnRecord = new ToggleButton("Record");
        btnRecord.SetSizeRequest(200,-1);
        VBox option =  new VBox();
        option.Spacing = 10;
        
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

        var fpsCont = new HBox();
        ComboBoxText fps = new ComboBoxText();
        var fpsText = new Label("FPS");
        fpsText.Xalign = 0;
        fps.AppendText("60");
        fps.AppendText("45");
        fps.AppendText("30");
        fps.AppendText("15");
        fps.Active = 0;
        fpsCont.Add(fpsText);
        fpsCont.Add(fps);
        
        var formatCont = new HBox();
        var format = new ComboBoxText();
        var formatText = new Label("Format");
        formatText.Xalign = 0;
        format.AppendText("mp4");
        format.AppendText("mkv");
        format.AppendText("mov");
        format.AppendText("avi");
        format.Active = 3;
        formatCont.Add(formatText);
        formatCont.Add(format);
        
        var bitCont = new HBox();
        Scale bit = new Scale(Orientation.Horizontal, 1000 , 10000 , 1000);
        var bitText = new Label("Bitrate");
        bitText.Xalign = 0;
        bitText.MarginEnd = 50;
        bitCont.PackStart(bitText, false, true, 0);
        bitCont.Add(bit);

        var cursorCout = new HBox();
        Switch cursor =  new Switch();
        var cursorText = new Label("Cursor");
        cursorText.Xalign = 0;
        cursorCout.Add(cursorText);
        cursorCout.PackStart(cursor, false, true, 0);
        
        var microphoneCont = new HBox();
        var microphoneText = new Label("Microphone");
        microphoneText.Xalign = 0;
        var microphone = new Switch();
        microphoneCont.Add(microphoneText);
        microphoneCont.PackStart(microphone, false,true, 0);
        
        var audioCont = new HBox();
        var audioText = new Label("Audio");
        audioText.Xalign = 0;
        var audio = new Switch();
        audioCont.Add(audioText);
        audioCont.PackStart(audio, false,true, 0);

        btnRecord.Clicked += (sender, args) =>
        {
            if (btnRecord.Active)
            {
                Program.root.Iconify();
                btnRecord.Label = "Stop";
                Logic.Record(
                    fps: int.Parse(fps.ActiveText),
                    resolution: int.Parse(qual.ActiveText),
                    bitrate: (int)bit.Value,
                    format: format.ActiveText,
                    cursor: cursor.Active,
                    microphone: microphone.Active,
                    systemAudio: audio.Active
                );
            }
            else
            {
                btnRecord.Label = "Record";
                Logic.Stop();
            }
        };
        
        option.Add(qualCont);
        option.Add(fpsCont);
        option.Add(formatCont);
        option.Add(bitCont);
        option.Add(cursorCout);
        option.Add(microphoneCont);
        option.Add(audioCont);
        
        content.PackStart(btnRecord, false, true, 0);
        content.PackStart(option, true, true, 10);
    }
}