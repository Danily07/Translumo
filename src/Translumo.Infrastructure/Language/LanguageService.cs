using System;
using System.Collections.Generic;
using System.Linq;

namespace Translumo.Infrastructure.Language
{
    public class LanguageService
    {
        private IDictionary<Languages, LanguageDescriptor> _langDescriptors;

        public LanguageService(LanguageDescriptorFactory langFactory)
        {
            this._langDescriptors = langFactory.GetAll().ToDictionary(lang => lang.Language, lang => lang);                
        }

        public LanguageDescriptor GetLanguageDescriptor(Languages language)
        {
            if (!_langDescriptors.ContainsKey(language))
            {
                throw new ArgumentException(nameof(language), "Unknown language");
            }

            return _langDescriptors[language];
        }

        public IEnumerable<LanguageDescriptor> GetAll()
        {
            return _langDescriptors.Select(lang => lang.Value).ToArray();
        }
    }
}
