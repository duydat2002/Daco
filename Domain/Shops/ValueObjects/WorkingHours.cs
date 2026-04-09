namespace Daco.Domain.Shops.ValueObjects
{
    public sealed class WorkingHours : ValueObject
    {
        public record DaySchedule(string Start, string End);

        public DaySchedule? Mon { get; }
        public DaySchedule? Tue { get; }
        public DaySchedule? Wed { get; }
        public DaySchedule? Thu { get; }
        public DaySchedule? Fri { get; }
        public DaySchedule? Sat { get; }
        public DaySchedule? Sun { get; }

        private WorkingHours(
            DaySchedule? mon, DaySchedule? tue, DaySchedule? wed,
            DaySchedule? thu, DaySchedule? fri, DaySchedule? sat,
            DaySchedule? sun)
        {
            Mon = mon; Tue = tue; Wed = wed;
            Thu = thu; Fri = fri; Sat = sat;
            Sun = sun;
        }

        public static WorkingHours Create(
            DaySchedule? mon = null, DaySchedule? tue = null, DaySchedule? wed = null,
            DaySchedule? thu = null, DaySchedule? fri = null, DaySchedule? sat = null,
            DaySchedule? sun = null)
            => new(mon, tue, wed, thu, fri, sat, sun);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Mon?.Start ?? "";
            yield return Mon?.End ?? "";
            yield return Tue?.Start ?? "";
            yield return Tue?.End ?? "";
            yield return Wed?.Start ?? "";
            yield return Wed?.End ?? "";
            yield return Thu?.Start ?? "";
            yield return Thu?.End ?? "";
            yield return Fri?.Start ?? "";
            yield return Fri?.End ?? "";
            yield return Sat?.Start ?? "";
            yield return Sat?.End ?? "";
            yield return Sun?.Start ?? "";
            yield return Sun?.End ?? "";
        }
    }
}
