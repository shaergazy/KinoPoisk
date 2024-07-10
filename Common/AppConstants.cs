using Common.Extensions;

namespace Common.Helpers;

public static class AppConstants
{
    #region ServiceUri.Self
    public static string BaseUri { get; set; }

    public static string BaseFrontUri { get; set; }
    #endregion

    #region RefreshToken
    public static byte[] Key { get; } = "8d774328bbcf413c95662aa223341e01".ToBytes();

    public static byte[] Iv { get; } = "eefd2c33e23d48039d1fafcab7502257".ToBytes();
    #endregion

    #region ContentTypes
    public static string ExcelContentType => "application/vnd.ms-excel";

    public static string AppJsonContentType => "application/json";

    public static string AppSomeJsonContentType => "application/*+json";
    #endregion
    public static string RelativeFilesPath { get; set; }
    public static string FullFilesPath { get; set; }
    public static string BaseDir { get; set; }
    public static string FlagDir = "Flags";
    public static string PosterDir = "Posters";
}
