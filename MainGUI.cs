
using System.ComponentModel;
using System.Diagnostics;
using MinecraftJukeboxPackCreator.controls;
using MinecraftJukeboxPackCreator.packcontents;

namespace MinecraftJukeboxPackCreator;

public partial class MainGUI : Form
{
    public MainGUI()
    {
        #region Drag&Drop Event Hooks
        AllowDrop = true;
        DragEnter += OnDragEnter;
        DragDrop += OnDragDrop;
        DragLeave += OnDragLeave;
        #endregion

        ClientSize = new Size(450, 450);
        Icon = new Icon(typeof(MinecraftJukeboxPackCreator).Assembly.GetManifestResourceStream("MinecraftJukeboxPackCreator.Icon.ico")!);
        BackColor = Color.FromArgb(220, 220, 220);
        Text = "Jukeþσcks";

        //☰ buger menu
        optionsButton = new Button
        {
            Text = "☰",
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ClientSize = new(40, 40),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new(ClientSize.Width - 40, 0)
        };
        optionsButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        optionsButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        optionsButton.FlatAppearance.BorderSize = 0;
        optionsButton.Click += OnClickOptions;
        Menu = new();
        Menu.Items.Add(text: "choose ffmpeg...", null, onClick: (sender, eventArgs) =>
        {
            var fileDialogue = new OpenFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(MinecraftJukeboxPackCreator.Settings.ffmpegPath),
                Filter = "ffmpeg executable | ffmpeg.exe",
                Title = "Choose an ffmpeg executable"
            };
            var result = fileDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                MinecraftJukeboxPackCreator.Settings.ffmpegPath = fileDialogue.FileName;
                Console.WriteLine($"set ffmpeg path to {fileDialogue.FileName}");
            }
        });
        Controls.Add(optionsButton);

        jukebox = new();
        Resize += (sender, args) => LayoutJukebox();
        LayoutJukebox();
        Controls.Add(jukebox);

        FileCrawler = new();
        FileCrawler.DoWork += (sender, args) => args.Result = CreatePack((args.Argument as string[])!);
        FileCrawler.RunWorkerCompleted += (sender, args) => SavePacks((JukeboxPacks)args.Result!);

        PackSaver.DoWork += (sender, args) =>
        {
            var packs = ((args.Argument as object[])![0] as JukeboxPacks)!;
            var path = ((args.Argument as object[])![1] as string)!;
            packs.SaveToDisk(path);
            args.Result = path;
        };
        PackSaver.RunWorkerCompleted += (_, args) =>
        {
            if (args.Error != null)
            {
                CustomToolTip notifyError = new();
                notifyError.Show(args.Error.Message, this, ClientSize.Width / 2 - notifyError.SIZE_X / 2 + 10, ClientSize.Height - notifyError.SIZE_Y / 2, 2000);
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = args.Result as string,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            OnPackSaved();
        };
    }
    ContextMenuStrip Menu;
    void OnClickOptions(object? sender, EventArgs eventArgs)
    {
        Menu.Show(optionsButton, -125, 0);
    }

    void OnDragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = DragDropEffects.Copy;
        jukebox.DiscInserted = true;
    }
    void OnDragLeave(object? sender, EventArgs e)
    {
        jukebox.DiscInserted = false;
    }

    private void OnDragDrop(object? sender, DragEventArgs e)
    {
        var assets = (string[])(e.Data?.GetData(DataFormats.FileDrop) ?? new string[0]);
        jukebox.Processing = true;
        AllowDrop = false;
        FileCrawler.RunWorkerAsync(assets);
    }
    BackgroundWorker FileCrawler = new();
    BackgroundWorker PackSaver = new();
    private JukeboxPacks CreatePack(string[] paths)
    {
        return JukeboxPacks.FromPaths(paths);
    }

    private void SavePacks(JukeboxPacks packs)
    {
        FolderBrowserDialog fbd = new()
        {
            InitialDirectory = Application.StartupPath,
            ShowNewFolderButton = true,
        };
        var result = fbd.ShowDialog();
        var path = fbd.SelectedPath;
        if (result == DialogResult.OK || result == DialogResult.Yes || result == DialogResult.Continue)
            PackSaver.RunWorkerAsync(new object[] { packs, path });
        else
        {
            OnPackSaved();
        }
    }

    private void OnPackSaved()
    {
        jukebox.Processing = false;
        jukebox.DiscInserted = false;
        AllowDrop = true;
    }

    JukeboxControl jukebox;
    private Button optionsButton;

    private void LayoutJukebox()
    {
        jukebox.ClientSize = new Size(1, 2) * Math.Min(ClientSize.Height, ClientSize.Width);
        jukebox.Location = new(ClientSize.Width / 2 - jukebox.ClientSize.Width / 2, ClientSize.Height - jukebox.ClientSize.Height * 3 / 4);
    }
}
