using System;

namespace Nibiru.Framework
{
    public static class IconHelper
    {
        public static string MapIconNameToUnicode(string? icon)
        {
            if (string.IsNullOrEmpty(icon)) return string.Empty;

            // If the icon looks like a unicode literal, return as-is
            if (icon.StartsWith("\\u", StringComparison.OrdinalIgnoreCase) || icon.Length == 1)
                return icon;

            return icon.ToLower() switch
            {
                "home" => "\uE80F",
                "projects" => "\uE8F4",
                "settings" => "\uE713",
                "folder" => "\uE8B7",
                "document" => "\uE8A5",
                "search" => "\uE721",
                "add" => "\uE710",
                "edit" => "\uE70F",
                "delete" => "\uE74D",
                "save" => "\uE74E",
                "refresh" => "\uE72C",
                "download" => "\uE896",
                "upload" => "\uE898",
                "calendar" => "\uE787",
                "mail" => "\uE715",
                "people" => "\uE716",
                "phone" => "\uE717",
                "clock" => "\uE823",
                "star" => "\uE734",
                "heart" => "\uE728",
                "flag" => "\uE7C1",
                "bookmark" => "\uE8A4",
                "tag" => "\uE8EC",
                "camera" => "\uE722",
                "image" => "\uE91B",
                "video" => "\uE714",
                "music" => "\uE8D6",
                "play" => "\uE768",
                "pause" => "\uE769",
                "stop" => "\uE71A",
                "back" => "\uE72B",
                "forward" => "\uE72A",
                "more" => "\uE712",
                "info" => "\uE946",
                "warning" => "\uE7BA",
                "error" => "\uE783",
                "lock" => "\uE72E",
                "unlock" => "\uE785",
                "cloud" => "\uE753",
                "sync" => "\uE895",
                "filter" => "\uE71C",
                "sort" => "\uE8CB",
                "start" => "\uE768",
                _ => string.Empty
            };
        }
    }
}
