
Public Class MainWindow
    Public mp As New Marketplace

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

    Public Class MarketResultsForMainWindow
        Private Shared decMoney As Decimal = 500.0
        Private Shared lngTotalCargo As Long = 0
        Private Shared strStatus As String = 0
        Public Shared Property Money() As Decimal
            Get
                Return decMoney
            End Get
            Set(value As Decimal)
                decMoney = value
            End Set
        End Property
        Public Shared Property TotalCargo() As Decimal
            Get
                Return lngTotalCargo
            End Get
            Set(value As Decimal)
                lngTotalCargo = value
            End Set
        End Property
        Public Shared Property Status() As String
            Get
                Return strStatus
            End Get
            Set(value As String)
                strStatus = value
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
            MarketResultsForMainWindow.Money -= 10
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
        MarketResultsForMainWindow.Money = 500.0
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
        Dim fullPrice As Long
        If MainWindow1.tabControl.SelectedItem.name = "tabMarketplace" Then
            'Buying
            mp.Buy(lvMarketplace)
        ElseIf MainWindow1.tabControl.SelectedItem.name = "tabCargo" Then
            'Selling
            'Things to Check: Enough Cargo, >0 val, <max cargo

        End If

        'valMoney.Content = MainWindow.MarketResultsForMainWindow.Money
        lvCargo.ItemsSource = mp.GetOwnedCommodities()
    End Sub
End Class
