using System;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Service_Tag_Reporter;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace ServiceTagTests
{
    public class MainAppTests
    {
        #region Variables

        MainWindow _mainWindow;

        #endregion Variables

        #region Setup

        public MainAppTests()
        {
            _mainWindow = new MainWindow();
        }

        #endregion Setup

        #region Private Methods

        #endregion Private Methods

        #region Tests

        [Fact]
        public void MainWindow_Startup_Succeeds()
        {
            _mainWindow.Should().NotBeNull();
        }

        [Fact]
        public void MainWindow_HasA_FetchButton()
        {
            _mainWindow.btnFetch.Should().NotBeNull();
            _mainWindow.btnFetch.Visibility.Should().Be(Visibility.Visible);
        }

        [Fact]
        public void MainWindow_HasA_QueryButton()
        {
            _mainWindow.btnFind.Should().NotBeNull();
            _mainWindow.btnFind.Visibility.Should().Be(Visibility.Visible);
        }

        [Fact]
        public void MainWindow_HasTextBox_ForInput()
        {
            _mainWindow.txtServiceTags.Should().NotBeNull();
            _mainWindow.txtServiceTags.Visibility.Should().Be(Visibility.Visible);
        }

        [Fact]
        public void MainWindow_HasGrid_ForOutput()
        {
            _mainWindow.grdWarrantyView.Should().NotBeNull();
            _mainWindow.grdWarrantyView.Visibility.Should().Be(Visibility.Visible);
        }

        [Fact]
        public void MainWindow_QueryButton_ActivatesServiceTagFetch_WhenPressed()
        {
            _mainWindow.txtServiceTags.Text = "123ABCT";
            _mainWindow.btnFetch.PerformClick();
        }

        [Fact]
        public void MainWindow_DataGrid_UpdatesWhen_DataSetUpdated()
        {
            (_mainWindow.grdWarrantyView.ItemsSource as DataView).Count.Should().Be(0);
            DellWarranty.WarrantyEngine.dataTable.Rows.Add("Model", DateTime.Today, DateTime.Today);
            (_mainWindow.grdWarrantyView.ItemsSource as DataView).Count.Should().Be(1);
        }

        [Fact]
        public void MainWIndow_Menu_HideOrShowQueryElements_ShowsByDefault()
        {
            //verify initial load
            _mainWindow.mnuToggleScout.Visibility.Should().Be(Visibility.Visible);
            _mainWindow.mnuToggleScout.IsChecked.Should().BeTrue();
        }

        [Fact]
        public void MainWindow_Menu_CanHideOrShowQueryElements()
        {
            _mainWindow.txtQueryTarget.Visibility.Should().Be(Visibility.Visible);
            _mainWindow.btnFind.Visibility.Should().Be(Visibility.Visible);
            _mainWindow.mnuToggleScout.IsChecked = false;
            _mainWindow.txtQueryTarget.Visibility.Should().Be(Visibility.Collapsed);
            _mainWindow.btnFind.Visibility.Should().Be(Visibility.Collapsed);

        }

        #endregion Tests
    }

    //TODO: Move to Test Case or to own class
    public static class ButtonExtension
    {
        public static void PerformClick(this Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
