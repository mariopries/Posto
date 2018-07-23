using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PostoWeb
{
    public class Util
    {

        
        /// <summary>
        /// Valida a Inscrição Estadual
        /// </summary>
        /// <param name="uf"></param>
        /// <param name="ie"></param>
        /// <returns></returns>
        public static bool ValidarInscricaoEstadual(string uf, string ie)
        {
            if (ie == string.Empty || ie == null || (ie != null && ie.ToUpperInvariant() == "ISENTO"))
            {
                return true;
            }

            if (Regex.IsMatch(ie, "[^0-9/.-]"))
            {
                return false;
            }

            var ieSemPicture = Regex.Replace(ie, "[^0-9]", "").Trim();

            if (ieSemPicture == string.Empty)
            {
                return false;
            }

            var ieCalculada = string.Empty;
            List<int> ieInt;
            int valorIe = 0;

            switch (uf)
            {
                #region Acre

                case "AC":
                    // Exemplo: Inscrição Estadual 01.004.823/001-12

                    if (ieSemPicture.Length != 13)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 11).Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Alagoas

                case "AL":
                    // Exemplo: Inscrição Estadual 24.073.784-9

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "24")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Amapá

                case "AP":
                    // Exemplo: Inscrição Estadual 03.012.345-9

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "03")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();

                    // De 03000001 a 03017000 => p = 5 e d = 0
                    // De 03017001 a 03019022 => p = 9 e d = 1
                    // De 03019023 em diante ===>p = 0 e d = 0
                    valorIe = Int32.Parse(ieCalculada);
                    int p = 0, d = 0;

                    if (valorIe <= 03017000)
                    {
                        p = 5;
                        d = 0;
                    }
                    else if (valorIe >= 03019023)
                    {
                        p = 0;
                        d = 0;
                    }
                    else
                    {
                        p = 9;
                        d = 1;
                    }

                    ieCalculada += CalcularDvModulo11(ieInt, 2, 9, p, 0, d).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Amazonas

                case "AM":
                    // Exemplo: Inscrição Estadual 04.150.239-6

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Bahia

                case "BA":
                    // Exemplo: Inscrição Estadual com 8 digitos 123456-63
                    //          Inscrição Estadual com 9 digitos 1000003-06

                    if (ieSemPicture.Length != 8 && ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, (ieSemPicture.Length == 8 ? 6 : 7)).Trim();

                    if ("0123458".Contains(ieSemPicture[0]))
                    {
                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                        ieCalculada += CalcularDvModulo10(ieInt, 10).ToString().Trim();

                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                        ieCalculada = string.Format("{0}{1}{2}", ieCalculada.Substring(0, ieCalculada.Length - 1), CalcularDvModulo10(ieInt, 10).ToString().Trim(), ieCalculada[ieCalculada.Length - 1].ToString().Trim());
                    }
                    else
                    {
                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                        ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                        ieCalculada = string.Format("{0}{1}{2}", ieCalculada.Substring(0, ieCalculada.Length - 1), CalcularDvModulo11(ieInt, 10).ToString().Trim(), ieCalculada[ieCalculada.Length - 1].ToString().Trim());
                    }

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Ceará

                case "CE":
                    // Exemplo: Inscrição Estadual 06.003.068-7

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Distrito Federal

                case "DF":
                    // Exemplo: Inscrição Estadual 07.303057001-34

                    if (ieSemPicture.Length != 13)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "07")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 11).Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Espirito Santo

                case "ES":
                    // Exemplo: Inscrição Estadual 24.073.784-9

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Goiás

                case "GO":
                    // Exemplo: Inscrição Estadual 24.073.784-9

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    valorIe = Int32.Parse(ieCalculada);

                    // Quando o resto da divisão for um (1), e a inscrição for maior ou igual a 10103105 e menor ou igual a 10119997, o dígito verificador será um (1);
                    // Quando o resto da divisão for um (1), e a inscrição estiver fora do intervalo citado acima, o dígito verificador será zero (0);
                    var dvResto10 = (valorIe >= 10103105 && valorIe <= 10119997 ? 1 : 0);

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 2, 9, 0, dvResto10, 0).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region MAranhão

                case "MA":
                    // Exemplo: Inscrição Estadual 12.137.258-8

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "12")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Mato Grosso

                case "MT":
                    // Exemplo: Inscrição Estadual 13.162.370-2

                    if (ieSemPicture.Length > 11)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Meto Grosso do Sul

                case "MS":
                    // Exemplo: Inscrição Estadual 24.073.784-9

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "28")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Minas Gerais

                case "MG":
                    // Exemplo: Inscrição Estadual 001.000.8020086

                    if (ieSemPicture.Length != 13)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 2).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieInt.Insert(3, 0);

                    int x = 2;
                    int soma = 0;

                    for (int i = ieInt.Count - 1; i >= 0; i--)
                    {

                    soma += FixedWrap((ieInt[i] * x).ToString(), 1).Select(t => int.Parse(t)).ToList().Sum();
                    //.FixedWrap(1).Select(t => int.Parse(t)).ToList().Sum();
                    x = (x == 1 ? 2 : 1);
                    }

                    int resto = soma % 10;
                    int dv = resto == 0 ? 0 : 10 - resto;
                    ieCalculada += dv.ToString().Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 2, 11, 0, 0, 0).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Pará

                case "PA":
                    // Exemplo: Inscrição Estadual 15.250.792-2

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "15")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Paraíba

                case "PB":
                    // Exemplo: Inscrição Estadual 24.073.784-9

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Paraná

                case "PR":
                    // Exemplo: Inscrição Estadual 01.004.823/001-12

                    if (ieSemPicture.Length != 10)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 8).ToString().Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 8).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Pernambuco

                case "PE":
                    // Exemplo: Inscrição Estadual 24.073.784-9

                    if (ieSemPicture.Length <= 9)
                    {
                        ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 2).Trim();

                        // Transforma a string em inteiros
                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                        ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                        // Transforma a string em inteiros
                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                        ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();
                    }
                    else
                    {
                        if (ieSemPicture.Length != 14)
                        {
                            return false;
                        }

                        ieCalculada = ieSemPicture.Substring(0, 13).Trim();

                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();

                        x = 2;
                        soma = 0;

                        for (int i = ieInt.Count - 1; i >= 0; i--)
                        {
                            soma += ieInt[i] * x;

                            x++;
                            if (x > 9)
                            {
                                x = 1;
                            }
                        }

                        dv = 11 - (soma % 11);
                        ieCalculada += (dv > 9 ? dv - 10 : dv).ToString().Trim();
                    }

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Piauí

                case "PI":
                    // Exemplo: Inscrição Estadual 13.200.220-5

                    if (ieSemPicture.Length > 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Rio de Janeiro

                case "RJ":
                    // Exemplo: Inscrição Estadual 75.877.99-4

                    if (ieSemPicture.Length > 8)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 2, 7, 0, 0, 0).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Rio Grande do Norte

                case "RN":
                    // Exemplo: Inscrição Estadual 20.040.040-1(9 dígitos) ou 20.0.040.040-0(10 dígitos)

                    if (ieSemPicture.Length != 9 && ieSemPicture.Length != 10)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "20")
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 2, 10, 0, 0, 0).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Rio Grande do Sul

                case "RS":
                    // Exemplo: Inscrição Estadual 224/365879-2

                    if (ieSemPicture.Length != 10)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Rondônia

                case "RO":
                    // Exemplo: Inscrição Estadual 0000000062521-3

                    if (ieSemPicture.Length != 9 && ieSemPicture.Length != 14)
                    {
                        return false;
                    }

                    if (ieSemPicture.Length == 9)
                    {
                        ieCalculada = ieSemPicture.Substring(3, 5).Trim().PadLeft(13, '0');
                    }
                    else
                    {
                        ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();
                    }

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 2, 9, 0, 0, 1).ToString().Trim();

                    if (ieSemPicture.Length == 9)
                    {
                        ieCalculada = string.Format("{0}{1}", ieSemPicture.Substring(0, 3), ieCalculada.Substring(8, 6));
                    }

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Roraima

                case "RR":
                    // Exemplo: Inscrição Estadual 24.009.176-5

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();

                    var count = 0;
                    soma = 0;
                    foreach (var item in ieInt)
                    {
                        count++;
                        soma += item * count;
                    }

                    ieCalculada += (soma % 9).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Santa Catarina

                case "SC":
                    // Exemplo: Inscrição Estadual 251.040.852

                    if (ieSemPicture.Length != 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region São Paulo

                case "SP":
                    // Exemplo: Inscrição Estadual 01.004.823/001-12

                    if (ieSemPicture.Length != 12)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    // Transforma a string em inteiros
                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    var pesos = new int[]
                    {
                        1,
                        3,
                        4,
                        5,
                        6,
                        7,
                        8,
                        10
                    };
                    soma = 0;

                    for (int i = 0; i < ieInt.Count(); i++)
                    {
                        soma += ieInt[i] * pesos[i];
                    }
                    ieCalculada += (soma % 11).ToString().Trim()[soma % 11 > 9 ? 1 : 0].ToString();

                    if ((ie.Trim()[0] != 'P'))
                    {
                        ieCalculada += ieSemPicture.Substring(9, 2).Trim();

                        // Transforma a string em inteiros
                        ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();

                        x = 2;
                        soma = 0;
                        for (int i = ieInt.Count - 1; i >= 0; i--)
                        {
                            soma += ieInt[i] * x;
                            x++;
                            if (x > 10)
                            {
                                x = 2;
                            }
                        }
                        ieCalculada += (soma % 11).ToString().Trim()[soma % 11 > 9 ? 1 : 0].ToString().Trim();
                    }
                    else
                    {
                        ieCalculada += ieSemPicture.Substring(9, 3).Trim();
                    }

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Sergipe

                case "SE":
                    // Exemplo: Inscrição Estadual 27123456-3

                    if (ieSemPicture.Length > 9)
                    {
                        return false;
                    }

                    ieCalculada = ieSemPicture.Substring(0, 8).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                #endregion

                #region Tocantins

                case "TO":
                    // Exemplo: Inscrição Estadual  29.01.022783-6

                    if (ieSemPicture.Length != 9 && ieSemPicture.Length != 11)
                    {
                        return false;
                    }

                    if (ieSemPicture.Substring(0, 2) != "29")
                    {
                        return false;
                    }

                    if (ieSemPicture.Length == 11)
                    {
                        ieSemPicture = ieSemPicture.Remove(2, 2).Trim();
                    }

                    ieCalculada = ieSemPicture.Substring(0, ieSemPicture.Length - 1).Trim();

                    ieInt = ieCalculada.Select(s => Int32.Parse(s.ToString())).ToList();
                    ieCalculada += CalcularDvModulo11(ieInt, 10).ToString().Trim();

                    return (ieSemPicture.Equals(ieCalculada) ? true : false);

                    #endregion
            }

            return false;
        }

        /// <summary>
        /// Calcula o digito verificador
        /// </summary>
        /// <param name="documento">Lista de inteiro com o documento</param>
        /// <param name="valorBreak">Valor para break</param>
        /// <returns>Digito verificador</returns>
        public static int CalcularDvModulo10(List<int> documento, int valorBreak)
        {
            int x = 2;
            int soma = 0;

            for (int i = documento.Count - 1; i >= 0; i--)
            {
                soma += documento[i] * x;

                x++;
                if (x == valorBreak)
                {
                    x = 2;
                }
            }

            int resto = soma % 10;
            return resto < 1 ? 0 : 10 - resto;
        }

        /// <summary>
        /// Calcula o digito verificador
        /// </summary>
        /// <param name="documento">Lista de inteiro com o documento</param>
        /// <param name="valorBreak">Valor para break</param>
        /// <returns>Digito verificador</returns>
        public static int CalcularDvModulo11(List<int> documento, int valorBreak)
        {
            int x = 2;
            int soma = 0;

            for (int i = documento.Count - 1; i >= 0; i--)
            {
                soma += documento[i] * x;

                x++;
                if (x == valorBreak)
                {
                    x = 2;
                }
            }

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        /// <summary>
        /// Calcula o dígito verificador de uma lista de inteiros
        /// </summary>
        /// <param name="documento">Lista de inteiros que representa um documento (CNPJ, CPF, IE, etc.)</param>
        /// <param name="pesoInicial">Peso inicial, default: 2</param>
        /// <param name="pesoFinal">Peso final, default: 9</param>
        /// <param name="soma">Soma inicial, default: 0</param>
        /// <param name="dvResto10">Valor do digito verificador caso o mesmo seja igual a 10</param>
        /// <param name="dvResto11">Valor do digito verificador caso o mesmo seja igual a 11</param>
        /// <returns>Valor to digito verificador</returns>
        public static int CalcularDvModulo11(List<int> documento, int pesoInicial = 2, int pesoFinal = 9, int soma = 0, int dvResto10 = 0, int dvResto11 = 0)
        {
            int x = pesoInicial;

            for (int i = documento.Count - 1; i >= 0; i--)
            {
                soma += documento[i] * x;

                x++;
                if (x > pesoFinal)
                {
                    x = pesoInicial;
                }
            }

            int dv = 11 - soma % 11;

            if (dv == 10)
            {
                return dvResto10;
            }

            if (dv == 11)
            {
                return dvResto11;
            }

            return dv;
        }

        /// <summary>
        /// Calcula o digito verificador
        /// </summary>
        /// <param name="chaveAcesso">chave de acesso da NFe</param>
        /// <returns>Digito verificador</returns>
        public static int CalcularDvModulo11(string chaveAcesso)
        {
            List<int> ichaveAcesso = chaveAcesso.Substring(0, 43).Select(s => Int32.Parse(s.ToString())).ToList();

            return CalcularDvModulo11(ichaveAcesso, 10);
        }


        /// <summary>
        /// Quebra uma string em uma lista de string conforme o tamanho desejado
        /// </summary>
        /// <param name="text">String com o texto a ser quebrado</param>
        /// <param name="length">Tamanho a ser quebrado</param>
        /// <returns>Lista de strings</returns>
        private static List<String> FixedWrap(string text, int length)
        {
            // Retorna uma lista de string vazias
            if (text == null || text.Length == 0)
            {
                return new List<string>();
            }

            var lines = new List<string>();
            lines.AddRange(Regex.Replace(text, string.Format("(.{{0}})", length.ToString()), "$1§").Split('§'));
            lines.Remove("");
            lines.Remove("");
            return lines;
        }


    }
}
