﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vip.Printer;

namespace Gerene.DFe.EscPos
{
    public static class GereneHelpers
    {
        #region String
        public static bool IsNull(this string _str) => string.IsNullOrEmpty(_str) || string.IsNullOrWhiteSpace(_str) || string.IsNullOrEmpty(_str.Trim());

        public static bool IsNotNull(this string _str) => !_str.IsNull();

        public static string OnlyNumber(this string value, bool decimalsperator = false, bool negative = false)
        {
            var numbers = new List<char>("1234567890");
            if (decimalsperator)
                numbers.Add(',');
            if (negative)
                numbers.Add('-');
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    bool add = false;
                    for (int j = 0; j < numbers.Count; j++)
                    {
                        if (value[i] == numbers[j])
                        {
                            add = true;
                            break;
                        }
                    }
                    if (add)
                        result += value[i];
                }
            }
            return result;
        }

        public static string[] WrapText(this string text, int max)
        {
            if (string.IsNullOrEmpty(text))
                return new string[] { text };

            if (max == 0)
                return new string[] { text };

            var charCount = 0;
            var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                            ? max - (charCount % max) : 0) + w.Length + 1) / max)
                        .Select(g => string.Join(" ", g.ToArray()))
                        .ToArray();
        }

        #region Formatação
        public static string FormatoCep(this string cep)
        {
            var _codigopostal = cep.OnlyNumber();

            if (_codigopostal.IsNull() || _codigopostal.Length != 8)
                return cep;

            return $"{_codigopostal.Substring(0, 2)}.{_codigopostal.Substring(2, 3)}-{_codigopostal.Substring(5, 3)}";
        }

        public static string FormatoTelefone(this string fone)
        {
            var _tel = fone.OnlyNumber();

            if (_tel.Length == 10)
                return $"({_tel.Substring(0, 2)}) {_tel.Substring(2, 4)}-{_tel.Substring(6, 4)}";

            if (fone.Length == 11)
                return $"({_tel.Substring(0, 2)}) {_tel.Substring(2, 5)}-{_tel.Substring(7, 4)}";

            return fone;
        }

        public static string FormatoCpfCnpj(this string documento)
        {
            var _docto = documento.OnlyNumber();

            if (_docto.Length == 11)
                return $"{_docto.Substring(0, 3)}.{_docto.Substring(3, 3)}.{_docto.Substring(6, 3)}-{_docto.Substring(9, 2)}";

            if (_docto.Length == 14)
                return $"{_docto.Substring(0, 2)}.{_docto.Substring(2, 3)}.{_docto.Substring(5, 3)}/{_docto.Substring(8, 4)}-{_docto.Substring(12, 2)}";

            return documento;
        }
        #endregion

        public static string TextoEsquerda_Direita(string textoE, string textoR, int colunas)
        {
            int padLenght = colunas - (textoR.Length + 1);

            string textoE_new = string.Empty;

            if (textoE.Length + 3 >= padLenght)
                textoE_new = textoE.Substring(0, padLenght - 3).PadRight(padLenght);
            else
                textoE_new = textoE.PadRight(padLenght);

            return  textoE_new + textoR;
        }

        public static string LimitarString(this string txt, int length)
        {
            if (txt.IsNull())
                return string.Empty;

            if (txt.Length < length)
                return txt;

            return txt.Substring(0, length);
        }

        #endregion
    }
}
