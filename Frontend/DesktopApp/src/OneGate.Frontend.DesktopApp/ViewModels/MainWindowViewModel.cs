﻿using Microsoft.Extensions.Options;
using OneGate.Frontend.Client;
using ReactiveUI;

namespace OneGate.Frontend.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IBaseWindow
    {
        /// <summary>
        /// Reference to the changing content of the app window.
        /// </summary>
        private ViewModelBase _content;

        public ViewModelBase Content
        {
            get => _content;
            set
            {
                value.BaseWindow = this;
                this.RaiseAndSetIfChanged(ref _content, value);
            }
        }

        /// <summary>
        /// The constructor initializes the ServerApi and Content properties.
        /// </summary>
        public MainWindowViewModel(IOptions<OneGateClientOptions> options)
        {
            Content = new SignInViewModel(options);
            //Content = new MainViewModel();
        }
    }
}