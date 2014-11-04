using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Raticon.Model
{
    public static class FilmFactory<T>
    {
        public static T BuildFilm(params object[] args)
        {
            return (T)CreateInstance(typeof(T), args);
        }

        //Fix Activator.CreateInstance so it handles Constructor with optional params
        private static object CreateInstance(Type type, params object[] args)
        {
            BindingFlags flags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding;
            return Activator.CreateInstance(type, flags, null, args, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
