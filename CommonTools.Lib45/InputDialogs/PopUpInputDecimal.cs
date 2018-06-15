namespace CommonTools.Lib45.InputDialogs
{
    internal class PopUpInputDecimal : PopUpInput<decimal, PopUpInputDecimalWindow>
    {
        internal PopUpInputDecimal(string caption, string message, decimal? defaultVal, bool allowZero) : base(caption, message)
        {
            Draft         = defaultVal;
            IsZeroAllowed = allowZero;
        }


        public decimal?  Draft          { get; set; }
        public bool      IsZeroAllowed  { get; }


        public override bool TryParseValue(out decimal parsed)
        {
            parsed = 0;
            if (!Draft.HasValue)
            {
                WhyInvalid = "Value should not be blank.";
                return false;
            }
            if (!IsZeroAllowed && Draft.Value == 0)
            {
                WhyInvalid = "Value should not be zero.";
                return false;
            }
            parsed = Draft.Value;
            return true;
        }


        protected override void OnWindowLoad(PopUpInputDecimalWindow win)
            => win.txtVal.SelectAll();
    }
}
