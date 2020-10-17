using System.IO;
using System.Windows.Controls;

namespace Sprightly.WPF.Components
{
    /// <summary>
    /// Interaction logic for RecentProjectContent.xaml
    /// </summary>
    public partial class RecentProjectContent : UserControl
    {
        private readonly string path;
        private readonly System.DateTime lastOpened;

        public RecentProjectContent(string path, System.DateTime lastOpened)
        {
            this.path = path;
            this.lastOpened = lastOpened;
            this.DataContext = this;
            InitializeComponent();
        }

        public string FileName => Path.GetFileName(path);
        public string DirectoryPath => Path.GetDirectoryName(path);
        public string LastOpenedDate => $"{lastOpened.ToShortDateString()} {lastOpened.ToShortTimeString()}";
    }
}
