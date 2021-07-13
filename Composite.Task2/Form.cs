using System;
using System.Collections.Generic;
using System.Text;

namespace Composite.Task2
{
    public class Form : IComponent
    {
        String name;

        private IList<IComponent> FormComponents { get; }

        public Form(String name)
        {
            this.name = name;
            FormComponents = new List<IComponent>();
        }

        public void AddComponent(IComponent component)
        {
            FormComponents.Add(component);
        }

        public string ConvertToString(int depth = 0)
        {
            var builder = new StringBuilder();
            var externalSpace = new string(' ', depth);
            builder.AppendLine($"<form name='{this.name}'>");
            foreach (var component in FormComponents)
            {
                var space = new string(' ', depth + 1);
                if (component is Form)
                {
                    depth += 1;
                }
                builder.AppendLine($"{space}{component.ConvertToString(depth)}");
            }
            builder.Append($"{externalSpace}</form>");

            return builder.ToString();
        }
    }
}