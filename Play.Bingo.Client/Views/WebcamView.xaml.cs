using System.Drawing;
using System.Windows;
using WPFMediaKit.DirectShow.MediaPlayers;

namespace Play.Bingo.Client.Views
{
    /// <summary> Interaction logic for WebcamView.xaml. </summary>
    public partial class WebcamView
    {
        public static readonly DependencyProperty SnapshotProperty =
            DependencyProperty.Register("Snapshot", typeof (Bitmap), typeof (WebcamView));

        public WebcamView()
        {
            InitializeComponent();
        }

        public Bitmap Snapshot
        {
            get { return (Bitmap) GetValue(SnapshotProperty); }
            set { SetValue(SnapshotProperty, value); }
        }

        private void CaptureSnapshot(object sender, VideoSampleArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => Snapshot = e.VideoFrame); 
        }
    }
}