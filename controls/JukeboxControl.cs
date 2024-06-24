using System.Drawing.Imaging;

namespace MinecraftJukeboxPackCreator.controls
{
    public class JukeboxControl : PictureBox
    {
        public bool DiscInserted { get; set; } = false;
        public bool Processing { get; set; } = false;
        private readonly int frameCount;
        private float AnimTarget => DiscInserted ? 1 : 0;
        private float currentAnim = 0f;
        private int CurrentFrame => Math.Clamp((int)((DiscInserted ? currentAnim : (1 - currentAnim)) * frameCount), 0, frameCount - 1);

        AnimState state = AnimState.Idle;
        enum AnimState
        {
            Idle,
            Processing,
        }

        public JukeboxControl()
        {
            var assembly = typeof(JukeboxControl).Assembly;
            var jukeboxGif = Image.FromStream(assembly.GetManifestResourceStream("MinecraftJukeboxPackCreator.Jukebox.gif")!);
            var jukeboxGifReverse = Image.FromStream(assembly.GetManifestResourceStream("MinecraftJukeboxPackCreator.Jukebox_Reverse.gif")!);
            var jukeboxGifSpin = Image.FromStream(assembly.GetManifestResourceStream("MinecraftJukeboxPackCreator.Jukebox_Spin.gif")!);
            var frameDimension = new FrameDimension(jukeboxGif.FrameDimensionsList[0]);
            frameCount = jukeboxGif.GetFrameCount(frameDimension);
            BackgroundImageLayout = ImageLayout.Zoom;

            var t = new System.Windows.Forms.Timer
            {
                Interval = 1000 / 30
            };
            t.Tick += (sender, args) =>
            {
                switch (state)
                {
                    case AnimState.Idle:
                        if (Processing && currentAnim == 1f)
                            state = AnimState.Processing;
                        break;
                    case AnimState.Processing:
                        if (!Processing)
                            state = AnimState.Idle;
                        break;
                }

                currentAnim += state switch
                {
                    AnimState.Processing => 1f / t.Interval,
                    AnimState.Idle => Math.Sign(AnimTarget - currentAnim) * 2f / t.Interval
                };
                currentAnim = state switch
                {
                    AnimState.Processing => currentAnim % 1f,
                    AnimState.Idle => Math.Clamp(currentAnim, 0f, 1f),
                };

                BackgroundImage = state switch
                {
                    AnimState.Processing => jukeboxGifSpin,
                    AnimState.Idle => DiscInserted ? jukeboxGif : jukeboxGifReverse
                };
                BackgroundImage.SelectActiveFrame(frameDimension, CurrentFrame);
                Refresh();
            };
            t.Start();
        }
    }
}