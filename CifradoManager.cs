using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAa
{
    public class CifradoManager
    {
        private Dictionary<char, string> tablaCifrado;

        public CifradoManager()
        {
            tablaCifrado = new Dictionary<char, string>();
        }

        public void GenerarTablaCifrado(int blockSize)
        {
            tablaCifrado.Clear();
            Random random = new Random();

            for (int i = 0; i < 256; i++)
            {
                string valor = "";
                for (int j = 0; j < blockSize; j++)
                {
                    valor += random.Next(0, 256).ToString("X2");
                }
                tablaCifrado.Add((char)i, valor);
            }
        }

        public string CifrarTexto(string input, int blockSize)
        {
            StringBuilder resultado = new StringBuilder();

            foreach (char c in input)
            {
                if (tablaCifrado.ContainsKey(c))
                {
                    resultado.Append(tablaCifrado[c]);
                }
            }

            return resultado.ToString();
        }

        public string DescifrarTexto(string input, int blockSize)
        {
            StringBuilder resultado = new StringBuilder();

            for (int i = 0; i < input.Length; i += blockSize * 2)
            {
                string fragmento = input.Substring(i, blockSize * 2);
                foreach (var par in tablaCifrado)
                {
                    if (par.Value == fragmento)
                    {
                        resultado.Append(par.Key);
                        break;
                    }
                }
            }

            return resultado.ToString();
        }

        public void GuardarTablaCifrado(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var par in tablaCifrado)
                {
                    writer.WriteLine($"{(int)par.Key:X2}={par.Value}");
                }
            }
        }

        public void CargarTablaCifrado(string filePath)
        {
            tablaCifrado.Clear();
            foreach (var linea in File.ReadLines(filePath))
            {
                var partes = linea.Split('=');
                char clave = (char)Convert.ToInt32(partes[0], 16);
                string valor = partes[1];
                tablaCifrado[clave] = valor;
            }
        }
    }
}
