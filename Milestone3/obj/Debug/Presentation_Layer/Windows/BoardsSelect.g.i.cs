﻿#pragma checksum "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "19FB661806CECFE5A141A21DC2B1B923D135C44DA1341F729661F8FD220E1219"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Milestone3.Presentation_Layer.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Milestone3.Presentation_Layer.Windows {
    
    
    /// <summary>
    /// BoardsSelect
    /// </summary>
    public partial class BoardsSelect : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid BoardsSelectDataGrid;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewBoardName;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddButtonSend;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Milestone3;component/presentation_layer/windows/boardsselect.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.BoardsSelectDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            
            #line 15 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LoadSpecificBoard);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 16 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddingNewBoard);
            
            #line default
            #line hidden
            return;
            case 4:
            this.NewBoardName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.AddButtonSend = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
            this.AddButtonSend.Click += new System.Windows.RoutedEventHandler(this.AddSend);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 19 "..\..\..\..\Presentation_Layer\Windows\BoardsSelect.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveBoardButton);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
