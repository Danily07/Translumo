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
            PSObject psObject = await GetPsObjectOcrLanguagePack(languageCode);
            return LanguagePackIsInstalled(psObject);
        }

        public static async Task<bool> TtsLanguagePackIsInstalled(string languageCode)
        {
            PSObject psObject = await GetPsObjectTextToSpeechLanguagePack(languageCode);
            return LanguagePackIsInstalled(psObject);
        }

        public static async Task<LanguagePackInstallResult> OcrLanguagePackInstall(string languageCode)
        {
            PSObject psObject = await GetPsObjectOcrLanguagePack(languageCode);
            return await LanguagePackInstall(psObject);
        }

        public static async Task<LanguagePackInstallResult> TtsLanguagePackInstall(string languageCode)
        {
            PSObject psObject = await GetPsObjectTextToSpeechLanguagePack(languageCode);
            return await LanguagePackInstall(psObject);
        }

        private static bool LanguagePackIsInstalled(PSObject languagePackPsObject)
        {
            const string INSTALLED_STATE = "Installed";
            return INSTALLED_STATE == languagePackPsObject.Properties["State"].Value.ToString();
        }
        
        private static async Task<LanguagePackInstallResult> LanguagePackInstall(PSObject languagePackPsObject)
        {
            using (var rs = new PowerShellRunspace())
            {
                rs.PowerShell.AddScript("$args[0] | Add-WindowsCapability -Online");
                rs.PowerShell.AddArgument(languagePackPsObject);


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
                    RestartIsNeeded = (bool)languagePackPsObject.Properties["RestartNeeded"].Value
                };
            }
        }

        private static async Task<PSObject> GetPsObjectOcrLanguagePack(string languageCode) =>
            await GetPsObjectLanguagePack(languageCode, "OCR");

        private static async Task<PSObject> GetPsObjectTextToSpeechLanguagePack(string languageCode) =>
            await GetPsObjectLanguagePack(languageCode, "TextToSpeech");

        private static async Task<PSObject> GetPsObjectLanguagePack(string languageCode, string languagePackType)
        {
            using (var rs = new PowerShellRunspace())
            {

                rs.PowerShell.AddScript(
                    $"Get-WindowsCapability -Online | Where-Object {{ $_.Name -Like 'Language.{languagePackType}*{languageCode}*' }}");

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
