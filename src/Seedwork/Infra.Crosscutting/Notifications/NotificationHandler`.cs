using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goal.Seedwork.Infra.Crosscutting.Notifications
{
    public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : class, INotification
    {
        private readonly ICollection<TNotification> notifications;

        protected NotificationHandler()
        {
            notifications = new List<TNotification>();
        }

        public virtual async Task AddNotificationAsync(TNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            notifications.Add(notification);

            await Task.CompletedTask;
        }

        public virtual ICollection<TNotification> GetNotifications()
            => notifications;

        public async Task<ICollection<TNotification>> GetNotificationsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(GetNotifications());
        }

        public virtual bool HasDomainViolation()
            => HasNotificationsOf(NotificationType.DomainViolation);

        public async Task<bool> HasDomainViolationAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(HasDomainViolation());
        }

        public virtual bool HasExternalError()
            => HasNotificationsOf(NotificationType.ExternalError);

        public async Task<bool> HasExternalErrorAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(HasExternalError());
        }

        public virtual bool HasInternalError()
            => HasNotificationsOf(NotificationType.InternalError);

        public async Task<bool> HasInternalErrorAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(HasInternalError());
        }

        public virtual bool HasInformation()
            => HasNotificationsOf(NotificationType.Information);

        public async Task<bool> HasInformationAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(HasInformation());
        }

        public virtual bool HasInputValidation()
            => HasNotificationsOf(NotificationType.InputValidation);

        public async Task<bool> HasInputValidationAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(HasInputValidation());
        }

        public virtual bool HasNotifications()
            => notifications.Count > 0;

        public async Task<bool> HasNotificationsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(HasNotifications());
        }

        private bool HasNotificationsOf(NotificationType type)
            => notifications.Any(p => p.Type == type);
    }
}
