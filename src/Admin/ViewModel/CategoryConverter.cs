using ITStore.Data;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows;
using System.Linq;

namespace ITStore.Admin.ViewModel
{
    public class CategoryConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is Int32))
                return Binding.DoNothing;

            String name = (String)value;
            CategoryDTO category = (parameter as IEnumerable<CategoryDTO>).FirstOrDefault(c => c.Name == name);

            if (category == null)
                return Binding.DoNothing;

            return category;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is CategoryDTO))
                return DependencyProperty.UnsetValue;

            return ((CategoryDTO)value).Name;
        }
    }
}
