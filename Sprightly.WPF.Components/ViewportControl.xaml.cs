using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sprightly.WPF.Components
{
    /// <summary>
    /// Interaction logic for ViewportControl.xaml
    /// </summary>
    public partial class ViewportControl: UserControl
    {
        private bool _hasLoaded = false;
        private bool _hasInitialized = false;

        private ViewportHost _viewportHost;
        private readonly IViewport _viewport;

        /// <summary>
        /// Creates a new <see cref="ViewportControl"/>.
        /// </summary>
        public ViewportControl(IViewport viewport)
        {
            InitializeComponent();
            _viewport = viewport;
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
            if (!_hasInitialized && IsValidSize(ViewportCanvas.ActualWidth, 
                                               ViewportCanvas.ActualHeight))
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
            _viewportHost = new ViewportHost(ViewportCanvas.ActualWidth, ViewportCanvas.ActualHeight, _viewport);
            ViewportCanvas.Child = _viewportHost;

            _viewportHost.MessageHook += new HwndSourceHook(ControlMsgFilter);

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