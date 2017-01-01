using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows;

namespace Play.Bingo.Client.Views
{
    /// <summary> Interaction logic for WebcamView.xaml. </summary>
    public partial class WebcamView
    {
        public static readonly DependencyProperty SnapshotProperty =
            DependencyProperty.Register("Snapshot", typeof(Bitmap), typeof(WebcamView));

        private Timer _timer;

        public WebcamView()
        {
            InitializeComponent();
        }

        public Bitmap Snapshot
        {
            get { return (Bitmap) GetValue(SnapshotProperty); }
            set { SetValue(SnapshotProperty, value); }
        }

        private void WebcamView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var videoCaptureDevices = WebCameraControl.GetVideoCaptureDevices();
            var device = videoCaptureDevices.FirstOrDefault();
            if (device == null) return;

            WebCameraControl.StartCapture(device);

            // Pin control to height/width of the camera input (and let the ViewBox scale).
            WebCameraBorder.Height = WebCameraControl.VideoSize.Height;
            WebCameraBorder.Width = WebCameraControl.VideoSize.Width;

            if (_timer == null)
            {
                _timer = new Timer
                {
                    Interval = 500
                };
                _timer.Elapsed += (o, args) =>
                        Application.Current.Dispatcher.Invoke(() => Snapshot = WebCameraControl.GetCurrentImage());
            }
            _timer.Start();
        }

        private void WebcamView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            WebCameraControl.StopCapture();
        }
    }
}