﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Milestone3.Bussiness_Layer.Interface;

namespace Milestone3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            log.Debug("Initialising ...");
            
        }
    }
}
