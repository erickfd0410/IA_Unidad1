using System;
using System.Collections.Generic;
class Program
{
    // Estructura simple para representar la orilla ('I' o 'D') de cada uno
    struct Estado
    {
        public char A, P, C, L;

        public Estado(char a, char p, char c, char l)
        {
            A = a; P = p; C = c; L = l;
        }

        // Reglas: Ni Puma con Cabra, ni Cabra con Lechuga si el Arriero no está
        public bool EsValido()
        {
            if (P == C && A != P) return false;
            if (C == L && A != C) return false;
            return true;
        }
        public bool EsMeta()
        {
            return A == 'D' && P == 'D' && C == 'D' && L == 'D';
        }
    }
    static void Main()
    {
        // La pila guarda listas que representan el camino (historial de pasos)
        Stack<List<Estado>> pila = new Stack<List<Estado>>();
        HashSet<string> visitados = new HashSet<string>();

        // Estado inicial: Todos en la izquierda ('I')
        List<Estado> inicio = new List<Estado>();
        inicio.Add(new Estado('I', 'I', 'I', 'I'));
        pila.Push(inicio);

        while (pila.Count > 0)
        {
            List<Estado> camino = pila.Pop();
            Estado actual = camino[camino.Count - 1]; // Último estado alcanzado

            // Crear clave única para saber si ya visitamos este estado
            string clave = "" + actual.A + actual.P + actual.C + actual.L;
            if (visitados.Contains(clave)) continue;
            visitados.Add(clave);

            // ¿Llegamos a la meta?
            if (actual.EsMeta())
            {
                Console.WriteLine("¡Solución encontrada en " + (camino.Count - 1) + " pasos!\n");
                for (int i = 0; i < camino.Count; i++)
                {
                    Estado e = camino[i];
                    Console.WriteLine("Paso " + i + ": Arriero:" + e.A + " | Puma:" + e.P + " | Cabra:" + e.C + " | Lechuga:" + e.L);
                }
                return;
            }

            char siguienteOrilla = (actual.A == 'I') ? 'D' : 'I';

            // Probar las 4 opciones posibles
            List<Estado> opciones = new List<Estado>();
            opciones.Add(new Estado(siguienteOrilla, actual.P, actual.C, actual.L)); // Arriero solo

            if (actual.P == actual.A)
                opciones.Add(new Estado(siguienteOrilla, siguienteOrilla, actual.C, actual.L)); // Con Puma

            if (actual.C == actual.A)
                opciones.Add(new Estado(siguienteOrilla, actual.P, siguienteOrilla, actual.L)); // Con Cabra

            if (actual.L == actual.A)
                opciones.Add(new Estado(siguienteOrilla, actual.P, actual.C, siguienteOrilla)); // Con Lechuga
            // Agregar opciones válidas a la pila
            foreach (Estado op in opciones)
            {
                if (op.EsValido())
                {
                    List<Estado> nuevoCamino = new List<Estado>(camino);
                    nuevoCamino.Add(op);
                    pila.Push(nuevoCamino);
                }
            }
        }
    }
}