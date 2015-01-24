using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Play.Bingo.Client.Properties;

namespace Play.Bingo.Client.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public static bool IsInDesignMode
        {
            get { return DesignerProperties.GetIsInDesignMode(new DependencyObject()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void UiInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}