﻿#pragma checksum "..\..\..\..\Views\TableSectionView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02C1D481AFA773F5CDF1CA9BCB59C6C6E8691FE6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using C968_Final.Views;
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


namespace C968_Final.Views {
    
    
    /// <summary>
    /// TableSectionView
    /// </summary>
    public partial class TableSectionView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\Views\TableSectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal C968_Final.Views.TableSectionView root;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\Views\TableSectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SearchButton;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Views\TableSectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SearchText;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Views\TableSectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddBttn;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Views\TableSectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ModiftyBttn;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Views\TableSectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteBttn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/C968_Final;component/views/tablesectionview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\TableSectionView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.root = ((C968_Final.Views.TableSectionView)(target));
            return;
            case 2:
            this.SearchButton = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\..\Views\TableSectionView.xaml"
            this.SearchButton.Click += new System.Windows.RoutedEventHandler(this.SearchButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SearchText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.AddBttn = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\..\Views\TableSectionView.xaml"
            this.AddBttn.Click += new System.Windows.RoutedEventHandler(this.AddBttn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ModiftyBttn = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\..\Views\TableSectionView.xaml"
            this.ModiftyBttn.Click += new System.Windows.RoutedEventHandler(this.ModiftyBttn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DeleteBttn = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\Views\TableSectionView.xaml"
            this.DeleteBttn.Click += new System.Windows.RoutedEventHandler(this.DeleteBttn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
