using System.Windows.Controls;

namespace Sprightly.WPF.Components
{
    /// <summary>
    /// Interaction logic for SprightlyButtonContent.xaml
    /// </summary>
    public partial class SprightlyButtonContent : UserControl
    {
        public SprightlyButtonContent(string text)
        {
            InitializeComponent();
            Content.Text = text;
        }
    }
}
