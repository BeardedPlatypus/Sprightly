using System.Windows.Controls;

namespace Sprightly.Presentation.WPF.Components
{
    /// <summary>
    /// Interaction logic for SprightlyIconButtonVerticalContent.xaml
    /// </summary>
    public partial class SprightlyIconButtonVerticalContent : UserControl
    {
        public SprightlyIconButtonVerticalContent(string icon, string text, double iconFontSize)
        {
            InitializeComponent();
            IconBlock.Text = icon;
            IconBlock.FontSize = iconFontSize;
            TextBlock.Text = text;
        }
    }
}
