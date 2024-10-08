﻿using DAL.Enums;

namespace KinopoiskWeb.ViewModels
{
    public class TranslationVM
    {
        public int Id { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
