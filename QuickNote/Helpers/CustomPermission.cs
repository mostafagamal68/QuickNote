using static Microsoft.Maui.ApplicationModel.Permissions;

namespace QuickNote.Helpers
{
    internal class NotificationPermission : BasePlatformPermission
    {
#if ANDROID
        
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
            ("android.permission.POST_NOTIFICATIONS", true)
            }.ToArray();
#endif    
    }
}
