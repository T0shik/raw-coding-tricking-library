using System;
using IdentityServer4;

namespace TrickingLibrary.Api
{
    public struct TrickingLibraryConstants
    {
        public struct Policies
        {
            // public const string Anon = nameof(Anon);
            // public const string User = IdentityServerConstants.LocalApi.PolicyName;
            public const string Mod = nameof(Mod);
        }

        public struct IdentityResources
        {
            public const string RoleScope = "role";
        }

        public struct Claims
        {
            public const string Role = "role";
        }

        public struct Roles
        {
            public const string Mod = nameof(Mod);
        }

        public struct Files
        {
            public struct Providers
            {
                public const string Local = nameof(Local);
                public const string S3 = nameof(S3);
            }

            public const string TempPrefix = "temp_";
            public const string ConvertedPrefix = "c";
            public const string ThumbnailPrefix = "t";
            public const string ProfilePrefix = "p";

            public static string GenerateConvertedFileName() => $"{ConvertedPrefix}{DateTime.Now.Ticks}.mp4";
            public static string GenerateThumbnailFileName() => $"{ThumbnailPrefix}{DateTime.Now.Ticks}.jpg";
            public static string GenerateProfileFileName() => $"{ProfilePrefix}{DateTime.Now.Ticks}.jpg";
        }
    }
}