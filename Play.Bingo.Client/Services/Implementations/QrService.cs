using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Play.Bingo.Client.ViewModels;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Play.Bingo.Client.Services.Implementations
{
    internal class QrService : IQrService
    {
        private readonly IBarcodeReader _reader = new BarcodeReader();

        private readonly IBarcodeWriter _writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions {ErrorCorrection = ErrorCorrectionLevel.Q}
        };

        public BitmapSource Encode(byte[] data)
        {
            var bitmap = Generate(data);
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            try
            {
                var size = rect.Width * rect.Height * 4;

                return BitmapSource.Create(
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

        public byte[] Decode(Bitmap qrCode)
        {
            if (qrCode == null) return null;

            // Detect and decode the barcode inside the bitmap.
            var result = _reader.Decode(qrCode);
            // If the result is an QR code ...
            if (result == null || result.BarcodeFormat != BarcodeFormat.QR_CODE)
                return null;

            // .. fetch the raw data and display the Bingo card.
            return Convert.FromBase64String(result.Text);
        }

        public Bitmap Generate(byte[] data)
        {
            return _writer.Write(Convert.ToBase64String(data));
        }
    }
}