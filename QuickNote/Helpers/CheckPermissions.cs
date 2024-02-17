namespace QuickNote.Helpers
{
    public class CheckPermissions
    {
        public static async Task<PermissionStatus> CheckNotificationPermission()
        {
            if (DeviceInfo.Version.Major >= 11)
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<NotificationPermission>();

                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<NotificationPermission>();
                return status;
            }
            else
                return PermissionStatus.Granted;
        }
    }
}
