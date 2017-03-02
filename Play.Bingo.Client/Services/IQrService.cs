using System.Drawing;
using System.Windows.Media.Imaging;

namespace Play.Bingo.Client.ViewModels
{
    public interface IQrService
    {
        BitmapSource Encode(byte[] data);
        byte[] Decode(Bitmap qrCode);
    }
}