using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
namespace MAC.ViewModel.Layout
{

    [DataContract]
    public class VMTabla
    {
        private string _titulo = String.Empty;
        public String titulo { get => _titulo; set => _titulo = value; }

        private DefinicionTabla _DefinicionTabla = new DefinicionTabla();
        [DataMember]
        public DefinicionTabla cDefinicionTabla { get => _DefinicionTabla; set => _DefinicionTabla = value; }

        private DefinicionHTML _DefinicionHTML = new DefinicionHTML();
        [DataMember]
        public DefinicionHTML cDefinicionHTML { get => _DefinicionHTML; set => _DefinicionHTML = value; }

        DefinicionJSON _definicion = new DefinicionJSON();
        public DefinicionJSON cDefinicionJSON { get => _definicion; set => _definicion=value; }

        public class DefinicionTabla
        {

        }

        public class DefinicionHTML
        {

        }
    }

    [DataContract]
    public class DefinicionJSON
    {
        [DataMember]
        public String source { get; set; }

       [DataMember]
        public List<Object> jsonData { get; set;}

        [DataMember]
        public Object newRowModel { get; set; }

        private short _orderByColumn =0;
        [DataMember]
        public short orderByColumn { get => _orderByColumn; set => _orderByColumn = value; }

        private String _orderByDirec = "ASC";
        [DataMember]
        public String orderByDirec { get => _orderByDirec; set => _orderByDirec = value; }

        [DataMember]
        public String scrollY { get; set; }

        private List<column> _columns = new List<column>();
        [DataMember]
        public List<column> columns { get => _columns; set => _columns = value; }

        private String _fnRowCallback = null;
        [DataMember]
        public String fnRowCallback { get => _fnRowCallback; set => _fnRowCallback = value; }

        private SelectKey _objSelectKey = null;
        [DataMember]
        public SelectKey objSelectKey { get => _objSelectKey; set => _objSelectKey = value; }

        [DataContract]
        public class column
        {

            public column()
            {

            }

            public column(String ptitle, String pdata, String prender)
            {
                _title = ptitle;
                _data = pdata;
                _render = prender;
            }

            private String _title = String.Empty;
            [DataMember]
            public String title { get => _title; set => _title = value; }

            private String _data = String.Empty;
            [DataMember]
            public String data { get => _data; set => _data = value; }

            private String _render = null;
            [DataMember]
            public String render { get => _render; set => _render = value; }

            private Boolean _visible = true;
            [DataMember]
            public Boolean visible { get => _visible; set => _visible = value; }
        }

        [DataContract]
        public class SelectKey
        {
            public SelectKey()
            {

            }
            public SelectKey(short pcolumn, String prefUrl)
            {
                _column = pcolumn;
                _refUrl = prefUrl;
            }

            private short _column = 0;
            [DataMember]
            public short column { get => _column; set => _column = value; }

            private String _refUrl = String.Empty;
            [DataMember]
            public String refUrl { get => _refUrl; set => _refUrl = value; }
        }
    }

    [DataContract]
    public class Orden
    {
        [DataMember]
        public short index { get; set; }

        [DataMember]
        public String direccion { get; set; }
    }

    [DataContract]
    public class VMTablaModel<T>
    {
        DefinicionJSON _definicion = new DefinicionJSON();
        public DefinicionJSON cDefinicionJSON { get => _definicion; set => _definicion = value; }

        public List<T> Datos { get; set; }
        public T NewRow { get; set; }

        public int getColIndex(String propiedad)
        {
            int index = 0;
            PropertyInfo[] propInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            index = propInfos.ToList().FindIndex(x => x.Name.Equals(propiedad));
            return index;
        }

        public void addColumnOption(String titulo,String funRender)
        {
            _definicion.columns.Add(new DefinicionJSON.column(titulo, null, funRender));
        }

        public VMTablaModel(T model,String urlData, short indexOrden, 
            String direccion, String scrollY, String fnRowCallback=null, short indexSelec=0, string urlSelec="")
        {
            _definicion.source = urlData;
            _definicion.orderByColumn= indexOrden;
            _definicion.orderByDirec = direccion;
            _definicion.scrollY = scrollY;
            _definicion.fnRowCallback = fnRowCallback;
             String funRender = String.Empty;
            PropertyInfo[] propInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Type type;
            String campo = String.Empty;
            String titulo = String.Empty;
            foreach (var prop in propInfos) {
                type = prop.PropertyType;
                campo = prop.Name;
                titulo = campo;
                funRender = type == typeof(DateTime) || type == typeof(DateTime?) ? "dateColumn" : String.Empty;
                IEnumerable<DisplayAttribute> propertyAttributes = prop.GetCustomAttributes<DisplayAttribute>();
                if (propertyAttributes!=null && propertyAttributes.Count() > 0)
                {
                    titulo = propertyAttributes.ToArray()[0].Description;
                    if (String.IsNullOrEmpty(titulo))
                    {
                        titulo = propertyAttributes.ToArray()[0].Name;
                        titulo = String.IsNullOrEmpty(titulo) ? campo : titulo;
                    }
                }
                _definicion.columns.Add(new DefinicionJSON.column(titulo,prop.Name,  funRender));
            }
            if (! String.IsNullOrEmpty(urlSelec))
            {
                _definicion.objSelectKey = new DefinicionJSON.SelectKey(indexSelec, urlSelec);
            }

        }
    }
}
