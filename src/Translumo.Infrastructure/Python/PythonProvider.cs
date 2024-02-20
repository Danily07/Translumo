using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Translumo.Infrastructure.Constants;
using Translumo.Infrastructure.Exceptions;

namespace Translumo.Infrastructure.Python
{
    public static class PythonProvider
    {
        private const int PYTH_COMMAND_TIMEOUT_MS = 1600000;

        public static async Task<bool> ModuleIsInstalledAsync(string moduleName)
        {
            try
            {
                var cancellationTokenSource = new CancellationTokenSource(PYTH_COMMAND_TIMEOUT_MS);
                using (var command = PythonCommand.Create($"-c \"import {moduleName}\"", cancellationTokenSource.Token))
                {
                    var result = await command.TryGetResult();

                    return !result.HasError;
                }
            }
            catch (Exception ex)
            {
                throw new PythonAccessException("Unable to run python", ex);
            }
        }

        public static async Task InstallModuleAsync(string moduleName, bool forceReinstall = false)
        {
            await TryInstallPipAsync();

            var cancellationTokenSource = new CancellationTokenSource(PYTH_COMMAND_TIMEOUT_MS);
            var commandStr = $"install --no-cache-dir --no-warn-script-location {moduleName}";
            if (forceReinstall)
            {
                commandStr += " --force-reinstall";
            }

            using (var command = PythonCommand.CreatePip(commandStr, cancellationTokenSource.Token))
            {
                var result = await command.TryGetResult();
                if (result.HasError)
                {
                    throw new PythonAccessException($"Failed to install module: '{result.ErrorOutput}'");
                }
            }
        }

        public static async Task TryInstallPipAsync()
        {
            if (PipIsInstalled())
            {
                return;
            }

            var cancellationTokenSource = new CancellationTokenSource(PYTH_COMMAND_TIMEOUT_MS);
            using (var command = PythonCommand.Create($"get-pip.py", cancellationTokenSource.Token))
            {
                var result = await command.TryGetResult();
                if (result.HasError)
                {
                    throw new PythonAccessException($"Failed to instal pip: '{result.ErrorOutput}'");
                }
            }
        }

        public static bool PipIsInstalled()
        {
            return File.Exists(Global.PipPath);
        }
    }
}
