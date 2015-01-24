using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Play.Bingo.Client.Helper;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Play.Bingo.Client.ViewModels
{
    public class QrCodeViewModel : ViewModelBase
    {
        public QrCodeViewModel()
        {
            if (!IsInDesignMode) return;

            var binary = CardGenerator.Generate().ToBinary();
            GenerateCode(binary);
        }

        #region Bindable properties and commands.

        private BitmapSource _code;
        private byte[] _data;

        public BitmapSource Code
        {
            get { return _code; }
            set
            {
                if (Equals(_code, value)) return;
                _code = value;
                RaisePropertyChanged();
            }
        }

        public byte[] Data
        {
            get { return _data; }
            set
            {
                if (_data == value) return;
                _data = value;
                RaisePropertyChanged();
                GenerateCode(_data);
            }
        }

        #endregion

        #region Private helper methods.

        private void GenerateCode(byte[] binary)
        {
            var qrGenerator = new QrCodeGenerator();
            var qrCode = qrGenerator.CreateQrCode(Convert.ToBase64String(binary), QrCodeGenerator.EccLevel.Q);
            var bitmap = qrCode.GetGraphic(20);
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width*rect.Height)*4;

                Code = BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        #endregion
    }
}