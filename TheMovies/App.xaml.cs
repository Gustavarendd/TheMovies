﻿using OfficeOpenXml;
using System.Windows;


namespace TheMovies
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      

        protected override void OnStartup(StartupEventArgs e)
        { 

             ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        
    }

}
