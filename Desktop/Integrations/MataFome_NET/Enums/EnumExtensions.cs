using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MataFome.API.Client.Extensions
{
    public static class EnumExtensions
    {
        private static string GetAttributeValue<TAttribute>(Enum enumValue, Func<TAttribute, string> methodToCall, string defaultValue = null)
            where TAttribute : Attribute
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return string.Empty;

            var valueFromField = GetAttributeValueFromField<TAttribute>(field, methodToCall);

            if (valueFromField != null)
                return valueFromField;
            else
                return defaultValue;
        }

        private static string GetAttributeValueFromField<TAttribute>(FieldInfo fieldInfo, Func<TAttribute, string> methodToCall)
        {
            var attribute = fieldInfo.GetCustomAttributes(true).OfType<TAttribute>().LastOrDefault();
            if (attribute != null)
                return methodToCall(attribute);
            else
                return null;
        }

        public static string GetEnumMemberValue(this Enum enumValue)
        {
            return GetAttributeValue<EnumMemberAttribute>(enumValue, attr => attr.Value, enumValue.ToString());
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return GetAttributeValue<DisplayAttribute>(enumValue, attr => attr.GetName(), enumValue.GetEnumMemberValue());
        }

        public static string GetDisplayDescription(this Enum enumValue)
        {

            return GetAttributeValue<DisplayAttribute>(enumValue, attr => attr.GetDescription(), enumValue.GetEnumMemberValue());
        }

        public static string GetDisplayShortName(this Enum enumValue)
        {
            return GetAttributeValue<DisplayAttribute>(enumValue, attr => attr.GetShortName());
        }

        public static string GetDescription(this Enum enumValue)
        {
            return GetAttributeValue<DescriptionAttribute>(enumValue, attr => attr.Description.Trim());
        }

        public static SortedDictionary<string, string> ListComboBoxValues<TEnum>()
            where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            var enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var dicComboBoxValues = new SortedDictionary<string, string>();

            foreach (var enumField in enumFields)
            {
                var enumValue =
                    GetAttributeValueFromField<DisplayAttribute>(enumField, attr => attr.GetDescription());

                if (enumField.Name != "Empty")
                    dicComboBoxValues.Add(
                        enumField.Name, // Key (enum field literal)
                        enumValue ?? enumField.Name // Value (Description)
                        );
            }

            return dicComboBoxValues;
        }

        public static SortedDictionary<string, string> ListComboBoxValuesByDescription<TEnum>()
            where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            var enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var dicComboBoxValues = new SortedDictionary<string, string>();

            foreach (var enumField in enumFields)
            {
                var enumValue =
                    GetAttributeValueFromField<DescriptionAttribute>(enumField, attr => attr.Description.Trim());

                dicComboBoxValues.Add(
                    enumField.Name, // Key (enum field literal)
                    enumValue ?? enumField.Name // Value (Description)
                    );
            }

            return dicComboBoxValues;
        }

        public static SortedDictionary<string, string> ListValuesByShortName<TEnum>()
            where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            var enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var dicValuesByShortName = new SortedDictionary<string, string>();

            foreach (var enumField in enumFields)
            {
                var enumDescription =
                    GetAttributeValueFromField<DisplayAttribute>(enumField, attr => attr.GetDescription());
                var enumShortName =
                    GetAttributeValueFromField<DisplayAttribute>(enumField, attr => attr.GetShortName());

                if (enumField.Name != "Empty")
                    dicValuesByShortName.Add(
                        enumShortName ?? enumField.Name, // Key (enum field literal)
                        enumDescription ?? enumField.Name // Value (Description)
                        );
            }

            return dicValuesByShortName;
        }

        public static TEnum Parse<TEnum>(string strEnumValue)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }

        public static TEnum GetEnumByShortName<TEnum>(string shortName)
            where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            var enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            TEnum enumReturnValue = default(TEnum);

            DisplayAttribute attribute = null;
            string attrShortName = null;
            foreach (var enumField in enumFields)
            {
                attribute = enumField.GetCustomAttributes(true).OfType<DisplayAttribute>().LastOrDefault();

                if (attribute == null)
                    continue;

                attrShortName = attribute.GetShortName();

                if (string.IsNullOrEmpty(attrShortName))
                    throw new InvalidOperationException();

                if (attrShortName.Equals(shortName, StringComparison.InvariantCultureIgnoreCase))
                {
                    enumReturnValue = (TEnum)Enum.Parse(enumType, enumField.Name);
                    break;
                }
            }

            return enumReturnValue;
        }

        public static TEnum GetEnumByName<TEnum>(string name)
            where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            var enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            TEnum enumReturnValue = default(TEnum);

            DisplayAttribute attribute = null;
            string attrName = null;
            foreach (var enumField in enumFields)
            {
                attribute = enumField.GetCustomAttributes(true).OfType<DisplayAttribute>().LastOrDefault();

                if (attribute == null)
                    continue;

                attrName = attribute.GetName();

                if (string.IsNullOrEmpty(attrName))
                    throw new InvalidOperationException();

                if (attrName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    enumReturnValue = (TEnum)Enum.Parse(enumType, enumField.Name);
                    break;
                }
            }

            return enumReturnValue;
        }
    }
}