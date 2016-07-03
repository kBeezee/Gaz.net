Imports System.ComponentModel

Public Class Marketplace
    Private Shared arrCurrentData As List(Of Commodity)
    Private Shared arrStartingData As List(Of Commodity)
    Private Shared arrMyCargo As List(Of Commodity)

    Public Shared Property CurrentData As List(Of Commodity)
        Get
            Return arrCurrentData
        End Get
        Set(value As List(Of Commodity))
            arrCurrentData = value
        End Set
    End Property
    Public Shared Property StartingData As List(Of Commodity)
        Get
            Return arrStartingData
        End Get
        Set(value As List(Of Commodity))
            arrStartingData = value
        End Set
    End Property
    Public Sub Init()
        Dim rand As New Random
        Dim dt As GazDataDataSet.tblMarketItemsDataTable = New GazDataDataSetTableAdapters.tblMarketItemsTableAdapter().GetData()
        Dim rows() As GazDataDataSet.tblMarketItemsRow = dt.Select()
        Dim arrTemp As Array, arrStartingData As New List(Of Commodity)
        Dim comTemp As New Commodity

        arrTemp = rows.ToArray()
        For Each n In arrTemp
            comTemp = New Commodity
            comTemp.Name = n.fcommodity
            comTemp.MaxPrice = n.fmaxprice
            comTemp.MinPrice = n.fminprice
            comTemp.State = n.fState
            arrStartingData.Add(comTemp)
        Next

        StartingData = arrStartingData
    End Sub
    Public Function GetCurrentMarketplace(strState As String, Optional boolChangePrices As Boolean = True) As List(Of Commodity)
        Dim r As New Random
        Dim All As String = "All"
        Dim output As New List(Of Commodity)
        Dim x As New Commodity

        If boolChangePrices = False Then
            For Each n In CurrentData
                output.Add(n)
            Next
            Return output
            Exit Function
        End If
        'Everything specific to that state
        For Each n In StartingData
            n.Price = r.Next(n.MinPrice, n.MaxPrice)
            If n.State = strState Then
                output.Add(n)
            End If
        Next

        'and one random generic place
        x = StartingData(r.Next(0, StartingData.Count))
        Do Until x.State = "All"
            x = StartingData(r.Next(0, StartingData.Count))
        Loop
        output.Add(x)

        Return output
    End Function
    Public Function GetOwnedCommodities() As List(Of Commodity)
        Dim output As New List(Of Commodity)
        Dim n As New Commodity

        For Each n In CurrentData
            If n.QuantityOwned > 0 Then
                output.Add(n)
            End If
        Next

        Return output
    End Function
    Public Sub Buy(lvMarketplace As ListView, mw1 As MainWindow)
        Dim decTotalTransactionPrice As Decimal, lngTotalTransactionCargo As Long, lngTotalCurrentnCargo As Long
        'Things to Check: Enough Money, Enough Cargo Area
        For Each n In CurrentData
            If n.QuantityOwned > 0 Then
                lngTotalCurrentnCargo += n.QuantityOwned
            End If
            If n.QuantityBuying > 0 Then
                decTotalTransactionPrice += n.QuantityBuying * n.Price
                lngTotalTransactionCargo += n.QuantityBuying
            End If
        Next

        If mw1.MarketReturnObject.Money < decTotalTransactionPrice Then
            mw1.MarketReturnObject.Status = "Not enough money."
            Exit Sub
        End If

        'Once all checks are done, do this.
        For Each n In CurrentData
            If n.QuantityBuying > 0 Then
                n.QuantityOwned += n.QuantityBuying
                n.QuantityBuying = 0
            End If
        Next
        lvMarketplace.ItemsSource = Nothing
        lvMarketplace.ItemsSource = Me.CurrentData

        mw1.MarketReturnObject.Money -= decTotalTransactionPrice
        mw1.MarketReturnObject.TotalCargo += lngTotalTransactionCargo
        mw1.MarketReturnObject.Status = "Spent " + decTotalTransactionPrice.ToString()
    End Sub
    Public Sub Sell(lvCargo As ListView, mw1 As MainWindow)
        'Things to Check: Enough Cargo, >0 val, <max cargo
        Dim decTotalSale As Decimal, lngTotalCargo As Long
        For Each n In GetOwnedCommodities()
            If n.QuantityOwned > 0 Then
                If n.QuantitySelling > n.QuantityOwned Then
                    mw1.MarketReturnObject.Status = "You only own " + n.QuantityOwned.ToString() + " " + n.Name
                    Exit Sub
                End If
                If n.QuantitySelling < 0 Then
                    mw1.MarketReturnObject.Status = "Cannot sell negative " + n.Name
                    Exit Sub
                End If
                decTotalSale += n.QuantitySelling * n.Price
                lngTotalCargo -= n.QuantitySelling
            End If
        Next

        If decTotalSale > 0 Then
            For Each n In GetOwnedCommodities()
                If n.QuantitySelling > 0 Then
                    n.QuantityOwned -= n.QuantitySelling
                End If
            Next
        End If

        lvCargo.ItemsSource = Nothing
        lvCargo.ItemsSource = GetCurrentMarketplace("", False)
        mw1.MarketReturnObject.Money += decTotalSale
        mw1.MarketReturnObject.TotalCargo += lngTotalCargo
        mw1.MarketReturnObject.Status = "Earned " + decTotalSale.ToString()



    End Sub
End Class

Public Class Commodity
    Private strName As String
    Private curPrice As Decimal
    Private curMinPrice As Decimal
    Private curMaxPrice As Decimal
    Private strState As String
    Private lngQuantityBuying As Long
    Private lngQuantitySelling As Long
    Private lngQuantityOwned As Long
    Public Property Name As String
        Get
            Return strName
        End Get
        Set(value As String)
            strName = value
        End Set
    End Property
    Public Property Price As Decimal
        Get
            Return curPrice
        End Get
        Set(value As Decimal)
            curPrice = value
        End Set
    End Property
    Public Property MaxPrice As Decimal
        Get
            Return curMaxPrice
        End Get
        Set(value As Decimal)
            curMaxPrice = value
        End Set
    End Property
    Public Property MinPrice As Decimal
        Get
            Return curMinPrice
        End Get
        Set(value As Decimal)
            curMinPrice = value
        End Set
    End Property
    Public Property State As String
        Get
            Return strState
        End Get
        Set(value As String)
            strState = value
        End Set
    End Property
    Public Property QuantityBuying As Long
        Get
            Return lngQuantityBuying
        End Get
        Set(value As Long)
            lngQuantityBuying = value
        End Set
    End Property

    Public Property QuantitySelling As Long
        Get
            Return lngQuantitySelling
        End Get
        Set(value As Long)
            lngQuantitySelling = value
        End Set
    End Property

    Public Property QuantityOwned As Long
        Get
            Return lngQuantityOwned
        End Get
        Set(value As Long)
            lngQuantityOwned = value
        End Set
    End Property
End Class
Public Class MarketResultsForMainWindow
    Implements INotifyPropertyChanged
    Private Shared decMoney As Decimal = 500.0
    Private Shared lngTotalCargo As Long = 0
    Private Shared strStatus As String = 0
    Public Property Money() As Decimal
        Get
            Return decMoney
        End Get
        Set(value As Decimal)
            decMoney = value
            OnPropertyChanged(New PropertyChangedEventArgs("Money"))

        End Set
    End Property
    Public Property TotalCargo() As Decimal
        Get
            Return lngTotalCargo
        End Get
        Set(value As Decimal)
            lngTotalCargo = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return strStatus
        End Get
        Set(value As String)
            strStatus = value
            OnPropertyChanged(New PropertyChangedEventArgs("Status"))
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
        If Not PropertyChangedEvent Is Nothing Then
            RaiseEvent PropertyChanged(Me, e)
        End If
    End Sub
End Class