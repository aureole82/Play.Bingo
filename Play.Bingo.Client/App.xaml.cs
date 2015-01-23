using Play.Bingo.Client.Services;

namespace Play.Bingo.Client
{
    /// <summary> Interaction logic for App.xaml. </summary>
    public partial class App
    {
        public static IStorageService Storage { get; private set; }
        public static IMessageService Messenger { get; private set; }

        /// <summary> A poor solution to wing an IoC container. </summary>
        static App()
        {
            Storage = new StorageService();
            Messenger = new MessageService();
        }
    }
}