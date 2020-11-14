using System.Windows.Controls;

namespace Sprightly.WPF.Components
{
    /// <summary>
    /// Interaction logic for SprightlyIconButtonHorizontalContent.xaml
    /// </summary>
    public partial class SprightlyIconButtonHorizontalContent : UserControl
    {
        public SprightlyIconButtonHorizontalContent(string icon, string text, double iconFontSize)
        {
            InitializeComponent();
            IconBlock.Text = icon;
            IconBlock.FontSize = iconFontSize;
            TextBlock.Text = text;
        }
    }
}
