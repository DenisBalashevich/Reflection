using ReflectionContainer.Attributes;
using ReflectionContainer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionContainer
{
    public class ReflectionContainerInjector
    {
        private readonly IDictionary<Type, Type> _typesDictionary;
        private readonly IInstanceActivator _activator;

        public ReflectionContainerInjector(IInstanceActivator activator)
        {
            _activator = activator;
            _typesDictionary = new Dictionary<Type, Type>();
        }

        public void RegisterAssemblyTypes(Assembly assembly)
        {
            var types = assembly.ExportedTypes;
            foreach (var type in types)
            {
                RegisterImportType(type);
                RegisterExportType(type);
            }
        }

        public void AddType(Type type)
        {
            _typesDictionary.Add(type, type);
        }

        public void AddType(Type type, Type baseType)
        {
            _typesDictionary.Add(baseType, type);
        }

        public object CreateInstance(Type type)
        {
            return ConstructInstanceOfType(type);
        }

        public T CreateInstance<T>()
        {
            var type = typeof(T);
            return (T)ConstructInstanceOfType(type);
        }

        private object ConstructInstanceOfType(Type type)
        {
            if (!_typesDictionary.ContainsKey(type))
            {
                throw new ContainerException();
            }

            var dependendType = _typesDictionary[type];
            var constructorInfo = GetConstructor(dependendType);
            var instance = CreateFromConstructor(dependendType, constructorInfo);

            if (dependendType.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                return instance;
            }

            ResolveProperties(dependendType, instance);
            return instance;
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            if (constructors.Length == 0)
            {
                  throw new ContainerException($"Type {type.Name} not contain public constructor");
            }

            return constructors.First();
        }

        private object CreateFromConstructor(Type type, ConstructorInfo constructorInfo)
        {
            var parametersInfo = constructorInfo.GetParameters();
            var parametersInstances = new List<object>(parametersInfo.Length);
            parametersInstances.AddRange(parametersInfo.Select(p => ConstructInstanceOfType(p.ParameterType)));

            return _activator.CreateInstance(type, parametersInstances.ToArray());
        }

        private IEnumerable<PropertyInfo> ImportedProperties(Type type)
        {
            return type.GetProperties().Where(p => p.GetCustomAttribute<ImportAttribute>() != null);
        }

        private void ResolveProperties(Type type, object instance)
        {
            var importedProperties = ImportedProperties(type);
            foreach (var property in importedProperties)
            {
                var resolvedProperty = ConstructInstanceOfType(property.PropertyType);
                property.SetValue(instance, resolvedProperty);
            }
        }

        private void RegisterImportType(Type type)
        {
            var importConstructorAttribute = type.GetCustomAttribute<ImportConstructorAttribute>();
            var importProperties = ImportedProperties(type);
            if (!ReferenceEquals(importConstructorAttribute, null) || !ReferenceEquals(importProperties, null))
            {
                _typesDictionary.Add(type, type);
            }
        }

        private void RegisterExportType(Type type)
        {
            var exportAttributes = type.GetCustomAttributes<ExportAttribute>();
            foreach (var exportAttribute in exportAttributes)
            {
                _typesDictionary.Add(exportAttribute.Contract ?? type, type);
            }
        }
    }
}
