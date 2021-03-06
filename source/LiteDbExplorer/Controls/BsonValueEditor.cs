﻿using LiteDB;
using LiteDbExplorer.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace LiteDbExplorer.Controls
{
  public class BsonValueEditor
  {
    public static FrameworkElement GetBsonValueEditor(string bindingPath, BsonValue bindingValue, object bindingSource, bool readOnly)
    {
      var binding = new Binding()
      {
        Path = new PropertyPath(bindingPath),
        Source = bindingSource,
        Mode = BindingMode.TwoWay,
        Converter = BsonValueToNetValueConverter.Current,
        UpdateSourceTrigger = UpdateSourceTrigger.Explicit
      };
      var guidBinding = new Binding()
      {
        Path = new PropertyPath(bindingPath),
        Source = bindingSource,
        Mode = BindingMode.TwoWay,
        Converter = BsonValueToGuidConverter.Current,
        UpdateSourceTrigger = UpdateSourceTrigger.Default
      };
      if (bindingValue.IsArray)
      {
        var button = new Button()
        {
          Content = $"Array({bindingValue.AsArray.Count})"
        };

        button.Click += (s, a) =>
        {
          var arrayValue = bindingValue as BsonArray;
          var window = new Windows.ArrayViewer(arrayValue, readOnly)
          {
            Owner = Application.Current.MainWindow
          };

          if (window.ShowDialog() == true)
          {
            arrayValue.Clear();
            arrayValue.AddRange(window.EditedItems);
          }
        };

        return button;
      }
      else if (bindingValue.IsBoolean)
      {
        var check = new CheckBox()
        {
          IsEnabled = !readOnly
        };

        check.SetBinding(ToggleButton.IsCheckedProperty, binding);
        return check;
      }
      else if (bindingValue.IsDateTime)
      {
        var datePicker = new DateTimePicker()
        {
          TextAlignment = TextAlignment.Left,
          IsReadOnly = readOnly
        };

        datePicker.SetBinding(DateTimePicker.ValueProperty, binding);
        return datePicker;
      }
      else if (bindingValue.IsDocument)
      {
        var button = new Button()
        {
          Content = $"Document({bindingValue.AsDocument.Count})"
        };

        button.Click += (s, a) =>
        {
          var window = new Windows.DocumentViewer(bindingValue as BsonDocument, readOnly)
          {
            Owner = Application.Current.MainWindow
          };

          window.ShowDialog();
        };

        return button;
      }
      else if (bindingValue.IsDouble)
      {
        var numberEditor = new DoubleUpDown()
        {
          TextAlignment = TextAlignment.Left,
          IsReadOnly = readOnly
        };

        numberEditor.SetBinding(DoubleUpDown.ValueProperty, binding);
        return numberEditor;
      }
      else if (bindingValue.IsInt32)
      {
        var numberEditor = new IntegerUpDown()
        {
          TextAlignment = TextAlignment.Left,
          IsReadOnly = readOnly
        };

        numberEditor.SetBinding(IntegerUpDown.ValueProperty, binding);
        return numberEditor;
      }
      else if (bindingValue.IsInt64)
      {
        var numberEditor = new LongUpDown()
        {
          TextAlignment = TextAlignment.Left,
          IsReadOnly = readOnly
        };

        numberEditor.SetBinding(LongUpDown.ValueProperty, binding);
        return numberEditor;
      }
      else if (bindingValue.IsString)
      {
        var stringEditor = new TextBox()
        {
          IsReadOnly = readOnly,
          AcceptsReturn = true
        };

        stringEditor.SetBinding(TextBox.TextProperty, binding);
        return stringEditor;
      }
      else if (bindingValue.IsBinary)
      {
        var text = new TextBlock()
        {
          Text = "[Binary Data]"
        };

        return text;
      }
      else if (bindingValue.IsObjectId)
      {
        var text = new TextBox()
        {
          Text = bindingValue.AsString,
          IsReadOnly = true
        };
        return text;
      }
      else if (bindingValue.IsGuid)
      {
        var stringEditor = new TextBox();
        stringEditor.SetBinding(TextBox.TextProperty, guidBinding);
        return stringEditor;
      }
      else
      {
        var stringEditor = new TextBox();
        stringEditor.SetBinding(TextBox.TextProperty, binding);
        return stringEditor;
      }
    }
  }
}