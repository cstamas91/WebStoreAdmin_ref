using ITStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace ITStore.Admin.ViewModel.Converters
{
    public class CategoryCollectionConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object paramter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is IEnumerable<CategoryDTO>))
                return Binding.DoNothing;

            return (value as IEnumerable<CategoryDTO>).Select(category => category.Name);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
