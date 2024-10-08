﻿using DAL.Enums;
using DAL.Models;

namespace DAL.Extensions
{
    public static class TranslatableFieldExtensions
    {
        public static string GetTranslatedField(this ICollection<TranslatableEntityField> translations, TranslatableFieldType fieldType, string languageCode)
        {
            var a = translations
                .FirstOrDefault(t => t.FieldType == fieldType && t.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
            return translations
                .FirstOrDefault(t => t.FieldType == fieldType && t.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
        }

        public static string[] GetTranslatedFields(this ICollection<TranslatableEntityField> translations, TranslatableFieldType fieldType, string languageCode)
        {
            var a = translations
                          .Where(x => x.FieldType == fieldType && x.LanguageCode.ToString() == languageCode)
                          .Select(x => x.Value).ToList();
            return translations
                          .Where(x => x.FieldType == fieldType && x.LanguageCode.ToString() == languageCode)
                          .Select(x => x.Value).ToArray();
        }

        public static string GetFullName(this ICollection<TranslatableEntityField> translations, string languageCode)
        {
            var firstName = GetTranslatedField(translations, TranslatableFieldType.FirstName, languageCode);
            var lastName = GetTranslatedField(translations, TranslatableFieldType.LastName, languageCode);
            return $"{firstName} {lastName}".Trim();
        }
    }
}
