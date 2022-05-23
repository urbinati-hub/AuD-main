using System;
namespace AuD_Praktikum
{
    interface IDictionary
    {
        bool search(int elem);
        // true = gefunden
       

        

        bool insert(int elem);
        // true = hinzugefügt
        

        

        bool delete(int elem);
        // true = gelöscht
        

        

        void print();
        

        


    }

    interface ISetSorted : IDictionary
    {

    }

    interface ISetUnsorted : IDictionary
    {

    }

    interface IMultiSetSorted : IDictionary
    {

    }

    interface IMultiSetUnsorted : IDictionary
    {

    }
}
