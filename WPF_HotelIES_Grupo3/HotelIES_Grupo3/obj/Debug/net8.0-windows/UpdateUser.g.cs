﻿#pragma checksum "..\..\..\UpdateUser.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8044BC1D06FCE523419E9429F68EC73310BE534D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using HotelIES_Grupo3;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace HotelIES_Grupo3 {
    
    
    /// <summary>
    /// UpdateUser
    /// </summary>
    public partial class UpdateUser : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 180 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDni;
        
        #line default
        #line hidden
        
        
        #line 189 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox rolSeleccionado;
        
        #line default
        #line hidden
        
        
        #line 204 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker txtFecha_nac;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNombre;
        
        #line default
        #line hidden
        
        
        #line 228 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtEmail;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCiudad;
        
        #line default
        #line hidden
        
        
        #line 245 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtApellido;
        
        #line default
        #line hidden
        
        
        #line 252 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox txtPassword;
        
        #line default
        #line hidden
        
        
        #line 259 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton hombre;
        
        #line default
        #line hidden
        
        
        #line 260 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton mujer;
        
        #line default
        #line hidden
        
        
        #line 261 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton indeterminado;
        
        #line default
        #line hidden
        
        
        #line 277 "..\..\..\UpdateUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgPreview;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/HotelIES_Grupo3;component/updateuser.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UpdateUser.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 167 "..\..\..\UpdateUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtDni = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.rolSeleccionado = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.txtFecha_nac = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.txtNombre = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtEmail = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.txtCiudad = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.txtApellido = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.txtPassword = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 10:
            this.hombre = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 11:
            this.mujer = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 12:
            this.indeterminado = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 13:
            this.imgPreview = ((System.Windows.Controls.Image)(target));
            return;
            case 14:
            
            #line 286 "..\..\..\UpdateUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_SelectImage_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 295 "..\..\..\UpdateUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnActualizar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

