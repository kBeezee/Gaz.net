Imports System.ComponentModel

Public Class MainWindow
    Public mp As New Marketplace
    Public MarketReturnObject As New MarketResultsForMainWindow
    Public twr As New TravelWindowResults

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.

        lblStatusBar.DataContext = MarketReturnObject
        valMoney.DataContext = MarketReturnObject
        valCity.DataContext = twr

        twr.City = "Springfield"
        twr.State = "MO"
        MarketReturnObject.Money = 500.0
        MarketReturnObject.Status = "Welcome"

    End Sub

    Private Sub btnOpenTravelMenu_Click(sender As Object, e As RoutedEventArgs) Handles btnOpenTravelMenu.Click
        Dim TravWindow As New Window1

        TravWindow = New Window1()
        TravWindow.CurrentCity = valCity.Content

        TravWindow.ShowDialog()

        twr.City = TravWindow.GetTravelWindowResults().City

        If twr.City = "Go Back" Then

        ElseIf twr.City <> TravWindow.CurrentCity Then
            MarketReturnObject.Money -= 5
            lvMarketplace.ItemsSource = Nothing
            lvMarketplace.ItemsSource = mp.GetCurrentMarketplace(twr.State)
        End If

        lvCargo.ItemsSource = mp.GetOwnedCommodities()

    End Sub

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        'First time load.
        mp.Init()
        mp.CurrentData = mp.GetCurrentMarketplace("MO")

        'yay data binding, see XML for how this works. The XML has the properties of the class bound
        'to the column they are supposed to be in.  Cool!
        lvMarketplace.ItemsSource = mp.CurrentData
    End Sub

    Private Sub tabChangeHandler() Handles tabMarketplace.GotFocus, tabCargo.GotFocus, gridMainWindow.Loaded
        If MainWindow1.tabControl.SelectedItem.name = "tabMarketplace" Then
            MainWindow1.btnBuy.Visibility = False
            MainWindow1.btnSell.Visibility = True
        ElseIf MainWindow1.tabControl.SelectedItem.name = "tabCargo" Then
            MainWindow1.btnBuy.Visibility = True
            MainWindow1.btnSell.Visibility = False
        End If
    End Sub

    Private Sub btnTradeHandled(sender As Object, e As RoutedEventArgs) Handles btnBuy.Click, btnSell.Click

        If MainWindow1.tabControl.SelectedItem.name = "tabMarketplace" Then
            mp.Buy(lvMarketplace, Me)
        ElseIf MainWindow1.tabControl.SelectedItem.name = "tabCargo" Then
            mp.Sell(lvMarketplace, Me)
        End If

        lvCargo.ItemsSource = mp.GetOwnedCommodities()
    End Sub

End Class
