﻿using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BNetInstaller
{
    internal static class ProgramLauncher
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

    }
}