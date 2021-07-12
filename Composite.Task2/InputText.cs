﻿using System;

namespace Composite.Task2
{
    public class InputText : IComponent
    {
        string name;
        string value;

        public InputText(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string ConvertToString(int depth = 0)
        {
            return $"<inputText name='{this.name}' value='{this.value}'/>";
        }
    }
}
