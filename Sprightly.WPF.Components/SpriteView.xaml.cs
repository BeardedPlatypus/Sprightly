using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sprightly.WPF.Components
{
    /// <summary>
    /// Interaction logic for SpriteView.xaml
    /// </summary>
    public partial class SpriteView : UserControl
    {
        private bool _hasLoaded = false;
        private bool _hasInitialized = false;

        private ViewHost _viewHost;

        /// <summary>
        /// Creates a new <see cref="SpriteView"/>.
        /// </summary>
        public SpriteView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!_hasLoaded)
                _hasLoaded = true;

            // It is not guaranteed that the ViewportCanvas has a 
            // a valid size, as such we only initialize when we
            // have a valid size and it has not been initialised yet.
            // Otherwise we will do so on the first non-zero size
            // change.
            if (!_hasInitialized && IsValidSize(SpriteCanvas.ActualWidth, 
                                                SpriteCanvas.ActualHeight))
                InitializeViewport();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!_hasLoaded || _hasInitialized || !IsValidSize(e.NewSize))
                return;

            InitializeViewport();
        }

        private static bool IsValidSize(Size size) => IsValidSize(size.Width, size.Height);
        private static bool IsValidSize(double width, double height) => width > 0.0 && height > 0.0;

        private void InitializeViewport()
        {
            _viewHost = new ViewHost(SpriteCanvas.ActualWidth, SpriteCanvas.ActualHeight);
            SpriteCanvas.Child = _viewHost;

            _viewHost.MessageHook += new HwndSourceHook(ControlMsgFilter);

            _hasInitialized = true;
            SizeChanged -= OnSizeChanged;
        }

        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }
    }
}