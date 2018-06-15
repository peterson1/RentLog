using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.Cashiering.SectionTabs.AmbulantCollections
{
    public class AmbulantColxnCrudVM : RepoCrudWindowVMBase<IAmbulantColxnsRepo, AmbulantColxnDTO, AmbulantColxnCrudWindow, ITenantDBsDir>
    {
        public AmbulantColxnCrudVM(SectionDTO sectionDTO, IAmbulantColxnsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
            Section = sectionDTO;
        }


        public SectionDTO Section { get; }


        public override string  TypeDescription => $"{Section} Ambulant Collection";
        protected override string CaptionPrefix => $"{Section} Ambulant Collection";
    }
}
