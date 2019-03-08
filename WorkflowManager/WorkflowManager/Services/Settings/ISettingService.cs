using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowManager.Services.Settings
{
    public interface ISettingService
    {
        string AuthAccessToken { get; set; }
        string GetValueOrDefault(string key, string defaultValue);
        Task AddOrUpdateValue(string key, string value);
    }
}
