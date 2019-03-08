using System.Threading.Tasks;
using WorkflowManager.Services.Dialog;
using WorkflowManager.Services.Navigation;

namespace WorkflowManager.ViewModels.Base
{
    public class ViewModelBase : ExtendedBindableObject
    {
        // services here
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
                base.RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase()
        {
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
            DialogService = ViewModelLocator.Resolve<IDialogService>();
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
