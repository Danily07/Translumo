using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Translumo.Infrastructure.Exceptions;

namespace Translumo.Infrastructure.Powershell
{
    public static class OptionalFeaturesProvider
    {
        public static async Task<bool> OcrLanguagePackIsInstalled(string languageCode)
        {
            const string INSTALLED_STATE = "Installed";

            PSObject psObject = await GetPsObjectOcrLanguagePack(languageCode);

            return INSTALLED_STATE == psObject.Properties["State"].Value.ToString();
        }

        public static async Task<LanguagePackInstallResult> OcrLanguagePackInstall(string languageCode)
        {
            PSObject psObject = await GetPsObjectOcrLanguagePack(languageCode);
            using (var rs = new PowerShellRunspace())
            {
                rs.PowerShell.AddScript("$args[0] | Add-WindowsCapability -Online");
                rs.PowerShell.AddArgument(psObject);


                var resultPsObject = (await rs.PowerShell.InvokeAsync()).FirstOrDefault();
                if (resultPsObject == null)
                {
                    if (rs.PowerShell.Streams.Error.Any())
                    {
                        throw new OptionalFeatureAccessException("Failed to install language pack",
                            rs.PowerShell.Streams.Error.First().Exception);
                    }
                }

                return new LanguagePackInstallResult()
                {
                    RestartIsNeeded = (bool)psObject.Properties["RestartNeeded"].Value
                };
            }
        }

        private static async Task<PSObject> GetPsObjectOcrLanguagePack(string languageCode)
        {
            using (var rs = new PowerShellRunspace())
            {

                rs.PowerShell.AddScript(
                    $"Get-WindowsCapability -Online | Where-Object {{ $_.Name -Like 'Language.OCR*{languageCode}*' }}");

                var resultPsObject = (await rs.PowerShell.InvokeAsync()).FirstOrDefault();
                if (resultPsObject == null)
                {
                    if (rs.PowerShell.Streams.Error.Any())
                    {
                        throw new OptionalFeatureAccessException("Failed to get PS object",
                            rs.PowerShell.Streams.Error.First().Exception);
                    }

                    throw new OptionalFeatureAccessException("Failed to get PS object");
                }
                
                return resultPsObject;
            }
        }
    }
}
