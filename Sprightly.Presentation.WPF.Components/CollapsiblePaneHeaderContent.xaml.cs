using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Sprightly.Presentation.WPF.Components
{
    /// <summary>
    /// Interaction logic for CollapsiblePaneHeaderContent.xaml
    /// </summary>
    public partial class CollapsiblePaneHeaderContent : UserControl, INotifyPropertyChanged
    {
        private string _headerText;
        private string _panelIcon = "\uf0da";

        /// <summary>
        /// Creates a new <see cref="CollapsiblePaneHeaderContent"/>.
        /// </summary>
        public CollapsiblePaneHeaderContent()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string HeaderText
        {
            get => _headerText;
            set
            {
                if (value == _headerText)
                {
                    return;
                }

                _headerText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the panel icon.
        /// </summary>
        public string PanelIcon
        {
            get => _panelIcon;
            set
            {
                if (value == _panelIcon)
                {
                    return;
                }

                _panelIcon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        public void SetIsOpen(bool isOpen) =>
            PanelIcon = isOpen ? "\uf0d7" : "\uf0da";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
