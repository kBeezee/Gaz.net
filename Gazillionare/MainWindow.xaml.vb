Imports System.ComponentModel

Public Class MainWindow
    Public mp As New Marketplace
    Public MarketReturnObject As New MarketResultsForMainWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MarketReturnObject.Money = 500.0
        lblStatusBar.DataContext = MarketReturnObject
        valMoney.DataContext = MarketReturnObject
        MarketReturnObject.Status = "Welcome"
    End Sub

    Public Class TravelWindowResults
        Private Shared strCity As String = ""
        Private Shared strState As String = ""
        Public Shared Property City() As String
            Get
                Return strCity
            End Get
            Set(value As String)
                strCity = value
            End Set
        End Property
        Public Shared Property State() As String
            Get
                Return strState
            End Get
            Set(value As String)
                strState = value
            End Set
        End Property
    End Class



    Private Sub btnOpenTravelMenu_Click(sender As Object, e As RoutedEventArgs) Handles btnOpenTravelMenu.Click
        Dim newWindow As Window
        Dim TravelReturnValue As New TravelWindowResults

        newWindow = New Window1()
        Window1.CurrentLocation = valLocation.Content
        newWindow.ShowDialog()

        If TravelWindowResults.City = "Go Back" Then

        ElseIf TravelWindowResults.City <> valLocation.Content Then
            valLocation.Content = TravelWindowResults.City
            MarketReturnObject.Money -= 10
            lvMarketplace.ItemsSource = Nothing
            lvMarketplace.ItemsSource = mp.GetCurrentMarketplace(TravelWindowResults.State)

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
            'Buying
            mp.Buy(lvMarketplace, Me)
        ElseIf MainWindow1.tabControl.SelectedItem.name = "tabCargo" Then
            'Selling
            'Things to Check: Enough Cargo, >0 val, <max cargo
            mp.Sell(lvMarketplace, Me)
        End If

        'valMoney.Content = MainWindow.MarketResultsForMainWindow.Money
        lvCargo.ItemsSource = mp.GetOwnedCommodities()
        'valMoney.DataContext = New MarketResultsForMainWindow
    End Sub

End Class
