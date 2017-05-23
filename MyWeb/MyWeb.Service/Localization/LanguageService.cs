using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeb.Data;

namespace MyWeb.Services.Localization
{
    public partial class LanguageService : ILanguageService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string LANGUAGES_BY_ID_KEY = "MyWeb.language.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string LANGUAGES_ALL_KEY = "MyWeb.language.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LANGUAGES_PATTERN_KEY = "MyWeb.language.";

        #endregion

        public void DeleteLanguage(Language language)
        {
            throw new NotImplementedException();
        }

        public IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0)
        {
            throw new NotImplementedException();
        }

        public Language GetLanguageById(int languageId)
        {
            throw new NotImplementedException();
        }

        public void InsertLanguage(Language language)
        {
            throw new NotImplementedException();
        }

        public void UpdateLanguage(Language language)
        {
            throw new NotImplementedException();
        }
    }
}
