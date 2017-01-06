using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Play.Bingo.Client.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void UiInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}