﻿using System;

namespace Composite.Task1
{
    public class LabelText
    {
        string value;

        public LabelText(string value)
        {
            this.value = value;
        }

        public string ConvertToString()
        {
            return $"<label value='{this.value}'/>";
        }
    }
}
