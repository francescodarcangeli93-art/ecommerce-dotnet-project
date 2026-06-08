using DTO_Ecommerce.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DTO_Ecommerce.Extensions
{
    public static class DataTableExtension
    {

        /// <summary>
        /// Converte il datarow in un oggetto generico
        /// </summary>
        /// <typeparam name="T">Oggetto generico</typeparam>
        /// <param name="row">DataRow</param>
        /// <returns></returns>
        public static T ToObject<T>(this DataRow row) where T : class, new()
        {
            var listOfProps = typeof(T).GetProperties();

            return row.ToObject<T>(listOfProps);
        }


        /// <summary>
        /// Converte il DataRow in un oggetto generico
        /// </summary>
        /// <typeparam name="T">Oggetto generico</typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="props">Lista di proprietà dell'oggetto</param>
        /// <returns>L'istanza del relativo oggetto generico</returns>
        public static T ToObject<T>(this DataRow row, PropertyInfo[] props) where T : class, new()
        {
            T obj = new T();

            foreach (var prop in props)
            {
                try
                {
                    var fieldAttr = (DbFieldAttribute)prop.GetCustomAttributes(typeof(DbFieldAttribute), true).FirstOrDefault();

                    var fieldName = (fieldAttr == null) ? prop.Name : fieldAttr.FieldName ?? prop.Name;

                    if (prop.PropertyType.IsGenericType && prop.PropertyType.Name.Contains("Nullable"))
                    {
                        if (!string.IsNullOrEmpty(row[fieldName].ToString()))
                            prop.SetValue(obj, Convert.ChangeType(row[fieldName], Nullable.GetUnderlyingType(prop.PropertyType), null), null);
                        //else do nothing
                    }
                    else
                        prop.SetValue(obj, Convert.ChangeType(row[fieldName], prop.PropertyType), null);
                }
                catch
                {
                    continue;
                }
            }
            return obj;
        }

        /// <summary>
        /// Converte il DataTable in una lista di oggetti generici
        /// </summary>
        /// <typeparam name="T">Oggetto generico</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>Lista di oggetti generici</returns>
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var list = new List<T>();

                var listOfProps = typeof(T).GetProperties();

                foreach (var row in table.AsEnumerable())
                {
                    var obj = row.ToObject<T>(listOfProps);

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    var obj = row.ToObject<T>();

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }
            return tb;
        }
    }
}
