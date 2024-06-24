using System.Drawing.Drawing2D;

class CustomToolTip : ToolTip
{

    public int SIZE_X = 500;
    public int SIZE_Y = 50;

    public CustomToolTip()
    {
        this.OwnerDraw = true;
        this.Popup += new PopupEventHandler(this.OnPopup);
        this.Draw += new DrawToolTipEventHandler(this.OnDraw);
    }

    string m_EndSpecialText;
    Color m_EndSpecialTextColor = Color.Black;

    public Color EndSpecialTextColor
    {
        get { return m_EndSpecialTextColor; }
        set { m_EndSpecialTextColor = value; }
    }

    public string EndSpecialText
    {
        get { return m_EndSpecialText; }
        set { m_EndSpecialText = value; }
    }

    private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
    {
        e.ToolTipSize = new Size(SIZE_X, SIZE_Y);
    }


    private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
    {
        Graphics g = e.Graphics;

        LinearGradientBrush b = new LinearGradientBrush(e.Bounds,
            Color.AntiqueWhite, Color.LightCyan, 45f);

        g.FillRectangle(b, e.Bounds);

        g.DrawRectangle(new Pen(Brushes.Black, 1), new Rectangle(e.Bounds.X, e.Bounds.Y,
            e.Bounds.Width - 1, e.Bounds.Height - 1));


        System.Drawing.Size toolTipTextSize = TextRenderer.MeasureText(e.ToolTipText, e.Font);

        g.DrawString(e.ToolTipText, new Font(e.Font, FontStyle.Bold), Brushes.Black,
            new PointF((SIZE_X - toolTipTextSize.Width)/2, (SIZE_Y - toolTipTextSize.Height) / 2)); 

        b.Dispose();
    }
}