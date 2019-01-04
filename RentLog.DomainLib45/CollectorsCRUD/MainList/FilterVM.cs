using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib45.CollectorsCRUD.MainList
{
    [AddINotifyPropertyChangedInterface]
    public class FilterVM : TextFilterBase<CollectorDTO>
    {
        public string   FilterId         { get; set; }
        public string   FilterName       { get; set; }
        public string   FilterIsActive   { get; set; }

        protected override Dictionary<string, Func<CollectorDTO, string>> FilterProperties => new Dictionary<string, Func<CollectorDTO, string>>
        {
            { nameof(FilterId)      , _ => _.Id.ToString()       },
            { nameof(FilterName)    , _ => _.Name                },
            { nameof(FilterIsActive), _ => _.IsActive.ToString() }
        };
    }
}
