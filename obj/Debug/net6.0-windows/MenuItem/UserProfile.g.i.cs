﻿#pragma checksum "..\..\..\..\MenuItem\UserProfile.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02E40427780E0D8CC5B05787D5A4635FCFA5E714"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ProDiaryApplication.MenuItem;
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


namespace ProDiaryApplication.MenuItem {
    
    
    /// <summary>
    /// UserProfile
    /// </summary>
    public partial class UserProfile : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSearch;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border emptyDiary;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtEmail;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtUsername;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFullname;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgAvatar;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowserFile;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\..\MenuItem\UserProfile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSaveProfile;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProDiaryApplication;V1.0.0.0;component/menuitem/userprofile.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MenuItem\UserProfile.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\MenuItem\UserProfile.xaml"
            ((ProDiaryApplication.MenuItem.UserProfile)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtSearch = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.emptyDiary = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.txtEmail = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtUsername = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtFullname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.imgAvatar = ((System.Windows.Controls.Image)(target));
            return;
            case 8:
            this.btnBrowserFile = ((System.Windows.Controls.Button)(target));
            
            #line 114 "..\..\..\..\MenuItem\UserProfile.xaml"
            this.btnBrowserFile.Click += new System.Windows.RoutedEventHandler(this.btnBrowserFile_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnSaveProfile = ((System.Windows.Controls.Button)(target));
            
            #line 117 "..\..\..\..\MenuItem\UserProfile.xaml"
            this.btnSaveProfile.Click += new System.Windows.RoutedEventHandler(this.btnSaveProfile_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

