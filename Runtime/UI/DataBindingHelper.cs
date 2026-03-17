// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.
// Based on DataBindingHelper by Adam Myhre (adammyhre)

using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Static helpers for wiring UI Toolkit VisualElements to data sources at runtime.
    /// Wraps DataBinding creation to reduce boilerplate for common binding modes.
    /// </summary>
    public static class DataBindingHelper
    {
        static string GetDefaultTargetProperty<T>() where T : VisualElement
        {
            return typeof(T) == typeof(Label) ? "text" : "value";
        }

        static void Bind(VisualElement element, string propertyPath, BindingMode bindingMode, string targetProperty)
        {
            if (element == null)
            {
                Debug.LogWarning($"Cannot bind to null element for property: {propertyPath}");
                return;
            }

            if (element is not BindableElement bindableElement)
            {
                Debug.LogWarning($"Element type {element.GetType().Name} does not support binding. It must inherit from BindableElement.");
                return;
            }

            bindableElement.bindingPath = propertyPath;

            DataBinding binding = new DataBinding
            {
                dataSourcePath = new PropertyPath(propertyPath),
                bindingMode = bindingMode
            };

            element.SetBinding(targetProperty, binding);
        }

        static void Bind(VisualElement element, PropertyPath propertyPath, BindingMode bindingMode, string targetProperty)
        {
            if (element == null)
            {
                Debug.LogWarning($"Cannot bind to null element for property: {propertyPath}");
                return;
            }

            if (element is not BindableElement bindableElement)
            {
                Debug.LogWarning($"Element type {element.GetType().Name} does not support binding.");
                return;
            }

            bindableElement.bindingPath = propertyPath.ToString();

            DataBinding binding = new DataBinding
            {
                dataSourcePath = propertyPath,
                bindingMode = bindingMode
            };

            element.SetBinding(targetProperty, binding);
        }

        public static void Bind<T>(T element, string propertyPath, BindingMode bindingMode) where T : VisualElement
        {
            string targetProperty = GetDefaultTargetProperty<T>();
            Bind((VisualElement)element, propertyPath, bindingMode, targetProperty);
        }

        public static void Bind<T>(T element, PropertyPath propertyPath, BindingMode bindingMode) where T : VisualElement
        {
            string targetProperty = GetDefaultTargetProperty<T>();
            Bind((VisualElement)element, propertyPath, bindingMode, targetProperty);
        }

        public static void Bind<T>(T element, string propertyPath, BindingMode bindingMode, string targetProperty) where T : VisualElement
        {
            Bind((VisualElement)element, propertyPath, bindingMode, targetProperty);
        }

        public static void Bind<T>(T element, PropertyPath propertyPath, BindingMode bindingMode, string targetProperty) where T : VisualElement
        {
            Bind((VisualElement)element, propertyPath, bindingMode, targetProperty);
        }

        /// <summary>Source → Element, one-way.</summary>
        public static void BindOneWay<T>(T element, string propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.ToTarget);
        }

        /// <summary>Source → Element, one-way.</summary>
        public static void BindOneWay<T>(T element, PropertyPath propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.ToTarget);
        }

        /// <summary>Element → Source, one-way.</summary>
        public static void BindToSource<T>(T element, string propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.ToSource);
        }

        /// <summary>Element → Source, one-way.</summary>
        public static void BindToSource<T>(T element, PropertyPath propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.ToSource);
        }

        /// <summary>Source ↔ Element, two-way.</summary>
        public static void BindTwoWay<T>(T element, string propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.TwoWay);
        }

        /// <summary>Source ↔ Element, two-way.</summary>
        public static void BindTwoWay<T>(T element, PropertyPath propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.TwoWay);
        }

        /// <summary>Source → Element, once only.</summary>
        public static void BindToTargetOnce<T>(T element, string propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.ToTargetOnce);
        }

        /// <summary>Source → Element, once only.</summary>
        public static void BindToTargetOnce<T>(T element, PropertyPath propertyPath) where T : VisualElement
        {
            Bind(element, propertyPath, BindingMode.ToTargetOnce);
        }
    }
}
