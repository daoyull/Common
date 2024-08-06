using Avalonia.Controls.Notifications;

namespace Common.Avalonia.Service;

/// <summary>
/// 全局信息提示
/// </summary>
public interface IGlobalNotify
{
    public WindowNotificationManager? NotificationManager { get; set; }

    public void Message(Notification notification);

    public void Info(string title, string message)
    {
        Message(new Notification(title, message, NotificationType.Information));
    }

    public void Success(string title, string message)
    {
        Message(new Notification(title, message, NotificationType.Success));
    }
    
    public void SuccessTip( string message)
    {
        Message(new Notification("提示", message, NotificationType.Success));
    }

    public void Warning(string title, string message)
    {
        Message(new Notification(title, message, NotificationType.Information));
    }

    public void Error(string title, string message)
    {
        Message(new Notification(title, message, NotificationType.Error));
    }
}