﻿using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace CommonTools.Lib45.FileSystemTools
{
    [AddINotifyPropertyChangedInterface]
    public class UpdatedExeNotifier : UpdatedFileNotifier
    {

        public UpdatedExeNotifier(string fileToWatch) : base(fileToWatch)
        {
            if (!CurrentExe.GetFullPath().IsInTempDir())
                RelaunchInTemp();
        }


        private void RelaunchInTemp()
        {
            if (WatchedFile.IsBlank()) return;
            var tmpExe = "";
            try
            {
                tmpExe = WatchedFile.MakeTempCopy(".exe");
            }
            catch (Exception ex)
            {
                Alert.Show(ex, $"Creating temp copy of {WatchedFile}");
                return;
            }
            CopyCfgToTemp(tmpExe);

            Process.Start(tmpExe, GetCommandLineArgs());
            Application.Current.Shutdown();
        }


        private static void CopyCfgToTemp(string tmpExe)
        {
            var exeNow = CurrentExe.GetFullPath();
            var cfgNow = exeNow + ".config";
            var tmpCfg = tmpExe + ".config";

            if (File.Exists(cfgNow))
                File.Copy(cfgNow, tmpCfg, true);
        }


        private string GetCommandLineArgs()
            => string.Join(" ", Environment.GetCommandLineArgs().Skip(1)
                .Select(_ => Quotify(_)));


        private string Quotify(string soloArg)
        {
            var pos = soloArg.IndexOf('=');
            var key = soloArg.Substring(0, pos);
            var val = soloArg.Substring(pos + 1);

            if (val.Contains(" "))
                val = $"\"{val}\"";

            return $"{key}={val}";
        }


        protected override void OnExecuteClick() 
            => RelaunchInTemp();
    }
}
