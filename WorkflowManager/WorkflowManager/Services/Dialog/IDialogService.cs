﻿using System.Threading.Tasks;

namespace WorkflowManager.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
