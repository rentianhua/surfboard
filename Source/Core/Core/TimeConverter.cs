using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cedar.Core.ApplicationContexts;

namespace Cedar.Core
{
    /// <summary>
	/// A helper class to perform UTC time and local time transformation.
	/// </summary>
	public static class TimeConverter
    {
        private static bool needConvertUtc = true;//LocalizationSettings.NeedConvertUtc();
        private static readonly DateTime MinIngoredDateLocal = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Local);
        private static readonly DateTime MinIngoredDateUtc = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime MinIngoredDateUnspecfied = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
        private static readonly DateTime MaxIngoredDateLocal = new DateTime(9998, 12, 31, 0, 0, 0, DateTimeKind.Local);
        private static readonly DateTime MaxIngoredDateUtc = new DateTime(9998, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime MaxIngoredDateUnspecfied = new DateTime(9998, 12, 31, 0, 0, 0, DateTimeKind.Unspecified);

        /// <summary>
        /// Gets a value indicating whether [need convert UTC].
        /// </summary>
        /// <value><c>true</c> if [need convert UTC]; otherwise, <c>false</c>.</value>
        public static bool NeedConvertUtc
        {
            get
            {
                return TimeConverter.needConvertUtc;
            }
        }

        /// <summary>
        /// Gets the current date time.
        /// </summary>
        /// <value>The current date time.</value>
        public static DateTime CurrentDateTime
        {
            get
            {
                if (TimeConverter.NeedConvertUtc)
                {
                    return DateTime.UtcNow;
                }
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Determines whether date is ingored date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// 	<c>true</c> if ingored date; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIngoredDate(DateTime dateTime)
        {
            return dateTime.Date == TimeConverter.MinIngoredDateLocal.Date || dateTime.Date == TimeConverter.MinIngoredDateUtc.Date || dateTime.Date == TimeConverter.MinIngoredDateUnspecfied.Date || dateTime.Date == TimeConverter.MaxIngoredDateLocal.Date || dateTime.Date == TimeConverter.MaxIngoredDateUnspecfied.Date || dateTime.Date == TimeConverter.MaxIngoredDateUtc.Date;
        }

        /// <summary>
        /// Transforms the before saving.
        /// </summary>
        /// <param name="dateTime">The time to convert.</param>
        /// <returns>The tranformed time.</returns>
        public static DateTime ConvertBeforeSaving(DateTime dateTime)
        {
            if (TimeConverter.NeedConvertUtc && dateTime.Kind != DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, ApplicationContext.Current.TimeZone);
            }
            return dateTime;
        }

        /// <summary>
        /// Transforms the after retrieval from database.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The tranformed time.</returns>
        public static DateTime ConvertAfterQuery(DateTime time)
        {
            if (TimeConverter.NeedConvertUtc && time.Kind != DateTimeKind.Local)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(time, ApplicationContext.Current.TimeZone);
            }
            return time;
        }
    }
}
