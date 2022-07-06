using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MAC.Serguridad.Acceso
{
   [DataContract]
    public class VMMapa<N, T>
    {
        List<VMTree<T>> nodos;
        N nodo;

        [DataMember]
        public List<VMTree<T>> Nodos { get => nodos; set => nodos = value; }
        [DataMember]
        public N Nodo { get => nodo; set => nodo = value; }
    }

}
