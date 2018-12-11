using CommonTools.Lib45.InputCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.BusyIndicators
{
    public class LoadingSplash
    {
        private SplashScreen _splash;

        public LoadingSplash()
        {
            _splash = new SplashScreen(Assembly.GetAssembly(typeof(LoadingSplash)), "loading.png");
            _splash.Show(false, true);
        }


        public void Close(int fadeOutSeconds = 3)
        {
            _splash.Close(TimeSpan.FromSeconds(fadeOutSeconds));
        }
    }
}
