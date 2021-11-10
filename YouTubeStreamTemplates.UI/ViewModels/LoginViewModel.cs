using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using YouTubeStreamTemplates.LiveStream;
using YouTubeStreamTemplates.Settings;

namespace YouTubeStreamTemplates.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel() { LoginCommand = ReactiveCommand.CreateFromTask(InitLiveStreamService); }

        public ReactiveCommand<Unit, bool> LoginCommand { get; }

        private static async Task<bool> InitLiveStreamService()
        {
            await SettingsService.Init();
            await StreamService.Init();
            return true;
        }
    }
}