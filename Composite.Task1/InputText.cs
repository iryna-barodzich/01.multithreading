﻿using System;

namespace Composite.Task1
{
    public class InputText
    {
        string name;
        string value;

        public InputText(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string ConvertToString()
        {
            return $"<inputText name='{this.name}' value='{this.value}'/>";
        }
    }
}
