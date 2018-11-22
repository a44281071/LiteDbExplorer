using LiteDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LiteDbExplorer.Converters
{
  [ValueConversion(typeof(Guid?), typeof(string))]
  public class BsonValueToGuidConverter : IValueConverter
  {
    private static readonly Lazy<BsonValueToGuidConverter> _Current = new Lazy<BsonValueToGuidConverter>(() => new BsonValueToGuidConverter());
    public static BsonValueToGuidConverter Current => _Current.Value;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null
        ? null
        : ((BsonValue)value).AsGuid.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string vvv = value?.ToString();
      if (!String.IsNullOrWhiteSpace(vvv))
      {
        if (Guid.TryParse(vvv, out Guid g))
        {
          return new BsonValue(g);
        }
      }
      return null;
    }
  }
}