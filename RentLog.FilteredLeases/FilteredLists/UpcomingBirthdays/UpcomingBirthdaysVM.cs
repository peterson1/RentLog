using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.UpcomingBirthdays
{
    public class UpcomingBirthdaysVM : FilteredListVMBase
    {
        private DateTime _minDate;
        private DateTime _maxDate;
        private int      _yrNow;


        public UpcomingBirthdaysVM(MainWindowVM mainWindowVM, ITenantDBsDir dir) : base(mainWindowVM, dir)
        {
            SetMinMaxDates(DateTime.Now.Date);
        }


        private void SetMinMaxDates(DateTime now)
        {
            _minDate = now.AddDays(-3);
            _maxDate = now.AddDays(15);
            _yrNow   = now.Year;
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDbBase mkt, int secId)
        {
            var all = mkt.ActiveLeases.GetAll()
                         .Where  (_ => IsUpcomingBirthday(_.Tenant.BirthDate))
                         .OrderBy(_ => BirthDayNow(_.Tenant.BirthDate))
                         .ToList ();

            return secId == 0 ? all
                : all.Where(_ => _.Stall.Section.Id == secId).ToList();
        }


        private bool IsUpcomingBirthday(DateTime bDay)
        {
            var bDayNow = new DateTime(_yrNow, bDay.Month, bDay.Day);
            return bDayNow >= _minDate
                && bDayNow <  _maxDate;
        }


        private DateTime BirthDayNow(DateTime realBday)
            => new DateTime(_yrNow, realBday.Month, realBday.Day);
    }
}
