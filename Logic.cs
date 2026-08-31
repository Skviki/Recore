using System.Diagnostics;

namespace Recor;

public class Logic
{
    private static Process? ffmpegProcess;

    public static string version = File.ReadAllText(FastPath("version"));

    public static void Record(
        int resolution,
        int fps,
        int bitrate,
        string format,
        bool cursor,
        bool microphone,
        bool systemAudio)
    {
        if (ffmpegProcess is { HasExited: false })
            return;

        string ffmpeg = FastPath("ffmpeg");
        string output = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
            $"Recording_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{format}");

        var args = new List<string>
        {
            "-y",
            "-f", "x11grab",
            "-framerate", fps.ToString(),
            "-draw_mouse", cursor ? "1" : "0",
            "-i", ":0.0"
        };

        if (microphone)
            args.AddRange(["-f", "pulse", "-i", "default"]);

        if (systemAudio)
            args.AddRange(["-f", "pulse", "-i", "default.monitor"]);

        args.AddRange([
            "-map", "0:v:0",
            "-vf", $"scale=-2:{resolution}",
            "-c:v", "libx264",
            "-preset", "veryfast",
            "-b:v", $"{bitrate}k",
            "-pix_fmt", "yuv420p"
        ]);

        if (microphone && systemAudio)
        {
            args.AddRange([
                "-filter_complex",
                "[1:a][2:a]amix=inputs=2:duration=longest:dropout_transition=2[a]",
                "-map", "[a]"
            ]);
        }
        else if (microphone || systemAudio)
        {
            args.AddRange(["-map", "1:a:0"]);
        }

        if (microphone || systemAudio)
        {
            args.AddRange([
                "-c:a", "aac",
                "-b:a", "160k",
                "-ar", "48000"
            ]);
        }

        args.Add(output);

        ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffmpeg,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardError = true
            }
        };

        foreach (var arg in args)
            ffmpegProcess.StartInfo.ArgumentList.Add(arg);

        ffmpegProcess.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Console.WriteLine($"[FFmpeg] {e.Data}");
        };

        try
        {
            ffmpegProcess.Start();
            ffmpegProcess.BeginErrorReadLine();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            ffmpegProcess.Dispose();
            ffmpegProcess = null;
        }
    }

    public static void Stop()
    {
        if (ffmpegProcess == null)
            return;

        if (!ffmpegProcess.HasExited)
        {
            ffmpegProcess.StandardInput.WriteLine("q");
            ffmpegProcess.StandardInput.Flush();
            ffmpegProcess.WaitForExit();
        }

        ffmpegProcess.Dispose();
        ffmpegProcess = null;
    }
    
    public static void Screenshot(int resolution)
    {
        string output = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            $"Screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");

        string ffmpeg = FastPath("ffmpeg");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffmpeg,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.StartInfo.ArgumentList.Add("-f");
        process.StartInfo.ArgumentList.Add("x11grab");
        process.StartInfo.ArgumentList.Add("-video_size");
        process.StartInfo.ArgumentList.Add("1920x1080");
        process.StartInfo.ArgumentList.Add("-i");
        process.StartInfo.ArgumentList.Add(":0.0");
        process.StartInfo.ArgumentList.Add("-frames:v");
        process.StartInfo.ArgumentList.Add("1");
        process.StartInfo.ArgumentList.Add("-vf");
        process.StartInfo.ArgumentList.Add($"scale=-2:{resolution}");
        process.StartInfo.ArgumentList.Add(output);

        process.Start();
        process.WaitForExit();
    }

    public static string FastPath(string path) =>
        Path.Combine(AppContext.BaseDirectory, path.TrimStart('/', '\\'));
}