using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using System;

namespace RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1
{
    public class LeaseCRUD1VM
    {
        public static IR2Command GetEncodeNewDraftCmd(Action doWhenDone)
        {
            return R2Command.Relay(() => Alert.Show("sdfs"));
        }
    }
}
