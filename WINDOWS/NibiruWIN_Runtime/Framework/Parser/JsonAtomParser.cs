using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Nibiru.Framework
{
    internal class JsonAtomParser
    {
        public static UIAtom? Parse(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return ParseElement(doc.RootElement);
        }

        private static UIAtom? ParseElement(JsonElement element)
        {
            string? type = element.GetProperty("type").GetString();
            if (type == null) return null;

            UIAtom? atom = type switch
            {
                "stack" => new UIStack
                {
                    Orientation = element.TryGetProperty("orientation", out var o) 
                        ? (o.GetString() == "horizontal" ? Orientation.Horizontal : Orientation.Vertical)
                        : Orientation.Vertical,
                    Spacing = element.TryGetProperty("spacing", out var sp)
                        ? sp.GetDouble()
                        : 0
                },
                "dock" => new UIDock
                {
                    LastChildFill = element.TryGetProperty("lastChildFill", out var lcf)
                        ? lcf.GetBoolean()
                        : true
                },
                "navigation" => new UINavigation
                {
                    SelectedIndex = element.TryGetProperty("selectedIndex", out var si)
                        ? si.GetInt32()
                        : 0
                },
                "text" => new UIText
                {
                    Text = element.GetProperty("text").GetString() ?? string.Empty,
                    FontSize = element.TryGetProperty("fontSize", out var fs)
                        ? fs.GetDouble()
                        : 14,
                    FontWeight = element.TryGetProperty("fontWeight", out var fw)
                        ? ParseFontWeight(fw.GetString())
                        : FontWeights.Normal,
                    TextAlignment = element.TryGetProperty("textAlign", out var ta)
                        ? ta.GetString()?.ToLower() switch
                        {
                            "center" => TextAlignment.Center,
                            "right" => TextAlignment.Right,
                            "justify" => TextAlignment.Justify,
                            _ => TextAlignment.Left
                        }
                        : TextAlignment.Left
                },
                "button" => new UIButton
                {
                    Text = element.GetProperty("text").GetString() ?? string.Empty,
                    Icon = element.TryGetProperty("icon", out var icon) 
                        ? icon.GetString() 
                        : null,
                    IsAccent = element.TryGetProperty("isAccent", out var iaVal)
                        ? iaVal.GetBoolean()
                        : false,
                    Position = element.TryGetProperty("position", out var pos)
                        ? pos.GetString()
                        : null,
                },
                _ => null
            };

            if (atom == null)
            {
                return null;
            }

            // Parse alignment properties
            if (element.TryGetProperty("horizontalAlignment", out var hAlign))
            {
                atom.HorizontalAlignment = hAlign.GetString()?.ToLower() switch
                {
                    "left" => HorizontalAlignment.Left,
                    "center" => HorizontalAlignment.Center,
                    "right" => HorizontalAlignment.Right,
                    "stretch" => HorizontalAlignment.Stretch,
                    _ => null
                };
            }

            if (element.TryGetProperty("verticalAlignment", out var vAlign))
            {
                atom.VerticalAlignment = vAlign.GetString()?.ToLower() switch
                {
                    "top" => VerticalAlignment.Top,
                    "center" => VerticalAlignment.Center,
                    "bottom" => VerticalAlignment.Bottom,
                    "stretch" => VerticalAlignment.Stretch,
                    _ => null
                };
            }

            // Parse dock property
            if (element.TryGetProperty("dock", out var dockProp))
            {
                atom.Dock = dockProp.GetString()?.ToLower() switch
                {
                    "left" => Dock.Left,
                    "top" => Dock.Top,
                    "right" => Dock.Right,
                    "bottom" => Dock.Bottom,
                    _ => null
                };
            }

            // Parse margin
            if (element.TryGetProperty("margin", out var margin))
            {
                if (margin.ValueKind == JsonValueKind.Number)
                {
                    // Simple number - apply to all sides
                    double value = margin.GetDouble();
                    atom.Margin = new Thickness(value);
                }
                else if (margin.ValueKind == JsonValueKind.Object)
                {
                    // Object with left, top, right, bottom
                    double left = margin.TryGetProperty("left", out var l) ? l.GetDouble() : 0;
                    double top = margin.TryGetProperty("top", out var t) ? t.GetDouble() : 0;
                    double right = margin.TryGetProperty("right", out var r) ? r.GetDouble() : 0;
                    double bottom = margin.TryGetProperty("bottom", out var b) ? b.GetDouble() : 0;
                    atom.Margin = new Thickness(left, top, right, bottom);
                }
            }

            // Prase children
            if (element.TryGetProperty("children", out var children))
            {
                foreach (var child in children.EnumerateArray())
                {
                    var childAtom = ParseElement(child);
                    if (childAtom != null)
                        atom.Children.Add(childAtom);
                }
            }

            // Parse pages (for navigation)
            if (atom is UINavigation navAtom && element.TryGetProperty("pages", out var pages))
            {
                foreach (var page in pages.EnumerateArray())
                {
                    var pageAtom = ParseElement(page);
                    if (pageAtom != null)
                        navAtom.Pages.Add(pageAtom);
                }
            }

            return atom;
        }

        private static FontWeight ParseFontWeight(string? value)
        {
            return value?.ToLower() switch
            {
                "thin" => FontWeights.Thin,
                "extralight" or "ultralight" => FontWeights.ExtraLight,
                "light" => FontWeights.Light,
                "regular" or "normal" => FontWeights.Normal,
                "medium" => FontWeights.Medium,
                "semibold" or "demibold" => FontWeights.SemiBold,
                "bold" => FontWeights.Bold,
                "extrabold" or "ultrabold" => FontWeights.ExtraBold,
                "black" or "heavy" => FontWeights.Black,
                "extrablack" or "ultrablack" => FontWeights.ExtraBlack,
                _ => FontWeights.Normal
            };
        }
    }
}
