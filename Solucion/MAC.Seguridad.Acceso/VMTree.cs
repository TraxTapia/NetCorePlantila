using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    [DataContract]
    public class VMTree<T>
    {
        List<VMTree<T>> nodos;
        T nodo;

        [DataMember]
        public List<VMTree<T>> Nodos { get => nodos; set => nodos = value; }
        [DataMember]
        public T Nodo { get => nodo; set => nodo = value; }
    }
}
