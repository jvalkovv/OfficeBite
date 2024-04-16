using OfficeBite.Infrastructure.Extensions.InterfaceForTest;

namespace OfficeBite.Infrastructure.Extensions
{
    public class DateTimeNowWrapper : IDateTimeNowWrapper
    {
        private readonly DateTime _dateTime;
        public DateTimeNowWrapper(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
        public DateTime Now => DateTime.Now;
    }
}
