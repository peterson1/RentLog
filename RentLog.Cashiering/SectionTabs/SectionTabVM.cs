using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;

namespace RentLog.Cashiering.SectionTabs
{
    [AddINotifyPropertyChangedInterface]
    public class SectionTabVM
    {
        public SectionTabVM()
        {
        }

        public SectionTabVM(SectionDTO section, AppArguments appArguments)
        {
            Title = section.Name;
        }


        public string Title { get; private set; }
    }
}
