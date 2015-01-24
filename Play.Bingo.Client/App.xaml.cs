using Play.Bingo.Client.Services;

namespace Play.Bingo.Client
{
    /// <summary> Interaction logic for App.xaml. </summary>
    public partial class App
    {
        /// <summary> A poor solution to wing an IoC container. </summary>
        static App()
        {
            Storage = new StorageService();
            Messenger = new MessageService();
            Solver = new Solver();
            Solver.AddRule(new ColumnRule());
            Solver.AddRule(new RowRule());
            Solver.AddRule(new DiagonalRule());
        }

        public static IStorageService Storage { get; private set; }
        public static IMessageService Messenger { get; private set; }
        public static ISolver Solver { get; private set; }
    }
}