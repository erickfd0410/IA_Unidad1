using System;
using System.Collections.Generic;
using System.Linq;

class NodoAEstrella
{
    public string Nombre { get; set; }
    public int G { get; set; } // Costo desde el origen hasta este nodo
    public int H { get; set; } // Heurística (distancia estimada al destino)
    public int F => G + H;     // f(n) = g(n) + h(n)
    public NodoAEstrella Padre { get; set; }

    public NodoAEstrella(string nombre, int g, int h, NodoAEstrella padre = null)
    {
        Nombre = nombre;
        G = g;
        H = h;
        Padre = padre;
    }
}

class Program
{
    // Lista de ciudades ordenadas alfabéticamente
    static string[] ciudades = new string[]
    {
        "Arad", "Bucharest", "Craiova", "Drobeta", "Eforie",
        "Fagaras", "Giurgiu", "Hirsova", "Iasi", "Lugoj",
        "Mehadia", "Neamt", "Oradea", "Pitesti", "Rimnku Vilcea",
        "Sibiu", "Timisoara", "Urziceni", "Vaslui", "Zerind"
    };

    // Matriz de distancias g(n) entre ciudades conexas
    static int[,] matrizAdyacencia = new int[20, 20]
    {
        // Arad, Buch, Crai, Drob, Efor, Faga, Giur, Hirs, Iasi, Lugoj, Meha, Neam, Orad, Pite, Rimn, Sibi, Timi, Urzi, Vasl, Zeri
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  140,  118,    0,    0,   75 }, // Arad
        {    0,    0,    0,    0,    0,  211,   90,    0,    0,    0,    0,    0,    0,  101,    0,    0,    0,   85,    0,    0 }, // Bucharest
        {    0,    0,    0,  120,    0,    0,    0,    0,    0,    0,    0,    0,    0,  138,  146,    0,    0,    0,    0,    0 }, // Craiova
        {    0,    0,  120,    0,    0,    0,    0,    0,    0,    0,   75,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, // Drobeta
        {    0,    0,    0,    0,    0,    0,    0,   86,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, // Eforie
        {    0,  211,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   99,    0,    0,    0,    0 }, // Fagaras
        {    0,   90,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, // Giurgiu
        {    0,    0,    0,    0,   86,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   98,    0,    0 }, // Hirsova
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   87,    0,    0,    0,    0,    0,    0,   92,    0 }, // Iasi
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   70,    0,    0,    0,    0,    0,  111,    0,    0,    0 }, // Lugoj
        {    0,    0,    0,   75,    0,    0,    0,    0,    0,   70,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, // Mehadia
        {    0,    0,    0,    0,    0,    0,    0,    0,   87,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, // Neamt
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  151,    0,    0,    0,   71 }, // Oradea
        {    0,  101,  138,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   97,    0,    0,    0,    0,    0 }, // Pitesti
        {    0,    0,  146,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   97,    0,   80,    0,    0,    0,    0 }, // Rimnicu Vilcea
        {  140,    0,    0,    0,    0,   99,    0,    0,    0,    0,    0,    0,  151,    0,   80,    0,    0,    0,    0,    0 }, // Sibiu
        {  118,    0,    0,    0,    0,    0,    0,    0,    0,  111,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, // Timisoara
        {    0,   85,    0,    0,    0,    0,    0,   98,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  142,    0 }, // Urziceni
        {    0,    0,    0,    0,    0,    0,    0,    0,   92,    0,    0,    0,    0,    0,    0,    0,    0,  142,    0,    0 }, // Vaslui
        {   75,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   71,    0,    0,    0,    0,    0,    0,    0 }  // Zerind
    };

    // Heurística h(n): Distancia en línea recta a Bucharest
    static Dictionary<string, int> heuristica = new Dictionary<string, int>()
    {
        { "Arad", 366 }, { "Bucharest", 0 }, { "Craiova", 160 }, { "Drobeta", 242 },
        { "Eforie", 161 }, { "Fagaras", 178 }, { "Giurgiu", 77 }, { "Hirsova", 151 },
        { "Iasi", 226 }, { "Lugoj", 244 }, { "Mehadia", 241 }, { "Neamt", 234 },
        { "Oradea", 380 }, { "Pitesti", 98 }, { "Rimnku Vilcea", 193 }, { "Sibiu", 253 },
        { "Timisoara", 329 }, { "Urziceni", 80 }, { "Vaslui", 199 }, { "Zerind", 374 }
    };

    static void Main()
    {
        string inicio = "Arad";
        string destino = "Bucharest";

        Console.WriteLine($"Calculando el camino más corto con A* de {inicio} a {destino}...\n");

        List<string> camino = AlgoritmoAEstrella(inicio, destino, out int costoTotal);

        if (camino != null)
        {
            Console.WriteLine("¡Camino óptimo encontrado!");
            Console.WriteLine(string.Join(" -> ", camino));
            Console.WriteLine($"Distancia total recorrida: {costoTotal} km");
        }
        else
        {
            Console.WriteLine("No se encontró ningún camino.");
        }
    }


    static List<string> AlgoritmoAEstrella(string inicio, string destino, out int costoTotal)
    {
        costoTotal = 0;

        // Lista abierta (nodos por evaluar) y cerrada (nodos ya evaluados)
        List<NodoAEstrella> listaAbierta = new List<NodoAEstrella>();
        Dictionary<string, int> listaCerrada = new Dictionary<string, int>();

        // Agregar el nodo inicial
        listaAbierta.Add(new NodoAEstrella(inicio, 0, heuristica[inicio]));

        while (listaAbierta.Count > 0)
        {
            // Seleccionar el nodo con el menor valor de F = G + H
            NodoAEstrella actual = listaAbierta.OrderBy(n => n.F).ThenBy(n => n.H).First();

            // Si llegamos al destino, reconstruimos el camino
            if (actual.Nombre == destino)
            {
                costoTotal = actual.G;
                return ReconstruirCamino(actual);
            }

            listaAbierta.Remove(actual);

            // Marcar el nodo como visitado guardando su mejor G conocido
            if (listaCerrada.ContainsKey(actual.Nombre))
            {
                if (listaCerrada[actual.Nombre] <= actual.G)
                    continue;
                listaCerrada[actual.Nombre] = actual.G;
            }
            else
            {
                listaCerrada.Add(actual.Nombre, actual.G);
            }

            // Obtener vecinos conectados
            int idxActual = Array.IndexOf(ciudades, actual.Nombre);

            for (int i = 0; i < ciudades.Length; i++)
            {
                int distanciaDirecta = matrizAdyacencia[idxActual, i];

                // Si existe conexión entre las ciudades
                if (distanciaDirecta > 0)
                {
                    string vecina = ciudades[i];
                    int nuevoG = actual.G + distanciaDirecta;

                    // Si ya fue procesada con un menor costo, omitir
                    if (listaCerrada.ContainsKey(vecina) && listaCerrada[vecina] <= nuevoG)
                        continue;

                    // Buscar si ya está en la lista abierta con un costo mayor
                    NodoAEstrella nodoEnAbierta = listaAbierta.FirstOrDefault(n => n.Nombre == vecina);

                    if (nodoEnAbierta == null)
                    {
                        // Agregar nuevo nodo a evaluar
                        listaAbierta.Add(new NodoAEstrella(vecina, nuevoG, heuristica[vecina], actual));
                    }
                    else if (nuevoG < nodoEnAbierta.G)
                    {
                        // Actualizar a una mejor ruta
                        nodoEnAbierta.G = nuevoG;
                        nodoEnAbierta.Padre = actual;
                    }
                }
            }
        }

        return null;
    }

    static List<string> ReconstruirCamino(NodoAEstrella nodoFinal)
    {
        List<string> camino = new List<string>();
        NodoAEstrella paso = nodoFinal;

        while (paso != null)
        {
            camino.Add(paso.Nombre);
            paso = paso.Padre;
        }

        camino.Reverse();
        return camino;
    }
}