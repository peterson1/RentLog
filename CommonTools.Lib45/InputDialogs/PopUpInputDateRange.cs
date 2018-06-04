using System;

namespace CommonTools.Lib45.InputDialogs
{
    internal class PopUpInputDateRange : PopUpInput<(DateTime Start, DateTime End), PopUpInputDateRangeWindow>
    {
        internal PopUpInputDateRange(string caption, string message, DateTime? defaultStart, DateTime? defaultEnd) : base(caption, message)
        {
            Start = defaultStart;
            End   = defaultEnd;
        }


        //public (DateTime? Start, DateTime? End)  Draft  { get; set; }
        public DateTime?  Start  { get; set; }
        public DateTime?  End    { get; set; }


        public override bool TryParseValue(out (DateTime Start, DateTime End) parsed)
        {
            parsed = (DateTime.MinValue, DateTime.MinValue);

            if (!Start.HasValue)
            {
                WhyInvalid = "Start Date should not be blank.";
                return false;
            }

            if (!End.HasValue)
            {
                WhyInvalid = "End Date should not be blank.";
                return false;
            }

            if (End.Value < Start.Value)
            {
                WhyInvalid = "End Date should be AFTER Start Date.";
                return false;
            }

            parsed = (Start.Value, End.Value);
            return true;
        }


        protected override void OnWindowLoad(PopUpInputDateRangeWindow win)
        {
            win.startPickr.CalendarClosed += (s, e) 
                => win.endPickr.IsDropDownOpen = true;

            if (!Start.HasValue)
                win.startPickr.IsDropDownOpen = true;
        }
    }
}
