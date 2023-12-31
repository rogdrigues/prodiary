﻿#pragma checksum "..\..\..\..\MusicListening\AddSongDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5F8AC7B61DACD01012D2E76914E8A579E63B3FF5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using ProDiaryApplication.MusicListening;
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


namespace ProDiaryApplication.MusicListening {
    
    
    /// <summary>
    /// AddSongDialog
    /// </summary>
    public partial class AddSongDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Title;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Category;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Time;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Status;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Author;
        
        #line default
        #line hidden
        
        
        #line 148 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ImageURL;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LinkToFile;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button chooseFileButton;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\..\..\MusicListening\AddSongDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addSong;
        
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
            System.Uri resourceLocater = new System.Uri("/ProDiaryApplication;V1.0.0.0;component/musiclistening/addsongdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MusicListening\AddSongDialog.xaml"
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
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\..\MusicListening\AddSongDialog.xaml"
            this.CloseButton.Click += new System.Windows.RoutedEventHandler(this.CloseButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Title = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Category = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.Time = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Status = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.Author = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.ImageURL = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.LinkToFile = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.chooseFileButton = ((System.Windows.Controls.Button)(target));
            
            #line 162 "..\..\..\..\MusicListening\AddSongDialog.xaml"
            this.chooseFileButton.Click += new System.Windows.RoutedEventHandler(this.chooseFileButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.addSong = ((System.Windows.Controls.Button)(target));
            
            #line 171 "..\..\..\..\MusicListening\AddSongDialog.xaml"
            this.addSong.Click += new System.Windows.RoutedEventHandler(this.addSong_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 178 "..\..\..\..\MusicListening\AddSongDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

