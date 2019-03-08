using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorkflowManager.Services.Settings
{
    public class SettingService : ISettingService
    {
        #region Setting Constants

        private const string AccessToken = "access_token";                // "access_token"
        private readonly string AccessTokenDefault = string.Empty;        // string.Empty or "access_token"

        #endregion

        #region Settings Properties

        public string AuthAccessToken
        {
            get => GetValueOrDefault(AccessToken, AccessTokenDefault);
            set => AddOrUpdateValue(AccessToken, value);
        }

        #endregion

        #region Public Methods

        public Task AddOrUpdateValue(string key, string value) => this.AddOrUpdateValueInternal(key, value);
        public string GetValueOrDefault(string key, string defaultValue) => this.GetValueOrDefaultInternal(key, defaultValue);
        
        #endregion

        #region Internal Implementation

        private async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await this.Remove(key);
            }

            Application.Current.Properties[key] = value;

            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to remove: " + key, "Message: " + e.Message);
                throw e;
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;

            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }

            return null != value ? (T)value : defaultValue;
        }

        private async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to remove: " + key, "Message: " + e.Message);
                throw e;
            }
        }
        
        #endregion
    }
}
