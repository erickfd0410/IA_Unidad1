using System;

class Program
{
    static void Main()
    {
        //Generar número aleatorio entre 1 y 100
        Random random = new Random();
        int numeroSecreto = random.Next(1, 101); //101 es exclusivo

        int intento = 0;
        int intentosRealizados = 0;
        bool adivinado = false;
        Console.WriteLine("  ¡Bienvenido al juego del Número Secreto!\n");
        Console.WriteLine("He elegido un número entre 1 y 100.");
        Console.WriteLine("Intenta adivinarlo en el menor número de intentos.\n");

        //Bucle principal
        while (!adivinado)
        {
            Console.Write("Introduce tu intento: ");
            string entrada = Console.ReadLine();

            //Validación de la entrada del usuario
            if (!int.TryParse(entrada, out intento) || intento < 1 || intento > 100)
            {
                Console.WriteLine("Por favor, introduce un número válido entre 1 y 100.\n");
                continue;
            }

            intentosRealizados++;

            //Evalua el intento
            if (intento == numeroSecreto)
            {
                adivinado = true;
                Console.WriteLine($"\nAdivinaste el número secreto ({numeroSecreto}).");
                Console.WriteLine($"Total de intentos: {intentosRealizados}");
            }
            else if (intento < numeroSecreto)
            {
                Console.WriteLine("El número secreto es MAYOR. Intenta de nuevo.\n");
            }
            else
            {
                Console.WriteLine("El número secreto es MENOR. Intenta de nuevo.\n");
            }
        }
    }
}