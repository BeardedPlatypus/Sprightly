using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Sprightly.WPF.Components
{
    /// <summary>
    /// Interaction logic for RippleContainer.xaml
    /// </summary>
    public partial class RippleContainer : UserControl
    {
        public RippleContainer(object content, Color highlightColor)
        {
            InitializeComponent();

            ContentContainer.Content = content;

            var brush = new SolidColorBrush(highlightColor);
            Ellipse.Fill = brush;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddHandler(MouseDownEvent, new RoutedEventHandler((sender, e) =>
            {
                var targetHeight = Math.Max(ActualWidth, ActualHeight) * 2;
                var mousePosition = (e as MouseButtonEventArgs).GetPosition(this);
                var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);

                var animation = GridContainer.FindResource("EllipseAnimation") as Storyboard;

                //set initial margin to mouse position
                Ellipse.Margin = startMargin;
                //set the to value of the animation that animates the width to the target width
                (animation.Children[0] as DoubleAnimation).To = targetHeight;
                //set the to and from values of the animation that animates the distance relative to the container (grid)
                (animation.Children[1] as ThicknessAnimation).From = startMargin;
                (animation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - targetHeight / 2, mousePosition.Y - targetHeight / 2, 0, 0);
                Ellipse.BeginStoryboard(animation);
            }), true);
        }
	}
}
