﻿using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.FileSystemTools;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace CommonTools.Lib45.ThreadTools
{
    public class Alert
    {
        public static void Show(string message,
                                MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                MessageBoxButton messageBoxButton = MessageBoxButton.OK)
            => Show(DateTime.Now.ToShortDateString(),
                    message, messageBoxImage, messageBoxButton);


        public static void Show(Exception ex,
                                [CallerMemberName] string context = null,
                                bool withTypeNames = true, 
                                bool withShortStackTrace = true,
                                MessageBoxImage messageBoxImage = MessageBoxImage.Error,
                                MessageBoxButton messageBoxButton = MessageBoxButton.OK)
            => Show($"Error on “{context}”",
                    $"{ex.Info(withTypeNames, withShortStackTrace)}", 
                    messageBoxImage, messageBoxButton);


        public static void Show(string caption, 
                                string message,
                                MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                MessageBoxButton messageBoxButton = MessageBoxButton.OK)
            => new Thread(new ThreadStart(delegate
            {
                //var longCap = $"   {caption}  [{DateTime.Now.ToShortTimeString()}]  -  {GetExeInfo()}";
                //MessageBox.Show(message, longCap, messageBoxButton, messageBoxImage);
                ShowModal(caption, message, messageBoxImage, messageBoxButton);
            }
            )).Start();


        public static void ShowModal(string caption,
                                     string message,
                                     MessageBoxImage messageBoxImage = MessageBoxImage.Information,
                                     MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            var longCap = $"   {caption}  [{DateTime.Now.ToShortTimeString()}]  -  {GetExeInfo()}";
            MessageBox.Show(message, longCap, messageBoxButton, messageBoxImage);
        }


        private static string GetExeInfo()
        {
            try   { return $"{CurrentExe.GetShortName()} v.{CurrentExe.GetVersion()}"; }
            catch { return ""; }
        }


        public static void Confirm(string message,
                                   Action action,
                                   string caption = "Please Confirm",
                                   MessageBoxImage messageBoxImage = MessageBoxImage.Question,
                                   MessageBoxButton messageBoxButton = MessageBoxButton.YesNo)
        {
            var choice = MessageBox.Show(message, "   " + caption, 
                            messageBoxButton, messageBoxImage);

            if (choice == MessageBoxResult.Yes)
                action?.Invoke();
        }
    }
}
