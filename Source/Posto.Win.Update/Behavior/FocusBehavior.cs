using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Posto.Win.Update.Behavior
{
    public static class FocusBehavior
    {
        #region IsFocused - Obsoleto
        
        /*
        * Método antigo para setar o focus
        * */
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusBehavior), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));
        
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }
        
        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }
        
        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (UIElement)d;
            
            if ((bool)e.NewValue)
            {
                target.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (target.Focusable)
                    {
                        if (target is Xceed.Wpf.Toolkit.MaskedTextBox)
                        {
                            var maskedInput = ((Xceed.Wpf.Toolkit.MaskedTextBox)target);

                            maskedInput.Focus();
                        }
                        else if (target is TextBox)
                        {
                            target.Focus();
                            ((TextBox)target).SelectAll();                            
                        }
                        else if (target is PasswordBox)
                        {
                            target.Focus();
                            ((PasswordBox)target).SelectAll();
                        }
                        else 
                        {
                            target.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }
                    }
                }), DispatcherPriority.Input);
            }
        }
        
        #endregion
        
        #region Focus Element - Novo Método
        
        /**
        * Sugestão de implementação no ViewModel:
        * 
        * Não passar string para dar o focus. Isto pode ocorrer no caso de alterar o nome do controle e ter o focus em vários lugares do ViewModel.
        * Talvez seja melhor passar um Enum com o nome dos campos. E criar uma função SetFocus(Enum);
        * 
        * Um exemplo completo pode ser visto no SangriSuprimentoViewModel.
        * 
        * */
        
        /// <summary>
        /// Behavior para colocar o focus nos controles, passando o Name do campo. 
        /// Já está funcionando corretamente, agora necessita alterar em todos os View e ViewModels
        /// </summary>
        public static readonly DependencyProperty FocusElementProperty =
            DependencyProperty.RegisterAttached("FocusElement", typeof(string), typeof(FocusBehavior), new UIPropertyMetadata(string.Empty, OnFocusElementPropertyChanged));
        
        public static string GetFocusElement(DependencyObject obj)
        {
            return (string)obj.GetValue(FocusElementProperty);
        }
        
        public static void SetFocusElement(DependencyObject obj, string value)
        {
            obj.SetValue(FocusElementProperty, value);
        }
        
        private static void OnFocusElementPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var elementName = (string)e.NewValue;
            
            if (string.IsNullOrWhiteSpace(elementName))
            {
                return;
            }
            
            var frameworkElement = obj as FrameworkElement;
            
            // Busca o elemento pelo nome
            var target = (UIElement)frameworkElement.FindName(elementName);
            
            if (target == null)
            {
                return;
            }
            
            target.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!target.Focusable || !target.IsEnabled)
                {
                    return;
                }

                if (target is Xceed.Wpf.Toolkit.MaskedTextBox)
                {
                    var maskedInput = (Xceed.Wpf.Toolkit.MaskedTextBox)target;

                    maskedInput.Focus();
                }
                else if (target is TextBox)
                {
                    target.Focus();                    
                    ((TextBox)target).SelectAll();
                }
                else if (target is PasswordBox)
                {
                    target.Focus();
                    ((PasswordBox)target).SelectAll();
                }
                else
                {
                    target.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    Keyboard.Focus(target);
                }
            }), DispatcherPriority.Input);
            
            //
            // Reseta o valor, senão não irá setar o focus novamente
            var bindingExpression = BindingOperations.GetBindingExpression(obj, FocusElementProperty);
            if (bindingExpression != null)
            {
                var property = bindingExpression.DataItem.GetType().GetProperty(bindingExpression.ParentBinding.Path.Path);
                if (property != null)
                {
                    property.SetValue(bindingExpression.DataItem, string.Empty, null);
                }
            }
            else
            {
                SetFocusElement(obj, string.Empty);
            }
        }
        
        #endregion
    }
}