using System;
using System.Collections.Generic;

namespace LigaFutbolConsola
{
    class Equipo
    {
        public string Nombre { get; set; }
        public int PJ { get; private set; }
        public int PG { get; private set; }
        public int PE { get; private set; }
        public int PP { get; private set; }
        public int GF { get; private set; }
        public int GC { get; private set; }
        public int PTS { get; private set; }

        public int DG => GF - GC;

        public Equipo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del equipo no puede ser vacío o nulo", nameof(nombre));
            }

            Nombre = nombre;
        }

        public void ActualizarEstadisticas(int golesFavor, int golesContra)
        {
            PJ++;
            GF += golesFavor;
            GC += golesContra;

            if (golesFavor > golesContra)
            {
                PG++;
                PTS += 3;
            }
            else if (golesFavor == golesContra)
            {
                PE++;
                PTS += 1;
            }
            else
            {
                PP++;
            }
        }

        public override string ToString()
        {
            return $"{Nombre,-15} PJ: {PJ} PG: {PG} PE: {PE} PP: {PP} GF: {GF} GC: {GC} DG: {DG} PTS: {PTS}";
        }
    }

    class Liga
    {
        private List<Equipo> equipos = new List<Equipo>();

        public void AgregarEquipo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre del equipo no puede ser vacío.");
                return;
            }

            if (equipos.Exists(e => e.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("El equipo ya está registrado.");
                return;
            }

            equipos.Add(new Equipo(nombre));
            Console.WriteLine($"Equipo '{nombre}' agregado.");
        }

        public void RegistrarPartido(string nombreLocal, int golesLocal, string nombreVisitante, int golesVisitante)
{
    // Validamos que los nombres de los equipos no sean nulos ni vacíos
    if (string.IsNullOrWhiteSpace(nombreLocal) || string.IsNullOrWhiteSpace(nombreVisitante))
    {
        Console.WriteLine("Los nombres de los equipos no pueden ser vacíos o nulos.");
        return;
    }

    // Buscamos los equipos en la lista, y usamos FirstOrDefault para que no se genere una excepción si no se encuentra un equipo
    Equipo? equipoLocal = equipos.FirstOrDefault(e => e.Nombre.Equals(nombreLocal, StringComparison.OrdinalIgnoreCase));
    Equipo? equipoVisitante = equipos.FirstOrDefault(e => e.Nombre.Equals(nombreVisitante, StringComparison.OrdinalIgnoreCase));

    // Verificamos si los equipos se encontraron en la lista
    if (equipoLocal == null || equipoVisitante == null)
    {
        Console.WriteLine("Uno o ambos equipos no están registrados.");
        return;
    }

    // Si los equipos existen, actualizamos sus estadísticas
    equipoLocal.ActualizarEstadisticas(golesLocal, golesVisitante);
    equipoVisitante.ActualizarEstadisticas(golesVisitante, golesLocal);
    Console.WriteLine("Partido registrado correctamente.");
}


        public void MostrarTablaPosiciones()
        {
            Console.WriteLine("\nTabla de Posiciones:");
            Console.WriteLine("Equipo           PJ  PG  PE  PP  GF  GC  DG  PTS");

            // Ordenando por PTS de forma descendente y comprobando si la lista está vacía
            if (equipos.Count > 0)
            {
                equipos.Sort((a, b) => b.PTS.CompareTo(a.PTS));
                foreach (var equipo in equipos)
                {
                    Console.WriteLine(equipo);
                }
            }
            else
            {
                Console.WriteLine("No hay equipos registrados.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Liga liga = new Liga();
            string opcion;

             do
            {
                Console.WriteLine("\nSistema Liga Profesional de Fútbol - Ecuador");
                Console.WriteLine("1. Agregar Equipo");
                Console.WriteLine("2. Registrar Partido");
                Console.WriteLine("3. Mostrar Tabla de Posiciones");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                opcion = Console.ReadLine() ?? "0"; // Valor predeterminado en caso de que ReadLine sea nulo

                switch (opcion)
                {
                    case "1":
                        Console.Write("Nombre del equipo: ");
                        string? nombreEquipo = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nombreEquipo))
                        {
                            Console.WriteLine("El nombre del equipo no puede estar vacío.");
                            continue; // Vuelve a la opción del menú
                        }
                        liga.AgregarEquipo(nombreEquipo);
                        break;
                    case "2":
                        Console.Write("Nombre del equipo local: ");
                        string? equipoLocal = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(equipoLocal))
                        {
                            Console.WriteLine("El nombre del equipo local no puede estar vacío.");
                            continue; // Vuelve a la opción del menú
                        }
                        
                        Console.Write("Goles del equipo local: ");
                        bool esNumeroLocal = int.TryParse(Console.ReadLine(), out int golesLocal);

                        Console.Write("Nombre del equipo visitante: ");
                        string? equipoVisitante = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(equipoVisitante))
                        {
                            Console.WriteLine("El nombre del equipo visitante no puede estar vacío.");
                            continue; // Vuelve a la opción del menú
                        }

                        Console.Write("Goles del equipo visitante: ");
                        bool esNumeroVisitante = int.TryParse(Console.ReadLine(), out int golesVisitante);

                        if (esNumeroLocal && esNumeroVisitante)
                        {
                            liga.RegistrarPartido(equipoLocal, golesLocal, equipoVisitante, golesVisitante);
                        }
                        else
                        {
                            Console.WriteLine("Datos del partido inválidos. Intente de nuevo.");
                        }
                        break;
                    case "3":
                        liga.MostrarTablaPosiciones();
                        break;
                    case "0":
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            } while (opcion != "0");
        }
    }
}