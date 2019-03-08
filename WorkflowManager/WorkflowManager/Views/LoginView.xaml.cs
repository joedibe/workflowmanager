using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkflowManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentPage
	{
        private LoginViewModel viewModel;
		public LoginView ()
		{
            NavigationPage.SetHasNavigationBar(this, false);

			InitializeComponent ();

            BindingContext = viewModel = new LoginViewModel();
		}
	}
}