using System;
using System.Collections.Generic;
using System.Xml.Linq;


public enum SortDirection { //Forma de marcar 2 estados posibles de forma rápida.
    Ascending, 
    Descending
}

public interface IList { //Reglas que las clases deben cumplir.
    void InsertInOrder(int value);
    int DeleteFirst();
    int DeleteLast(); 
    bool DeleteValue(int value); 
    int GetMiddle();
    void MergeSorted(IList listA, IList listB, SortDirection direction); 
}

public class Nodo
{
    public int Dato { get; set; }
    public Nodo Siguiente { get; set; }
    public Nodo Anterior { get; set; }

    public Nodo(int dato)
    {
        Dato = dato;
        Siguiente = null;
        Anterior = null;
    }
}

public class ListaDoble : IList
{
    private Nodo Cabeza;
    private Nodo Cola;
    private int tamaño;
    private Nodo nodoMedio;

    public ListaDoble()
    {
        Cabeza = null;
        Cola = null;
    }
    public void ImprimirLista() //Metodo para imprimir la lista debido a que hay que recorrer la lista para ir imprimiendo cada dato en ella.
    {
        Nodo actual = Cabeza;
        while (actual != null)
        {
            Console.Write(actual.Dato + " ");
            actual = actual.Siguiente;
        }
        Console.WriteLine();
    }
    private void ActualizarMedio() //Actualizamos el nodo del medio.
    {
        if (tamaño == 1) 
        {
            nodoMedio = Cabeza;
        }
        else if (tamaño > 1) 
        {
            Nodo actual = Cabeza;
            for (int i = 0; i < tamaño / 2; i++) 
            {
                actual = actual.Siguiente;
            }
            nodoMedio = actual;
        }
    }

    public void InsertInOrder(int value) //Insertamos un nodo en la lista enlazada de forma ascendente por condicion de la tarea.
    {
        Nodo nuevoNodo = new Nodo(value); 
        tamaño++; 
        if (Cabeza == null) 
        {
            Cabeza = nuevoNodo; 
            Cola = nuevoNodo;
            nodoMedio = Cabeza;
        }

        else 
        {
            if (value < Cabeza.Dato) 
            {
                nuevoNodo.Siguiente = Cabeza;
                Cabeza.Anterior = nuevoNodo;
                Cabeza = nuevoNodo;
            }
            else if (value > Cola.Dato) 
            {
                nuevoNodo.Anterior = Cola;
                Cola.Siguiente = nuevoNodo;
                Cola = nuevoNodo;
            }
            else 
            {
                Nodo actual = Cabeza;
                while (actual.Siguiente != null && actual.Siguiente.Dato < value) 
                {                                           
                    actual = actual.Siguiente;
                }
                nuevoNodo.Siguiente = actual.Siguiente;
                nuevoNodo.Anterior = actual;
                if (actual.Siguiente != null)
                {
                    actual.Siguiente.Anterior = nuevoNodo;
                }
                actual.Siguiente = nuevoNodo;
            }
        }
        ActualizarMedio(); 
    }
    public Nodo GetHead() //Devuelve la cabeza de la lista.
    {
        return Cabeza;
    }

    public int DeleteFirst()
    {
        if (Cabeza == null)
        {
            throw new InvalidOperationException("La lista está vacía.");
        }

        int valor = Cabeza.Dato; 
        Cabeza = Cabeza.Siguiente; 
        if (Cabeza != null)
        {
            Cabeza.Anterior = null; 
        }
        else
        {
            Cola = null;
        }

        tamaño--;
        ActualizarMedio(); 

        return valor;
    }

    public int DeleteLast()
    {
        if (Cola == null)
        {
            throw new InvalidOperationException("La lista está vacía.");
        }

        int valor = Cola.Dato;
        Cola = Cola.Anterior;
        if (Cola != null)
        {
            Cola.Siguiente = null;
        }
        else
        {
            Cabeza = null;
        }

        tamaño--;
        ActualizarMedio(); 

        return valor;
    }

    public void Insertar(int valor) //Insertamos cualquier tipo de lista,sin orden.
    {
        Nodo nuevoNodo = new Nodo(valor);
        if (Cabeza == null)
        {
            Cabeza = nuevoNodo;
            Cola = nuevoNodo;
        }
        else
        {
            Cola.Siguiente = nuevoNodo;
            nuevoNodo.Anterior = Cola;
            Cola = nuevoNodo;
        }
    }
    public void Invert(ListaDoble list)//Invertimos la lista enlazada sin crear una nueva lista.
    {
        if (list.Cabeza == null)
        {
            Console.WriteLine("La lista esta vacía");
            return; 
        }

        Nodo current = list.Cabeza;
        Nodo prev = null;
        Nodo next = null;

        while (current != null) 
        {
            next = current.Siguiente;
            current.Siguiente = prev;
            current.Anterior = next; 
            prev = current; 
            current = next;
        }

        list.Cabeza = prev;
    }
    public bool DeleteValue(int value)//Eliminar el valor dado como argumento de la lista enlazada
    {
        if (Cabeza == null)
        {
            return false;
        }

        if (Cabeza.Dato == value)
        {
            DeleteFirst();
            return true;
        }

        if (Cola.Dato == value)
        {
            DeleteLast();
            return true;
        }

        Nodo actual = Cabeza; 
        while (actual != null) 
        {
            if (actual.Dato == value) 
            {
                actual.Anterior.Siguiente = actual.Siguiente; 
                if (actual.Siguiente != null) 
                {
                    actual.Siguiente.Anterior = actual.Anterior; 
                }
                return true;
            }
            actual = actual.Siguiente;
        }
        return false; 
    }

    public int GetMiddle()//Retorna el dato del medio unicamente accediendo una vez a memoria.
    {
        if (nodoMedio == null)
        {
            throw new InvalidOperationException("La lista está vacía.");
        }

        return nodoMedio.Dato;
    }


    public void MergeSorted(IList listA, IList listB, SortDirection direction)
    {
        //Excepcion si alguna de las listas es nula
        if (listA == null || listB == null)
        {
            throw new ArgumentNullException("Una o ambas listas son nulas.");
        }

        ListaDoble listaA = (ListaDoble)listA;
        ListaDoble listaB = (ListaDoble)listB;

        if (listaA.Cabeza == null)
        {
            if (direction == SortDirection.Ascending)
            {
                listaA.Cabeza = listaB.Cabeza; 
                listaA.Cola = listaB.Cola;
            }
            else if (direction == SortDirection.Descending) 
            {
                Nodo current = listaB.Cabeza;
                Nodo prev = null;
                Nodo next = null;

                while (current != null)
                {
                    next = current.Siguiente;
                    current.Siguiente = prev;
                    current.Anterior = next;
                    prev = current;
                    current = next;
                }

                listaA.Cabeza = prev; 
                listaA.Cola = listaB.Cabeza; 
            }

            return;
        }

        Nodo InvertirLista(Nodo cabeza) 
        {
            Nodo prev = null;
            Nodo current = cabeza;
            Nodo next = null;

            while (current != null)
            {
                next = current.Siguiente;
                current.Siguiente = prev;
                current.Anterior = next;
                prev = current;
                current = next;
            }

            return prev; 
        }

        if (direction == SortDirection.Descending)
        {
            listaA.Cabeza = InvertirLista(listaA.Cabeza);
            listaB.Cabeza = InvertirLista(listaB.Cabeza);
        }

        Nodo currentA = listaA.Cabeza;
        Nodo currentB = listaB.Cabeza;
        Nodo previousA = null;

        if (direction == SortDirection.Ascending)
        {
            while (currentA != null && currentB != null)
            {
                if (currentA.Dato <= currentB.Dato)
                {
                    previousA = currentA; 
                    currentA = currentA.Siguiente;
                }
                else
                {
                    Nodo nextB = currentB.Siguiente; 

                    if (previousA == null) 
                    {
                        listaA.Cabeza = currentB; 
                    }
                    else
                    {
                        previousA.Siguiente = currentB;
                    }

                    currentB.Anterior = previousA;
                    currentB.Siguiente = currentA;

                    if (currentA != null) 
                    {
                        currentA.Anterior = currentB;
                    }
                    else
                    {
                        listaA.Cola = currentB; 
                    }

                    previousA = currentB; 
                    currentB = nextB; 
                }
            }
        }
        else if (direction == SortDirection.Descending) 
        {
            while (currentA != null && currentB != null)
            {
                if (currentA.Dato >= currentB.Dato)  
                {
                    previousA = currentA;
                    currentA = currentA.Siguiente;
                }
                else
                {
                    Nodo nextB = currentB.Siguiente;

                    if (previousA == null)
                    {
                        listaA.Cabeza = currentB;
                    }
                    else
                    {
                        previousA.Siguiente = currentB;
                    }

                    currentB.Anterior = previousA;
                    currentB.Siguiente = currentA;

                    if (currentA != null)
                    {
                        currentA.Anterior = currentB;
                    }
                    else
                    {
                        listaA.Cola = currentB;
                    }

                    previousA = currentB;
                    currentB = nextB;
                }
            }
        }

        if (currentB != null)
        {
            if (previousA == null)
            {
                listaA.Cabeza = currentB;
            }
            else
            {
                previousA.Siguiente = currentB;
            }

            currentB.Anterior = previousA;

            while (currentB.Siguiente != null)
            {
                currentB = currentB.Siguiente;
            }

            listaA.Cola = currentB;
        }

    }


}



public class Program
{
    static void Main(string[] args)
    {
        //Creacion de las listas
        ListaDoble listA = new ListaDoble();
        listA.InsertInOrder(10);
        listA.InsertInOrder(15);

        ListaDoble listB = new ListaDoble();
        listB.InsertInOrder(9);
        listB.InsertInOrder(40);
        listB.InsertInOrder(50);

        //Se unen ambas en descendente
        listA.MergeSorted(listA, listB, SortDirection.Descending);

        //Se imprime el resultado
        Console.WriteLine("Lista fusionada en orden descendente:");
        listA.ImprimirLista();

        //Cuando el usuario presiona una tecla, el programa se cierra.
        Console.ReadKey();
    }
}