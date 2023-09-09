using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using JsonSerializer = System.Text.Json.JsonSerializer;


class Principal
{
    public class User // Usuario que se separa entre Nombre, DPI, Fecha de nacimiento y Dirección//
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public string dateBirth { get; set; }
        public string address { get; set; }
    }
    public class UsersRegistry //Lista de objetos User donde serán enviados los datos del usuario para poder ser inspeccionados//
    {
        public User[]? Users { get; set; }
    }

    public class Arbol
    {
        int contador;
        User[] arbol = new User[128];
        User NewData;
        int n = 0;
        public void Add(User Node, double nivel)
        {
            int tempContador = contador;
            arbol[contador] = Node;
            contador++;
            int NivelCalc = 1;
            while (tempContador != 0)
            {
                while (Math.Pow(n,2) < tempContador && NivelCalc == 1)
                {
                        n++;
                }
                NivelCalc--;
                User Hijo = arbol[tempContador];
                int countpadre = Padre(tempContador);
                User padre = arbol[countpadre];
                int CompareValue = padre.name.CompareTo(Hijo.name);
                if (CompareValue > 0 && n > 0)
                {
                    User TemporalNodo = Hijo;
                    arbol[tempContador] = padre;
                    arbol[countpadre] = TemporalNodo;
                    n--;
                    tempContador = countpadre;
                }
                else
                {
                    return;                 
                }
            }
        }

        //Delete//
        public User Delete(User Delete)
        {
            User Node = arbol[0];
            if (contador != 0)
            {
                User padre = new User();
                int i = 0;
                while (arbol[i].name != Delete.name)
                {
                    if(arbol[i].name == Delete.name)
                    {
                        padre = arbol[i];
                    }
                    i++;
                }
                int contadorIz = HijoIz(0);
                int contadorDr = HijoDr(0);
                int contadorpadre = 0;
                User izquierdo = arbol[contadorIz];
                User derecho = arbol[contadorDr];
                while (izquierdo != null || derecho != null)
                {
                    if (izquierdo != null && derecho != null)
                    {
                        if (izquierdo.name.CompareTo(derecho.name) < 0)
                        {
                            if (izquierdo.name.CompareTo(padre.name) < 0)
                            {

                                User TemporalNodo = izquierdo;
                                arbol[contadorIz] = padre;
                                arbol[contadorpadre] = TemporalNodo;
                                contadorpadre = contadorIz;
                            }
                            else
                            {
                                contador--;
                                return Node;
                            }
                        }
                        else
                        {
                            if (derecho.name.CompareTo(padre.name) < 0)
                            {
                                User TemporalNodo = derecho;
                                arbol[contadorDr] = padre;
                                arbol[contadorpadre] = TemporalNodo;
                                contadorpadre = contadorDr;
                            }
                            else
                            {
                                contador--;
                                return Node;
                            }
                        }
                    }
                    else if (derecho != null)
                    {
                        if (derecho.name.CompareTo(padre.name) < 0)
                        {
                            User TemporalNodo = derecho;
                            arbol[contadorDr] = padre;
                            arbol[contadorpadre] = TemporalNodo;
                            contadorpadre = contadorDr;
                        }
                        else
                        {
                            contador--;
                            return Node;
                        }
                    }
                    else
                    {
                        if (izquierdo.name.CompareTo(padre.name) < 0)
                        {

                            User TemporalNodo = izquierdo;
                            arbol[contadorIz] = padre;
                            arbol[contadorpadre] = TemporalNodo;
                            contadorpadre = contadorIz;
                        }
                        else
                        {
                            contador--;
                            return Node;
                        }
                    }
                    contadorIz = HijoIz(contadorpadre);
                    contadorDr = HijoDr(contadorpadre);
                    izquierdo = arbol[contadorIz];
                    derecho = arbol[contadorDr];
                    padre = arbol[contadorpadre];
                }
                if(izquierdo == null && derecho == null)
                {
                    arbol[i] = null;
                }
                contador--;

            }
            return Node;
        }

        public User Search(User Search, int WhatSearch, bool NeedPatch)
        {
            int i = 0;
            if (contador != 0)
            {
                while (arbol[i].name != Search.name)
                {
                    i++;
                    if(arbol[i] == null)
                    {
                        Search.name = "No encontrado";
                        return Search;
                    }
                }
                if(NeedPatch == true)
                {
                    if (arbol[i].name == Search.name)
                    {
                        if (WhatSearch == 0)
                        {
                            arbol[i].address = Search.address;
                        }
                        else
                        {
                            arbol[i].dateBirth = Search.dateBirth;
                        }
                    }
                }
            }
            return arbol[i];
        }

        public User[] Llenar(int Size)
        {
            User[] Recolector = new User[Size];
            for(int i = 0; i < Size; i++)
            {
                Recolector[i] = arbol[i];
            }
            return Recolector;
        }
        public int Padre(int n)
        {
            if (n % 2 == 0) // Es par//
            {
                return ((n / 2) - 1);
            }
            else //Es impar//
            {
                return ((n - 1) / 2);
            }
        }
        public int HijoIz(int n)
        {
            return 2 * n + 1;
        }
        public int HijoDr(int n)
        {
            return 2 * (n + 1);
        }

        public static void Main(string[] args)
        {
            int contador = 0;
            double nivel = 0;
            Arbol tree = new Arbol();
                // Se consiguen los datos del JSON//
                string jsonText = File.ReadAllText(@"C:\Users\oswal\OneDrive\Documentos\Universidad\6. 4to Ciclo\Estructura de datos\Labs\Lab 1\Laboratorio1\ConsoleApp1\datos.json");
                string[] jsonObjects = jsonText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int Size = jsonObjects.Length - 1;
                string[] jsonObjectsT = new string[Size];
                string[] jsonObjectsData = new string[Size];
                string[] jsonObjectsAction = new string[Size];
                for (int i = 0; i < Size; i++)
                {
                    jsonObjectsT = jsonObjects[i].Split(';', 2);
                    jsonObjectsAction[i] = jsonObjectsT[0];
                }
                for (int i = 0; i < Size; i++)
                {
                    jsonObjectsT = jsonObjects[i].Split(';',2);
                    jsonObjectsData[i] = jsonObjectsT[1];
                }
                //Se consiguen los datos del JSON//

                for(int i = 0; i < Size - 1; i++)
                {
                string eleccion = "";
                bool NeedPatch = false;

                User input = JsonSerializer.Deserialize<User>(jsonObjectsData[i])!;
                eleccion = jsonObjectsAction[i];
                switch (eleccion)
                    {
                        case "INSERT": //Insertar registro de persona//}
                        User TempUser = new User();
                        TempUser.name = input.name.ToString();
                        TempUser.dpi = input.dpi.ToString();
                        TempUser.address = input.address.ToString();
                        TempUser.dateBirth = input.dateBirth.ToString();
                        if(contador >= Math.Pow(2, nivel))
                        {
                            nivel++;
                        }
                        tree.Add(TempUser,nivel);
                        break;
                        case "DELETE": //Eliminar registro de persona//
                        User NeedDelete = new User();
                        NeedDelete.name = input.name.ToString();
                        NeedDelete.dpi = input.dpi.ToString();
                        tree.Delete(NeedDelete);
                        break;
                        case "PATCH": //Actualizar registro//
                        User Patch = new User();
                        Patch.name = input.name.ToString();
                        Patch.dpi = input.dpi.ToString();
                        int WhatSearch = 0;
                        NeedPatch = true;
                        if(input.address != null)
                        {
                            Patch.address = input.address.ToString();
                        }
                        if(input.dateBirth != null)
                        {
                            Patch.dateBirth = input.dateBirth.ToString();
                            WhatSearch++;
                        }
                        if(NeedPatch == true)
                        {
                            if(WhatSearch == 0)
                            {
                                input.address = tree.Search(Patch, WhatSearch, NeedPatch).address;
                            }
                            else
                            {
                                input.dateBirth = tree.Search(Patch, WhatSearch, NeedPatch).dateBirth;
                            }
                        }
                        else
                        {
                            User Busqueda = tree.Search(Patch, WhatSearch, NeedPatch);
                            if(Busqueda == null)
                            {
                                Console.WriteLine("No se encontró a la persona");
                            }
                        }

                        int Searcher = 0;
                        break;
                }
            }
            User[] Show = new User[Size];
            int HowMuchWrite = tree.contador;
            for(int i = 0; i <= HowMuchWrite; i++)
            {
                Show[i] = tree.arbol[i];
                if(Show[i] == null)
                {
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Name: "+Show[i].name + ". dpi:"+Show[i].dpi+". address:"+Show[i].address+". dateBirth: "+Show[i].dateBirth+".");
                }
            }
            int Choose = 1;
            while (Choose != 2)
            {
                Console.WriteLine("¿Desea realizar una búsqueda?" + Environment.NewLine + "1. Si." + Environment.NewLine + "2. No.");
                Choose = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                if (Choose == 1)
                {
                    User TakeData = new User();
                    Console.WriteLine("¿A quien desea buscar? (Por nombre)");
                    TakeData.name = Console.ReadLine();
                    User ShowSearched = new User();
                    ShowSearched = tree.Search(TakeData, 0, false);
                    if(ShowSearched != null)
                    {
                        Console.WriteLine("Quien buscaba fue encontrado, es:");
                        Console.WriteLine("Name: " + ShowSearched.name + ". dpi:" + ShowSearched.dpi + ". address:" + ShowSearched.address + ". dateBirth: " + ShowSearched.dateBirth + ".");
                    }
                    else
                    {
                        Console.WriteLine("Quien buscaba no se encuentra en la base de datos");
                    }
                }
            }
        }
    }
}