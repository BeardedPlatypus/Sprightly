using System.Windows.Controls;

namespace Sprightly.Presentation.WPF.Components
{
    /// <summary>
    /// Interaction logic for SprightlyButtonContent.xaml
    /// </summary>
    public partial class SprightlyButtonContent : UserControl
    {
        public SprightlyButtonContent(string text)
        {
            InitializeComponent();
            ContentBox.Text = text;
        }
    }
}
