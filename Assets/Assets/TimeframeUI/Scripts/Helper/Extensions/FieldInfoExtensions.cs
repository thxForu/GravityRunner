namespace Termway.Helper
{
    using System.Linq;
    using System.Reflection;

    using UnityEngine;

    public static class FieldInfoExtensions
    {
        /// <summary>
        /// Recover the fieldInfo tooltip text from <see cref="TooltipAttribute "/> or an empty string if there is no tooltip.
        /// </summary>
        public static string GetTooltip(this FieldInfo fieldInfo)
        {
            TooltipAttribute tooltipAttribute = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true).FirstOrDefault() as TooltipAttribute;
            return tooltipAttribute == null ? "" : tooltipAttribute.tooltip;
        }
    }
}