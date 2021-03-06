﻿using System.Diagnostics.Contracts;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharper.ContractExtensions.ContextActions.ContractsFor;
using ReSharper.ContractExtensions.ContextActions.Infrastructure;
using ReSharper.ContractExtensions.Utilities;

namespace ReSharper.ContractExtensions.ContextActions.Requires
{
    /// <summary>
    /// Combo actions' availability that will check is it possible to add ContractClass attribute and
    /// then to add precondition.
    /// </summary>
    /// <remarks>
    /// Combo requires will try to add ContractClass attribute for the interface or abstract class and then
    /// will add Contract.Requires statement.
    /// </remarks>
    internal sealed class ComboRequiresAvailability : ContextActionAvailabilityBase<ComboRequiresAvailability>
    {
        private readonly string _parameterName;
        private readonly IClrTypeName _parameterType;
        private readonly ICSharpFunctionDeclaration _selectedAbstractMethod;
        private readonly AddContractClassAvailability _addContractAvailability;

        public ComboRequiresAvailability()
        {}

        public ComboRequiresAvailability(ICSharpContextActionDataProvider provider)
            : base(provider)
        {
            Contract.Requires(provider != null);

            _selectedAbstractMethod = GetSelectedFunctionDeclaration();

            if (_selectedAbstractMethod != null
                && IsAbstractClassOrInterface() 
                && IsRequiresAvailableFor(out _parameterName, out _parameterType) 
                && CanAddContractForSelectedMethod(_selectedAbstractMethod, out _addContractAvailability))
            {
                _isAvailable = true;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!IsAvailable || _provider != null);
            Contract.Invariant(!IsAvailable || _parameterName != null);
            Contract.Invariant(!IsAvailable || _parameterType != null);
            Contract.Invariant(!IsAvailable || _selectedAbstractMethod != null);
        }

        public string ParameterName { get { return _parameterName; } }
        public IClrTypeName ParameterType { get { return _parameterType; } }
        public ICSharpFunctionDeclaration SelectedFunction { get { return _selectedAbstractMethod; } }
        public AddContractClassAvailability AddContractAvailability { get { return _addContractAvailability; } }

        private bool IsAbstractClassOrInterface()
        {
            if (_provider.IsSelected<IInterfaceDeclaration>())
                return true;

            var classDeclaration = _provider.GetSelectedElement<IClassDeclaration>(true, true);

            if (classDeclaration == null)
                return false; // disabling if outside the class declaration
            
            return classDeclaration.IsAbstract;
        }

        private bool CanAddContractForSelectedMethod(ICSharpFunctionDeclaration selectedFunction, 
            out AddContractClassAvailability addContractAvailability)
        {
            addContractAvailability = AddContractClassAvailability.IsAvailableForSelectedMethod(_provider, selectedFunction);
            return addContractAvailability.IsAvailable;
        }

        /// <summary>
        /// Return selected function declaration (method declaration or property declaration)
        /// </summary>
        /// <returns></returns>
        [Pure]
        private ICSharpFunctionDeclaration GetSelectedFunctionDeclaration()
        {
            var selectedMethod = _provider.GetSelectedElement<ICSharpFunctionDeclaration>(true, true);
            if (selectedMethod != null)
                return selectedMethod;

            var propertyDeclaration = _provider.GetSelectedElement<IPropertyDeclaration>(true, true);
            if (propertyDeclaration == null || propertyDeclaration.IsAuto)
                return null;

            return propertyDeclaration.AccessorDeclarations.FirstOrDefault(a => a.Kind == AccessorKind.SETTER);
        }
      
        private bool IsRequiresAvailableFor(out string parameterName, out IClrTypeName parameterType)
        {
            parameterName = null;
            parameterType = null;

            var parameterRequiresAvailability = ParameterRequiresAvailability.Create(_provider);
            if (parameterRequiresAvailability.IsAvailable)
            {
                parameterName = parameterRequiresAvailability.ParameterName;
                parameterType = parameterRequiresAvailability.ParameterType;
                return true;
            }

            var propertySetterRequiresAvailability = new PropertySetterRequiresAvailability(_provider);
            if (propertySetterRequiresAvailability.IsAvailable)
            {
                parameterName = "value";
                parameterType = propertySetterRequiresAvailability.PropertyType;
                return true;
            }

            return false;
        }
    }
}