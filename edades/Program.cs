using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace edades
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Solicita al usuario ingresar la ruta del archivo
            Console.Write("Ingrese la ruta del archivo CSV: ");
            string rutaArchivo = Console.ReadLine();

            // Verifica si el archivo existe, si no, muestra un mensaje de error y termina
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("El archivo especificado no existe.");
                return;
            }

            try
            {
                // Almacena las edades como claves y el número de personas como valores
                var frecuencias = new Dictionary<int, int>();

                using (StreamReader sr = new StreamReader(rutaArchivo))
                {
                    string linea;
                    bool esPrimeraLinea = true;

                    // Leer el archivo CSV línea por línea
                    while ((linea = sr.ReadLine()) != null)
                    {
                        if (esPrimeraLinea)
                        {
                            esPrimeraLinea = false;
                            continue; // Ignorar encabezado
                        }

                        // Divide la línea por comas en dos columnas
                        string[] columnas = linea.Split(',');

                        // Verificar si el formato es correcto
                        if (columnas.Length != 2)
                        {
                            Console.WriteLine("Línea con formato incorrecto: " + linea);
                            continue;
                        }

                        // Informa si la línea no tiene las dos columnas o si los valores no son enteros
                        if (int.TryParse(columnas[0], out int edad) && int.TryParse(columnas[1], out int numeroDePersonas))
                        {
                            // Contar las frecuencias de las edades
                            if (!frecuencias.ContainsKey(edad))
                            {
                                frecuencias[edad] = 0;
                            }
                            frecuencias[edad] += numeroDePersonas; // Acumula el número de personas para cada edad
                        }
                        else
                        {
                            Console.WriteLine("Error en los datos: " + linea);
                        }
                    }
                }

                // Mostrar el título del eje Y
                Console.WriteLine("\nNúmero de Personas");

                // Muestra el histograma vertical en la consola
                Console.WriteLine("\nHistograma vertical de edades:");

                // Ordena las edades de menor a mayor y obtiene la altura máxima
                var edadesOrdenadas = new List<int>(frecuencias.Keys);
                edadesOrdenadas.Sort();
                int alturaMaxima = 0;
                foreach (var valor in frecuencias.Values)
                {
                    if (valor > alturaMaxima)
                    {
                        alturaMaxima = valor;
                    }
                }

                // Dibuja el histograma desde la altura máxima hacia abajo
                for (int i = alturaMaxima; i > 0; i--)
                {
                    // Imprime los números del eje Y
                    Console.Write($"{i:D2} | "); // Etiqueta del eje Y

                    foreach (var edad in edadesOrdenadas)
                    {
                        if (frecuencias[edad] >= i)
                        {
                            Console.Write(" * ");
                        }
                        else
                        {
                            Console.Write("   "); // Espacio vacío para alinear
                        }
                    }
                    Console.WriteLine(); // Siguiente nivel
                }

                // Línea base del histograma
                Console.Write("   +");
                for (int i = 0; i < edadesOrdenadas.Count; i++)
                {
                    Console.Write("---"); // Línea divisoria horizontal
                }
                Console.WriteLine();

                // Imprimir las edades en el eje X
                Console.Write("     "); // Espacio para alinear con el eje Y
                foreach (var edad in edadesOrdenadas)
                {
                    Console.Write($" {edad:D2}");
                }
                Console.WriteLine();

                // Mostrar el título del eje X
                Console.WriteLine("\n              Edades");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al procesar el archivo: {e.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
