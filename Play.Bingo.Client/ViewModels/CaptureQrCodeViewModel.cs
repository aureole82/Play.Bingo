using System;
using System.Drawing;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;
using ZXing;

namespace Play.Bingo.Client.ViewModels
{
    public class CaptureQrCodeViewModel : ViewModelBase
    {
        private readonly IMessageService _messenger ;
        // Create a barcode reader instance.
        private readonly IBarcodeReader _reader = new BarcodeReader();

        #region Bindable properties and commands.

        private Bitmap _snapshot;

        public CaptureQrCodeViewModel()
        {
            // For design view only.
        }

        public CaptureQrCodeViewModel(IMessageService messenger)
        {
            _messenger = messenger;
        }

        public Bitmap Snapshot
        {
            get { return _snapshot; }
            set
            {
                _snapshot = value;
                RaisePropertyChanged();
                Decode(value);
            }
        }

        private void Decode(Bitmap bitmap)
        {
            if (bitmap == null) return;

            // Detect and decode the barcode inside the bitmap.
            var result = _reader.Decode(bitmap);
            // If the result is an QR code ...
            if (result == null || result.BarcodeFormat != BarcodeFormat.QR_CODE)
                return;

            // .. fetch the raw data and display the Bingo card.
            var binary = Convert.FromBase64String(result.Text);
            _messenger.Publish(new BingoCardViewModel(new BingoCardModel(binary), null));
        }

        #endregion
    }
}