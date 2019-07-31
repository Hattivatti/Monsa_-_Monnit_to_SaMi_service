using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Monsa___Monnit_to_SaMi_service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();


        }

        protected override void OnCommitted(System.Collections.IDictionary savedState)
        {
            RegistryKey Regkey = Registry.LocalMachine;
            string Keypath = @"SYSTEM\CurrentControlSet\Services\MonSa\Params";
            Regkey = Regkey.CreateSubKey(Keypath);
            Regkey.SetValue("UName", "default", RegistryValueKind.String);
            Regkey.SetValue("PWord", "default", RegistryValueKind.String);
            Regkey.SetValue("Network", "1337", RegistryValueKind.String);
            Regkey.SetValue("Interval", "1", RegistryValueKind.String);

            /*ServiceController sc = new ServiceController("MonSa");
            sc.Start();*/
        }
}
}
