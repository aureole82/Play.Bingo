using System.Drawing;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class CaptureQrCodeViewModel : ViewModelBase
    {
        private readonly IMessageService _messenger;
        private readonly IQrService _qrService;

        #region Bindable properties and commands.

        private Bitmap _snapshot;

        public CaptureQrCodeViewModel()
        {
            // For design view only.
        }

        public CaptureQrCodeViewModel(IMessageService messenger, IQrService qrService)
        {
            _messenger = messenger;
            _qrService = qrService;
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
            var data = _qrService.Decode(bitmap);
            if (data == null) return;

            _messenger.Publish(new BingoCardViewModel(new BingoCardModel(data), null, null));
        }

        #endregion
    }
}