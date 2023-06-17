using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;

namespace Translumo.Infrastructure.Powershell
{
    public class PowerShellRunspace : IDisposable
    {
        public PowerShell PowerShell { get; }

        private readonly Runspace _runspace;

        public PowerShellRunspace(params string[] modulesToLoad)
        {
            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = ExecutionPolicy.RemoteSigned;
            foreach (var module in modulesToLoad)
            {
                defaultSessionState.ImportPSModule(module);
            }
            
            _runspace = RunspaceFactory.CreateRunspace(defaultSessionState);

            _runspace.Open();

            PowerShell = PowerShell.Create();
            PowerShell.Runspace = _runspace;
        }

        public void Dispose()
        {
            _runspace?.Dispose();
            PowerShell?.Dispose();
        }
    }
}
